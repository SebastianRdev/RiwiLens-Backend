using Microsoft.AspNetCore.Identity;
using src.RiwiLens.Application.DTOs.User;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Application.Interfaces.Services;
using src.RiwiLens.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace src.RiwiLens.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IGenericRepository<Coder> _coderRepository;
    private readonly IGenericRepository<TeamLeader> _teamLeaderRepository;
    // Assuming we might need ProfessionalProfile repo if Coder creation requires it, 
    // but Coder.Create takes an ID. We might need to create a default profile first.
    private readonly IGenericRepository<ProfessionalProfile> _profileRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IGenericRepository<ClanCoder> _clanCoderRepository;
    private readonly IGenericRepository<ClanTeamLeader> _clanTlRepository;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IGenericRepository<Coder> coderRepository,
        IGenericRepository<TeamLeader> teamLeaderRepository,
        IGenericRepository<ProfessionalProfile> profileRepository,
        ILogger<UserService> logger,
        IGenericRepository<ClanCoder> clanCoderRepository,
        IGenericRepository<ClanTeamLeader> clanTlRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _coderRepository = coderRepository;
        _teamLeaderRepository = teamLeaderRepository;
        _profileRepository = profileRepository;
        _logger = logger;
        _clanCoderRepository = clanCoderRepository;
        _clanTlRepository = clanTlRepository;
    }

    public async Task<UserResponseDto> CreateUserAsync(CreateUserDto dto)
    {
        _logger.LogInformation($"CreateUserAsync called. Email: {dto.Email}, Role: {dto.Role}, ClanId: {dto.ClanId}");

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            _logger.LogError($"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        try
        {
            if (!string.IsNullOrEmpty(dto.Role))
            {
                if (await _roleManager.RoleExistsAsync(dto.Role))
                {
                    await _userManager.AddToRoleAsync(user, dto.Role);
                }
                else
                {
                    _logger.LogWarning($"Role {dto.Role} does not exist.");
                }
            }

            // Set Defaults for missing fields
            var gender = dto.Gender == Gender.Unknown ? Gender.Other : dto.Gender;
            var docType = dto.DocumentType == DocumentType.Unknown ? DocumentType.CC : dto.DocumentType;
            var identification = string.IsNullOrEmpty(dto.Identification) ? Guid.NewGuid().ToString().Substring(0, 10) : dto.Identification;
            var birthDate = dto.BirthDate == default ? DateTime.UtcNow : dto.BirthDate;

            // Create Profile based on Role
            if (dto.Role == "Coder")
            {
                _logger.LogInformation("Creating Coder entity...");
                // Create default ProfessionalProfile
                var newProfile = ProfessionalProfile.Create(string.Empty); 
                await _profileRepository.AddAsync(newProfile);
                await _profileRepository.SaveChangesAsync();

                var coder = Coder.Create(
                    dto.FullName,
                    user.Id,
                    docType,
                    identification,
                    birthDate,
                    dto.Address ?? "",
                    dto.Country ?? "",
                    dto.City ?? "",
                    gender,
                    newProfile.Id,
                    1 // StatusId 1 = Active (Assumption)
                );
                await _coderRepository.AddAsync(coder);
                await _coderRepository.SaveChangesAsync();
                _logger.LogInformation($"Coder entity created successfully. ID: {coder.Id}");

                if (dto.ClanId.HasValue)
                {
                    _logger.LogInformation($"Assigning Coder to Clan {dto.ClanId.Value}...");
                    var clanCoder = new ClanCoder(dto.ClanId.Value, coder.Id, DateTime.UtcNow, true);
                    await _clanCoderRepository.AddAsync(clanCoder);
                    await _clanCoderRepository.SaveChangesAsync();
                }
            }
            else if (dto.Role == "TeamLeader")
            {
                _logger.LogInformation("Creating TeamLeader entity...");
                var tl = TeamLeader.Create(
                    user.Id,
                    dto.FullName,
                    gender,
                    birthDate
                );
                await _teamLeaderRepository.AddAsync(tl);
                await _teamLeaderRepository.SaveChangesAsync();
                _logger.LogInformation($"TeamLeader entity created successfully. ID: {tl.Id}");

                if (dto.ClanId.HasValue)
                {
                    _logger.LogInformation($"Assigning TeamLeader to Clan {dto.ClanId.Value}...");
                    // Assuming RoleTeamLeaderId = 1 (Default/Main)
                    var clanTl = new ClanTeamLeader(dto.ClanId.Value, tl.Id, 1, DateTime.UtcNow);
                    await _clanTlRepository.AddAsync(clanTl);
                    await _clanTlRepository.SaveChangesAsync();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating entity. Rolling back user creation.");
            // Rollback: Delete the user if entity creation fails
            await _userManager.DeleteAsync(user);
            throw;
        }

        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email!,
            Roles = new List<string> { dto.Role },
        };
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        var users = _userManager.Users.ToList();
        var dtos = new List<UserResponseDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            dtos.Add(new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email!,
                Roles = roles
            });
        }

        return dtos;
    }

    public async Task<UserResponseDto?> GetByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);
        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email!,
            Roles = roles
        };
    }

    public async Task<UserResponseDto> UpdateUserAsync(string id, UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) throw new KeyNotFoundException($"User with ID {id} not found.");

        if (!string.IsNullOrEmpty(dto.Email) && dto.Email != user.Email)
        {
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, dto.Email);
            var result = await _userManager.ChangeEmailAsync(user, dto.Email, token);
            if (!result.Succeeded) throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            
            user.UserName = dto.Email; // Keep UserName in sync with Email
            await _userManager.UpdateAsync(user);
        }

        if (!string.IsNullOrEmpty(dto.PhoneNumber) && dto.PhoneNumber != user.PhoneNumber)
        {
            var result = await _userManager.SetPhoneNumberAsync(user, dto.PhoneNumber);
            if (!result.Succeeded) throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        var roles = await _userManager.GetRolesAsync(user);
        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email!,
            Roles = roles
        };
    }

    public async Task DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) throw new KeyNotFoundException($"User with ID {id} not found.");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded) throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}
