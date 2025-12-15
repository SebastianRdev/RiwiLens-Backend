using src.RiwiLens.Application.DTOs.Dashboard;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Application.Interfaces.Services;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IGenericRepository<Coder> _coderRepository;
    private readonly IGenericRepository<TeamLeader> _tlRepository;
    private readonly IGenericRepository<Clan> _clanRepository;
    private readonly IGenericRepository<CoderTechnicalSkill> _coderSkillRepository;
    private readonly IGenericRepository<TechnicalSkill> _skillRepository;

    public DashboardService(
        IGenericRepository<Coder> coderRepository,
        IGenericRepository<TeamLeader> tlRepository,
        IGenericRepository<Clan> clanRepository,
        IGenericRepository<CoderTechnicalSkill> coderSkillRepository,
        IGenericRepository<TechnicalSkill> skillRepository)
    {
        _coderRepository = coderRepository;
        _tlRepository = tlRepository;
        _clanRepository = clanRepository;
        _coderSkillRepository = coderSkillRepository;
        _skillRepository = skillRepository;
    }

    public async Task<DashboardStatsDto> GetGlobalStatsAsync()
    {
        // 1. Total Coders
        var coders = await _coderRepository.GetAllAsync();
        var totalCoders = coders.Count();

        // 2. Total TeamLeaders
        var tls = await _tlRepository.GetAllAsync();
        var totalTls = tls.Count();

        // 3. Active Clans (Assuming all are active for now as there is no IsActive flag in Clan entity)
        var clans = await _clanRepository.GetAllAsync();
        var activeClans = clans.Count();

        // 4. Top 5 Technologies
        var coderSkills = await _coderSkillRepository.GetAllAsync();
        
        var topSkills = coderSkills
            .GroupBy(cs => cs.SkillId)
            .Select(g => new { SkillId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        var topTechDtos = new List<TopTechnologyDto>();
        foreach (var item in topSkills)
        {
            var skill = await _skillRepository.GetByIdAsync(item.SkillId);
            if (skill != null)
            {
                topTechDtos.Add(new TopTechnologyDto
                {
                    Name = skill.Name,
                    Count = item.Count
                });
            }
        }

        return new DashboardStatsDto
        {
            TotalCoders = totalCoders,
            TotalTeamLeaders = totalTls,
            ActiveClans = activeClans,
            TopTechnologies = topTechDtos
        };
    }
}
