using src.RiwiLens.Application.DTOs.Attendance;
using src.RiwiLens.Domain.Enums;

namespace src.RiwiLens.Application.Interfaces.Services;

public interface IAttendanceService
{
    Task<AttendanceResponseDto> RegisterAsync(RegisterAttendanceDto dto);
    Task<AttendanceResponseDto> UpdateAsync(int id, UpdateAttendanceDto dto);
    Task<IEnumerable<AttendanceResponseDto>> GetByCoderIdAsync(int coderId);
    Task<IEnumerable<AttendanceResponseDto>> GetByClassIdAsync(int classId);
    Task<IEnumerable<AttendanceResponseDto>> GetByClanIdAndDateAsync(int clanId, DateTime date);
}
