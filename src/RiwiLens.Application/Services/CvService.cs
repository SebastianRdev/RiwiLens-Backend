using System.Text;
using src.RiwiLens.Application.DTOs.Cv;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Application.Interfaces.Services;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Application.Services;

public class CvService : ICvService
{
    private readonly IGenericRepository<Coder> _coderRepo;
    private readonly IGenericRepository<CoderTechnicalSkill> _coderTechSkillRepo;
    private readonly IGenericRepository<TechnicalSkill> _techSkillRepo;
    private readonly IGenericRepository<CoderSoftSkill> _coderSoftSkillRepo;
    private readonly IGenericRepository<SoftSkill> _softSkillRepo;
    private readonly IGenericRepository<ProfessionalProfile> _profileRepo;
    private readonly IGenericRepository<ClanCoder> _clanCoderRepo;
    private readonly IGenericRepository<Clan> _clanRepo;
    private readonly IAiService _aiService;

    public CvService(
        IGenericRepository<Coder> coderRepo,
        IGenericRepository<CoderTechnicalSkill> coderTechSkillRepo,
        IGenericRepository<TechnicalSkill> techSkillRepo,
        IGenericRepository<CoderSoftSkill> coderSoftSkillRepo,
        IGenericRepository<SoftSkill> softSkillRepo,
        IGenericRepository<ProfessionalProfile> profileRepo,
        IGenericRepository<ClanCoder> clanCoderRepo,
        IGenericRepository<Clan> clanRepo,
        IAiService aiService)
    {
        _coderRepo = coderRepo;
        _coderTechSkillRepo = coderTechSkillRepo;
        _techSkillRepo = techSkillRepo;
        _coderSoftSkillRepo = coderSoftSkillRepo;
        _softSkillRepo = softSkillRepo;
        _profileRepo = profileRepo;
        _clanCoderRepo = clanCoderRepo;
        _clanRepo = clanRepo;
        _aiService = aiService;
    }

    public async Task<CvResponseDto> GenerateCvAsync(int coderId)
    {
        var coder = await _coderRepo.GetByIdAsync(coderId);
        if (coder == null) throw new KeyNotFoundException($"Coder {coderId} not found");

        // Fetch related data
        var profile = await _profileRepo.GetByIdAsync(coder.ProfessionalProfileId);
        
        var coderTechSkills = await _coderTechSkillRepo.FindAsync(x => x.CoderId == coderId);
        var techSkills = new List<string>();
        foreach(var cts in coderTechSkills)
        {
            var skill = await _techSkillRepo.GetByIdAsync(cts.SkillId);
            if(skill != null) techSkills.Add($"{skill.Name} ({cts.Level})");
        }

        var coderSoftSkills = await _coderSoftSkillRepo.FindAsync(x => x.CoderId == coderId);
        var softSkills = new List<string>();
        foreach(var css in coderSoftSkills)
        {
            var skill = await _softSkillRepo.GetByIdAsync(css.SoftSkillId);
            if(skill != null) softSkills.Add(skill.Name);
        }

        var clanCoders = await _clanCoderRepo.FindAsync(x => x.CoderId == coderId);
        var clans = new List<string>();
        foreach(var cc in clanCoders)
        {
            var clan = await _clanRepo.GetByIdAsync(cc.ClanId);
            if(clan != null) clans.Add(clan.Name);
        }

        // Generate AI Content
        var cvContent = await _aiService.GenerateCvContentAsync(coder, profile, techSkills, softSkills);

        return new CvResponseDto
        {
            CoderId = coder.Id,
            FullName = coder.FullName,
            CvContent = cvContent,
            GeneratedAt = DateTime.UtcNow
        };
    }
}
