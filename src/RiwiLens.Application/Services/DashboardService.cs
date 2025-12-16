using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using src.RiwiLens.Application.DTOs.Dashboard;
using System.Linq;
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

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IGenericRepository<ClanCoder> _clanCoderRepository;
    private readonly IGenericRepository<ClanTeamLeader> _clanTlRepository;
    private readonly IGenericRepository<StatusCoder> _statusCoderRepository;

    public DashboardService(
        IGenericRepository<Coder> coderRepository,
        IGenericRepository<TeamLeader> tlRepository,
        IGenericRepository<Clan> clanRepository,
        IGenericRepository<CoderTechnicalSkill> coderSkillRepository,
        IGenericRepository<TechnicalSkill> skillRepository,
        UserManager<ApplicationUser> userManager,
        IGenericRepository<ClanCoder> clanCoderRepository,
        IGenericRepository<ClanTeamLeader> clanTlRepository,
        IGenericRepository<StatusCoder> statusCoderRepository)
    {
        _coderRepository = coderRepository;
        _tlRepository = tlRepository;
        _clanRepository = clanRepository;
        _coderSkillRepository = coderSkillRepository;
        _skillRepository = skillRepository;
        _userManager = userManager;
        _clanCoderRepository = clanCoderRepository;
        _clanTlRepository = clanTlRepository;
        _statusCoderRepository = statusCoderRepository;
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
    public async Task<UserManagementStatsDto> GetUserManagementStatsAsync()
    {
        var totalUsers = await _userManager.Users.CountAsync();
        var totalCoders = await _coderRepository.GetAllAsync();
        var totalTls = await _tlRepository.GetAllAsync();
        
        // Count admins (users with role 'Admin')
        var admins = await _userManager.GetUsersInRoleAsync("Admin");
        var totalAdmins = admins.Count;

        return new UserManagementStatsDto
        {
            TotalUsers = totalUsers,
            TotalCoders = totalCoders.Count(),
            TotalTeamLeaders = totalTls.Count(),
            TotalAdmins = totalAdmins
        };
    }

    public async Task<IEnumerable<UserResponseDto>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var userDtos = new List<UserResponseDto>();

        // Pre-fetch data to avoid N+1
        var coders = await _coderRepository.GetAllAsync();
        var tls = await _tlRepository.GetAllAsync();
        var clans = await _clanRepository.GetAllAsync();
        var clanCoders = await _clanCoderRepository.GetAllAsync();
        var clanTls = await _clanTlRepository.GetAllAsync();
        var statuses = await _statusCoderRepository.GetAllAsync();

        // Dictionaries for fast lookup
        var coderDict = coders.ToDictionary(c => c.UserId);
        var tlDict = tls.ToDictionary(t => t.UserId);
        var clanDict = clans.ToDictionary(c => c.Id, c => c.Name);
        var statusDict = statuses.ToDictionary(s => s.Id, s => s.Name);
        
        // Coder -> Clan (Assuming 1 active clan or taking last)
        var coderClanDict = clanCoders
            .GroupBy(cc => cc.CoderId)
            .ToDictionary(g => g.Key, g => g.Last().ClanId); 

        // TL -> Clan
        var tlClanDict = clanTls
            .GroupBy(ct => ct.TeamLeaderId)
            .ToDictionary(g => g.Key, g => g.Last().ClanId);

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";
            var clanName = "N/A";
            var status = "Active"; // Default

            if (role == "Coder" && coderDict.TryGetValue(user.Id, out var coder))
            {
                if (coderClanDict.TryGetValue(coder.Id, out var clanId) && clanDict.TryGetValue(clanId, out var cName))
                {
                    clanName = cName;
                }
                
                if (statusDict.TryGetValue(coder.StatusId, out var sName))
                {
                    status = sName;
                }
            }
            else if (role == "TeamLeader" && tlDict.TryGetValue(user.Id, out var tl))
            {
                if (tlClanDict.TryGetValue(tl.Id, out var clanId) && clanDict.TryGetValue(clanId, out var cName))
                {
                    clanName = cName;
                }
            }

            userDtos.Add(new UserResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName ?? "Unknown",
                Email = user.Email ?? "Unknown",
                Role = role,
                Clan = clanName,
                Status = status
            });
        }

        return userDtos;
    }
}
