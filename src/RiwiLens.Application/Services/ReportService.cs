using src.RiwiLens.Application.DTOs.Report;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Application.Interfaces.Services;
using src.RiwiLens.Domain.Entities;
using src.RiwiLens.Domain.Enums;

namespace src.RiwiLens.Application.Services;

public class ReportService : IReportService
{
    private readonly IGenericRepository<Attendance> _attendanceRepository;
    private readonly IGenericRepository<Feedback> _feedbackRepository;
    private readonly IGenericRepository<Coder> _coderRepository;
    private readonly IGenericRepository<ClanCoder> _clanCoderRepository;

    public ReportService(
        IGenericRepository<Attendance> attendanceRepository,
        IGenericRepository<Feedback> feedbackRepository,
        IGenericRepository<Coder> coderRepository,
        IGenericRepository<ClanCoder> clanCoderRepository)
    {
        _attendanceRepository = attendanceRepository;
        _feedbackRepository = feedbackRepository;
        _coderRepository = coderRepository;
        _clanCoderRepository = clanCoderRepository;
    }

    public async Task<IEnumerable<AttendanceReportDto>> GetAttendanceReportAsync(int? clanId)
    {
        var coders = await _coderRepository.GetAllAsync();
        
        if (clanId.HasValue)
        {
            var clanCoders = await _clanCoderRepository.FindAsync(cc => cc.ClanId == clanId.Value && cc.IsActive);
            var coderIds = clanCoders.Select(cc => cc.CoderId).ToHashSet();
            coders = coders.Where(c => coderIds.Contains(c.Id));
        }

        var attendances = await _attendanceRepository.GetAllAsync();
        // Filter attendances for relevant coders
        var relevantCoderIds = coders.Select(c => c.Id).ToHashSet();
        attendances = attendances.Where(a => relevantCoderIds.Contains(a.CoderId));

        var report = new List<AttendanceReportDto>();

        foreach (var coder in coders)
        {
            var coderAttendances = attendances.Where(a => a.CoderId == coder.Id).ToList();
            var total = coderAttendances.Count;
            var present = coderAttendances.Count(a => a.Status == AttendanceStatus.Present);
            var absent = coderAttendances.Count(a => a.Status == AttendanceStatus.Absent);
            var justified = coderAttendances.Count(a => a.Status == AttendanceStatus.Justified);

            report.Add(new AttendanceReportDto
            {
                CoderId = coder.Id,
                CoderName = coder.FullName,
                TotalClasses = total,
                PresentCount = present,
                AbsentCount = absent,
                JustifiedCount = justified,
                AttendancePercentage = total > 0 ? (double)present / total * 100 : 0
            });
        }

        return report;
    }

    public async Task<IEnumerable<FeedbackReportDto>> GetFeedbackReportAsync(int? clanId)
    {
        var coders = await _coderRepository.GetAllAsync();

        if (clanId.HasValue)
        {
            var clanCoders = await _clanCoderRepository.FindAsync(cc => cc.ClanId == clanId.Value && cc.IsActive);
            var coderIds = clanCoders.Select(cc => cc.CoderId).ToHashSet();
            coders = coders.Where(c => coderIds.Contains(c.Id));
        }

        var feedbacks = await _feedbackRepository.GetAllAsync();
        var relevantCoderIds = coders.Select(c => c.Id).ToHashSet();
        feedbacks = feedbacks.Where(f => relevantCoderIds.Contains(f.CoderId));

        var report = new List<FeedbackReportDto>();

        foreach (var coder in coders)
        {
            var coderFeedbacks = feedbacks.Where(f => f.CoderId == coder.Id).ToList();
            
            report.Add(new FeedbackReportDto
            {
                CoderId = coder.Id,
                CoderName = coder.FullName,
                TotalFeedbacks = coderFeedbacks.Count,
                LastFeedbackDate = coderFeedbacks.OrderByDescending(f => f.CreatedAt).FirstOrDefault()?.CreatedAt
            });
        }

        return report;
    }
}
