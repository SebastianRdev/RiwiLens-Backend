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
    private readonly IGenericRepository<Attendance> _attendanceRepository;
    private readonly IGenericRepository<Feedback> _feedbackRepository;

    public DashboardService(
        IGenericRepository<Coder> coderRepository,
        IGenericRepository<TeamLeader> tlRepository,
        IGenericRepository<Clan> clanRepository,
        IGenericRepository<CoderTechnicalSkill> coderSkillRepository,
        IGenericRepository<TechnicalSkill> skillRepository,
        UserManager<ApplicationUser> userManager,
        IGenericRepository<ClanCoder> clanCoderRepository,
        IGenericRepository<ClanTeamLeader> clanTlRepository,
        IGenericRepository<StatusCoder> statusCoderRepository,
        IGenericRepository<Attendance> attendanceRepository,
        IGenericRepository<Feedback> feedbackRepository)
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
        _attendanceRepository = attendanceRepository;
        _feedbackRepository = feedbackRepository;
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
        
        // Coder -> Clan (Active only)
        var coderClanDict = clanCoders
            .Where(cc => cc.IsActive)
            .GroupBy(cc => cc.CoderId)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(cc => cc.StartDate).First().ClanId); 

        // TL -> Clan (Active only, with fallback to latest)
        var tlClanDict = clanTls
            .GroupBy(ct => ct.TeamLeaderId)
            .ToDictionary(g => g.Key, g => 
            {
                var active = g.FirstOrDefault(ct => ct.EndDate == null);
                return active != null ? active.ClanId : g.OrderByDescending(ct => ct.StartDate).First().ClanId;
            });

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
    public async Task<CoderDashboardDto> GetCoderDashboardAsync(int coderId)
    {
        var coder = await _coderRepository.GetByIdAsync(coderId);
        if (coder == null) throw new KeyNotFoundException($"Coder with ID {coderId} not found.");

        // Clan
        var clanCoders = await _clanCoderRepository.FindAsync(cc => cc.CoderId == coderId);
        var activeClanName = "No Clan";
        if (clanCoders.Any())
        {
            var lastClanId = clanCoders.OrderByDescending(cc => cc.Id).First().ClanId; // Assuming higher ID is newer
            var clan = await _clanRepository.GetByIdAsync(lastClanId);
            if (clan != null) activeClanName = clan.Name;
        }

        // Attendance
        var attendances = (await _attendanceRepository.FindAsync(a => a.CoderId == coderId)).ToList();
        var totalClasses = attendances.Count; // Ideally this should be total classes scheduled, but using attendance count for now or assuming 1 record per class
        // If we want "8 of 9", we need total classes. For now, let's assume totalClasses is count of records (present/absent/justified).
        // If there are no records for a class, we don't know about it unless we query Class repository.
        // Given the prompt "8 de 9 asistencias", it implies 9 classes happened.
        // Let's assume 'attendances' contains all records including absent.
        
        var presentCount = attendances.Count(a => a.Status == Domain.Enums.AttendanceStatus.Present);
        var percentage = totalClasses > 0 ? (int)((double)presentCount / totalClasses * 100) : 0;
        var detail = $"{presentCount} de {totalClasses} asistencias";

        var recentAttendance = attendances
            .OrderByDescending(a => a.TimestampIn)
            .Take(5)
            .Select(a => new RecentAttendanceDto
            {
                Date = a.TimestampIn.ToString("yyyy-MM-dd"),
                Day = a.TimestampIn.ToString("ddd", new System.Globalization.CultureInfo("es-ES")), // Spanish day
                Status = a.Status.ToString().ToLower()
            })
            .ToList();

        // Feedback
        var feedbacks = await _feedbackRepository.FindAsync(f => f.CoderId == coderId);
        var latestFeedback = feedbacks.OrderByDescending(f => f.CreatedAt).FirstOrDefault();
        LatestFeedbackDto? latestFeedbackDto = null;

        if (latestFeedback != null)
        {
            var tl = await _tlRepository.GetByIdAsync(latestFeedback.TeamLeaderId);
            latestFeedbackDto = new LatestFeedbackDto
            {
                Author = new FeedbackAuthorDto
                {
                    Name = tl?.FullName ?? "Unknown",
                    Role = "TeamLeader"
                },
                Date = latestFeedback.CreatedAt.ToString("yyyy-MM-dd"),
                Message = latestFeedback.Message
            };
        }

        return new CoderDashboardDto
        {
            User = new DashboardUserDto
            {
                Id = coder.Id,
                FullName = coder.FullName
            },
            Attendance = new DashboardAttendanceSummaryDto
            {
                Percentage = percentage,
                Detail = detail
            },
            Clan = new DashboardClanDto
            {
                Name = activeClanName
            },
            RecentAttendance = recentAttendance,
            LatestFeedback = latestFeedbackDto
        };
    }

    public async Task<TeamLeaderDashboardDto> GetTeamLeaderDashboardAsync(int tlId)
    {
        var tl = await _tlRepository.GetByIdAsync(tlId);
        if (tl == null) throw new KeyNotFoundException($"TeamLeader with ID {tlId} not found.");

        // Get Clans assigned to TL
        var clanTls = await _clanTlRepository.FindAsync(ct => ct.TeamLeaderId == tlId);
        var clanIds = clanTls.Select(ct => ct.ClanId).Distinct().ToList();

        if (!clanIds.Any())
        {
            return new TeamLeaderDashboardDto
            {
                TotalCoders = 0,
                AverageAttendance = 0,
                Coders = new List<CoderSummaryDto>()
            };
        }

        // Get Coders in those Clans
        var allClanCoders = await _clanCoderRepository.GetAllAsync();
        var relevantClanCoders = allClanCoders.Where(cc => clanIds.Contains(cc.ClanId)).ToList();
        
        var coderIdsInClans = relevantClanCoders.Select(cc => cc.CoderId).Distinct().ToList();

        var coders = new List<CoderSummaryDto>();
        double totalAttendancePercentage = 0;
        int codersWithAttendance = 0;

        foreach (var coderId in coderIdsInClans)
        {
            var coder = await _coderRepository.GetByIdAsync(coderId);
            if (coder == null) continue;

            // Get User Email (need to fetch User)
            var user = await _userManager.FindByIdAsync(coder.UserId);
            var email = user?.Email ?? "Unknown";

            // Get Status Name
            var status = "Unknown";
            var statusObj = await _statusCoderRepository.GetByIdAsync(coder.StatusId);
            if (statusObj != null) status = statusObj.Name;

            // Calculate Attendance for this coder
            var attendances = (await _attendanceRepository.FindAsync(a => a.CoderId == coderId)).ToList();
            var totalClasses = attendances.Count;
            var presentCount = attendances.Count(a => a.Status == Domain.Enums.AttendanceStatus.Present);
            var percentage = totalClasses > 0 ? (double)presentCount / totalClasses * 100 : 0;

            if (totalClasses > 0)
            {
                totalAttendancePercentage += percentage;
                codersWithAttendance++;
            }

            coders.Add(new CoderSummaryDto
            {
                Id = coder.Id,
                FullName = coder.FullName,
                Email = email,
                Status = status,
                ProfileImageUrl = "" // Placeholder
            });
        }

        var avgAttendance = codersWithAttendance > 0 ? totalAttendancePercentage / codersWithAttendance : 0;

        return new TeamLeaderDashboardDto
        {
            TotalCoders = coders.Count,
            AverageAttendance = Math.Round(avgAttendance, 2),
            Coders = coders
        };
    }
    public async Task<IEnumerable<CoderSummaryDto>> GetCodersByClanAsync(int tlId)
    {
        var tl = await _tlRepository.GetByIdAsync(tlId);
        if (tl == null) throw new KeyNotFoundException($"TeamLeader with ID {tlId} not found.");

        // Get Clans assigned to TL (Active/Latest fallback)
        var clanTls = await _clanTlRepository.FindAsync(ct => ct.TeamLeaderId == tlId);
        var activeClanTls = clanTls.Where(ct => ct.EndDate == null).ToList();
        
        // If no active assignment found, fallback to latest
        if (!activeClanTls.Any() && clanTls.Any())
        {
            activeClanTls.Add(clanTls.OrderByDescending(ct => ct.StartDate).First());
        }

        var clanIds = activeClanTls.Select(ct => ct.ClanId).Distinct().ToList();

        if (!clanIds.Any()) return new List<CoderSummaryDto>();

        // Get Coders in those Clans (Active only)
        var allClanCoders = await _clanCoderRepository.GetAllAsync();
        var relevantClanCoders = allClanCoders
            .Where(cc => clanIds.Contains(cc.ClanId) && cc.IsActive)
            .ToList();
        
        var coderIds = relevantClanCoders.Select(cc => cc.CoderId).Distinct().ToList();

        var coders = new List<CoderSummaryDto>();

        foreach (var coderId in coderIds)
        {
            var coder = await _coderRepository.GetByIdAsync(coderId);
            if (coder == null) continue;

            var user = await _userManager.FindByIdAsync(coder.UserId);
            var email = user?.Email ?? "Unknown";

            var status = "Unknown";
            var statusObj = await _statusCoderRepository.GetByIdAsync(coder.StatusId);
            if (statusObj != null) status = statusObj.Name;

            coders.Add(new CoderSummaryDto
            {
                Id = coder.Id,
                FullName = coder.FullName,
                Email = email,
                Status = status,
                ProfileImageUrl = "" // Placeholder
            });
        }

        return coders;
    }
}
