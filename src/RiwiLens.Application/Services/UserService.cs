using Microsoft.AspNetCore.Identity;
using src.RiwiLens.Application.DTOs.User;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Application.Interfaces.Services;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IGenericRepository<Coder> _coderRepository;
    private readonly IGenericRepository<TeamLeader> _teamLeaderRepository;
    // Assuming we might need ProfessionalProfile repo if Coder creation requires it, 
    // but Coder.Create takes an ID. We might need to create a default profile first.
    // For now, I'll assume 0 or handle it. Coder.Create takes int professionalProfileId.
    // I should probably create a ProfessionalProfile first.
    private readonly IGenericRepository<ProfessionalProfile> _profileRepository;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IGenericRepository<Coder> coderRepository,
        IGenericRepository<TeamLeader> teamLeaderRepository,
        IGenericRepository<ProfessionalProfile> profileRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _coderRepository = coderRepository;
        _teamLeaderRepository = teamLeaderRepository;
        _profileRepository = profileRepository;
    }

    public async Task<UserResponseDto> CreateUserAsync(CreateUserDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        if (!string.IsNullOrEmpty(dto.Role))
        {
            if (await _roleManager.RoleExistsAsync(dto.Role))
            {
                await _userManager.AddToRoleAsync(user, dto.Role);
            }
        }

        // Create Profile based on Role
        if (dto.Role == "Coder")
        {
            // Create default ProfessionalProfile
            var newProfile = ProfessionalProfile.Create(string.Empty); 
            await _profileRepository.AddAsync(newProfile);
            await _profileRepository.SaveChangesAsync();

            var coder = Coder.Create(
                dto.FullName,
                user.Id,
                dto.DocumentType,
                dto.Identification,
                dto.BirthDate,
                dto.Address ?? "",
                dto.Country ?? "",
                dto.City ?? "",
                dto.Gender,
                newProfile.Id,
                1 // StatusId 1 = Active (Assumption)
            );
            await _coderRepository.AddAsync(coder);
            await _coderRepository.SaveChangesAsync();
        }
        else if (dto.Role == "TeamLeader")
        {
            var tl = TeamLeader.Create(
                user.Id,
                dto.FullName,
                dto.Gender,
                dto.BirthDate
            );
            await _teamLeaderRepository.AddAsync(tl);
            await _teamLeaderRepository.SaveChangesAsync();
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
