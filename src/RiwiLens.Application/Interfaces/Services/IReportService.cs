using src.RiwiLens.Application.DTOs.Report;

namespace src.RiwiLens.Application.Interfaces.Services;

public interface IReportService
{
    Task<IEnumerable<AttendanceReportDto>> GetAttendanceReportAsync(int? clanId);
    Task<IEnumerable<FeedbackReportDto>> GetFeedbackReportAsync(int? clanId);
}
