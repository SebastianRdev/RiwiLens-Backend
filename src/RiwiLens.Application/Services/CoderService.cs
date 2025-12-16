using Microsoft.AspNetCore.Identity;
using src.RiwiLens.Application.DTOs.Coder;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Application.Interfaces.Services;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Application.Services;

public class CoderService : ICoderService
{
    private readonly IGenericRepository<Coder> _coderRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IGenericRepository<ProfessionalProfile> _profileRepository;
    private readonly IGenericRepository<CoderTechnicalSkill> _coderTechSkillRepository;
    private readonly IGenericRepository<TechnicalSkill> _techSkillRepository;
    private readonly IGenericRepository<CoderSoftSkill> _coderSoftSkillRepository;
    private readonly IGenericRepository<SoftSkill> _softSkillRepository;

    public CoderService(
        IGenericRepository<Coder> coderRepository, 
        UserManager<ApplicationUser> userManager,
        IGenericRepository<ProfessionalProfile> profileRepository,
        IGenericRepository<CoderTechnicalSkill> coderTechSkillRepository,
        IGenericRepository<TechnicalSkill> techSkillRepository,
        IGenericRepository<CoderSoftSkill> coderSoftSkillRepository,
        IGenericRepository<SoftSkill> softSkillRepository)
    {
        _coderRepository = coderRepository;
        _userManager = userManager;
        _profileRepository = profileRepository;
        _coderTechSkillRepository = coderTechSkillRepository;
        _techSkillRepository = techSkillRepository;
        _coderSoftSkillRepository = coderSoftSkillRepository;
        _softSkillRepository = softSkillRepository;
    }

    public async Task<CoderDetailResponseDto?> GetByIdAsync(int id)
    {
        var coder = (await _coderRepository.FindAsync(c => c.Id == id)).FirstOrDefault();
        if (coder == null) return null;

        var baseDto = await MapToDto(coder);
        
        var detailDto = new CoderDetailResponseDto
        {
            Id = baseDto.Id,
            FullName = baseDto.FullName,
            UserId = baseDto.UserId,
            Email = baseDto.Email,
            DocumentType = baseDto.DocumentType,
            Identification = baseDto.Identification,
            Address = baseDto.Address,
            BirthDate = baseDto.BirthDate,
            Country = baseDto.Country,
            City = baseDto.City,
            Gender = baseDto.Gender,
            StatusId = baseDto.StatusId,
            StatusName = baseDto.StatusName,
            ProfessionalProfileId = baseDto.ProfessionalProfileId
        };

        var profile = await _profileRepository.GetByIdAsync(coder.ProfessionalProfileId);
        if (profile != null)
        {
            detailDto.AboutMe = profile.AboutMe;
            detailDto.LinkedIn = profile.LinkedIn;
            detailDto.GitHub = profile.GitHub;
            detailDto.Portfolio = profile.Portfolio;
        }

        var coderTechSkills = await _coderTechSkillRepository.FindAsync(x => x.CoderId == id);
        foreach (var cts in coderTechSkills)
        {
            var skill = await _techSkillRepository.GetByIdAsync(cts.SkillId);
            if (skill != null)
            {
                detailDto.TechnicalSkills.Add(skill.Name);
            }
        }

        var coderSoftSkills = await _coderSoftSkillRepository.FindAsync(x => x.CoderId == id);
        foreach (var css in coderSoftSkills)
        {
            var skill = await _softSkillRepository.GetByIdAsync(css.SoftSkillId);
            if (skill != null)
            {
                detailDto.SoftSkills.Add(skill.Name);
            }
        }

        return detailDto;
    }

    public async Task<IEnumerable<CoderResponseDto>> GetAllAsync()
    {
        var coders = await _coderRepository.GetAllAsync();
        var dtos = new List<CoderResponseDto>();

        foreach (var coder in coders)
        {
            dtos.Add(await MapToDto(coder));
        }

        return dtos;
    }

    public async Task<CoderResponseDto> UpdateAsync(int id, UpdateCoderDto dto)
    {
        var coder = await _coderRepository.GetByIdAsync(id);
        if (coder == null) throw new KeyNotFoundException($"Coder with ID {id} not found.");

        coder.UpdatePersonalInfo(dto.FullName, dto.BirthDate, dto.Gender);
        coder.UpdateLocation(dto.Address, dto.Country, dto.City);

        _coderRepository.Update(coder);
        await _coderRepository.SaveChangesAsync();

        return await MapToDto(coder);
    }

    private async Task<CoderResponseDto> MapToDto(Coder coder)
    {
        var user = await _userManager.FindByIdAsync(coder.UserId);
        
        return new CoderResponseDto
        {
            Id = coder.Id,
            FullName = coder.FullName,
            UserId = coder.UserId,
            Email = user?.Email ?? string.Empty,
            DocumentType = coder.DocumentType,
            Identification = coder.Identification,
            Address = coder.Address,
            BirthDate = coder.BirthDate,
            Country = coder.Country,
            City = coder.City,
            Gender = coder.Gender,
            StatusId = coder.StatusId,
            StatusName = coder.Status?.Name ?? "Unknown", // Assuming StatusCoder has a Name property
            ProfessionalProfileId = coder.ProfessionalProfileId
        };
    }
}
