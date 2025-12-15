using src.RiwiLens.Application.DTOs.Attendance;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Application.Interfaces.Services;
using src.RiwiLens.Domain.Entities;
using src.RiwiLens.Domain.Enums;

namespace src.RiwiLens.Application.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IGenericRepository<Attendance> _attendanceRepository;
    private readonly IGenericRepository<Coder> _coderRepository;
    private readonly IGenericRepository<Class> _classRepository;
    private readonly IGenericRepository<Clan> _clanRepository;

    public AttendanceService(
        IGenericRepository<Attendance> attendanceRepository,
        IGenericRepository<Coder> coderRepository,
        IGenericRepository<Class> classRepository,
        IGenericRepository<Clan> clanRepository)
    {
        _attendanceRepository = attendanceRepository;
        _coderRepository = coderRepository;
        _classRepository = classRepository;
        _clanRepository = clanRepository;
    }

    public async Task<AttendanceResponseDto> RegisterAsync(RegisterAttendanceDto dto)
    {
        var coder = await _coderRepository.GetByIdAsync(dto.CoderId);
        if (coder == null) throw new KeyNotFoundException("Coder not found");

        var classEntity = await _classRepository.GetByIdAsync(dto.ClassId);
        if (classEntity == null) throw new KeyNotFoundException("Class not found");

        // Check if attendance already exists
        var existing = await _attendanceRepository.FindAsync(a => a.CoderId == dto.CoderId && a.ClassId == dto.ClassId);
        if (existing.Any()) throw new InvalidOperationException("Attendance already registered for this class.");

        var attendance = Attendance.Create(
            dto.ClanId,
            dto.ClassId,
            dto.CoderId,
            DateTime.UtcNow,
            "SYSTEM", // Or from DTO if manual
            dto.EvidenceUrl ?? ""
        );

        if (dto.Status == AttendanceStatus.Justified)
        {
            attendance.MarkAsJustified(dto.Remarks ?? "No justification provided");
        }
        else if (dto.Status == AttendanceStatus.Absent)
        {
            attendance.MarkAsAbsent();
        }
        // Default is Present

        await _attendanceRepository.AddAsync(attendance);
        await _attendanceRepository.SaveChangesAsync();

        return await MapToDto(attendance);
    }

    public async Task<AttendanceResponseDto> UpdateAsync(int id, UpdateAttendanceDto dto)
    {
        var attendance = await _attendanceRepository.GetByIdAsync(id);
        if (attendance == null) throw new KeyNotFoundException($"Attendance {id} not found");

        attendance.Update(dto.Status);
        
        _attendanceRepository.Update(attendance);
        await _attendanceRepository.SaveChangesAsync();

        return await MapToDto(attendance);
    }

    public async Task<IEnumerable<AttendanceResponseDto>> GetByCoderIdAsync(int coderId)
    {
        var attendances = await _attendanceRepository.FindAsync(a => a.CoderId == coderId);
        var dtos = new List<AttendanceResponseDto>();
        foreach (var a in attendances)
        {
            dtos.Add(await MapToDto(a));
        }
        return dtos;
    }

    public async Task<IEnumerable<AttendanceResponseDto>> GetByClassIdAsync(int classId)
    {
        var attendances = await _attendanceRepository.FindAsync(a => a.ClassId == classId);
        var dtos = new List<AttendanceResponseDto>();
        foreach (var a in attendances)
        {
            dtos.Add(await MapToDto(a));
        }
        return dtos;
    }

    public async Task<IEnumerable<AttendanceResponseDto>> GetByClanIdAndDateAsync(int clanId, DateTime date)
    {
        // This is tricky because Attendance stores TimestampIn, but we want by Class Date usually.
        // Or we filter by ClanId and TimestampIn date part.
        // Let's assume we filter by the Class date if possible, or just the attendance timestamp.
        // Given the requirement, it's likely "Show me attendance for Clan X on Day Y".
        
        // Strategy: Find classes for that clan on that date, then find attendances for those classes.
        // Or directly query attendances if they have ClanId (which they do).
        
        var attendances = await _attendanceRepository.FindAsync(a => a.ClanId == clanId && a.TimestampIn.Date == date.Date);
        var dtos = new List<AttendanceResponseDto>();
        foreach (var a in attendances)
        {
            dtos.Add(await MapToDto(a));
        }
        return dtos;
    }

    private async Task<AttendanceResponseDto> MapToDto(Attendance a)
    {
        var coder = await _coderRepository.GetByIdAsync(a.CoderId);
        var classEntity = await _classRepository.GetByIdAsync(a.ClassId);
        // We might need to fetch User to get Coder Name if it's not in Coder entity directly (Coder links to User)
        // Coder entity doesn't have Name, it has UserId. We need to fetch User? 
        // Or maybe Coder has navigation property to User.
        // Let's assume for now we return empty name or need to inject UserManager/UserService.
        // To keep it simple and avoid circular deps or complex queries in loop, I'll leave CoderName empty or minimal.
        // Ideally we should use a View or a more complex Repository method with Include.
        
        return new AttendanceResponseDto
        {
            Id = a.Id,
            CoderId = a.CoderId,
            CoderName = "", // TODO: Fetch name from User
            ClassId = a.ClassId,
            ClassName = classEntity?.Date.ToString("dd/MM/yyyy") ?? "",
            Date = a.TimestampIn,
            Status = a.Status,
            StatusName = a.Status.ToString(),
            EvidenceUrl = a.ImageUrl
        };
    }
}
