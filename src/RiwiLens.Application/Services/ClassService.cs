using src.RiwiLens.Application.DTOs.Class;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Application.Interfaces.Services;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Application.Services;

public class ClassService : IClassService
{
    private readonly IGenericRepository<Class> _classRepository;
    private readonly IGenericRepository<Day> _dayRepository;
    private readonly IGenericRepository<ClassType> _classTypeRepository;
    private readonly IGenericRepository<Clan> _clanRepository;
    private readonly IGenericRepository<TeamLeader> _tlRepository;

    public ClassService(
        IGenericRepository<Class> classRepository,
        IGenericRepository<Day> dayRepository,
        IGenericRepository<ClassType> classTypeRepository,
        IGenericRepository<Clan> clanRepository,
        IGenericRepository<TeamLeader> tlRepository)
    {
        _classRepository = classRepository;
        _dayRepository = dayRepository;
        _classTypeRepository = classTypeRepository;
        _clanRepository = clanRepository;
        _tlRepository = tlRepository;
    }

    public async Task<ClassResponseDto> CreateAsync(CreateClassDto dto)
    {
        var day = await _dayRepository.GetByIdAsync(dto.DayId);
        if (day == null) throw new KeyNotFoundException("Day not found");

        // Use Day's schedule as default
        var newClass = Class.Create(
            dto.Date,
            dto.DayId,
            dto.ClassTypeId,
            dto.TeamLeaderId,
            day.StartTime,
            day.EndTime
        );

        await _classRepository.AddAsync(newClass);
        await _classRepository.SaveChangesAsync();

        return await MapToDto(newClass);
    }

    public async Task<IEnumerable<ClassResponseDto>> GetAllAsync()
    {
        var classes = await _classRepository.GetAllAsync();
        var dtos = new List<ClassResponseDto>();
        foreach (var c in classes)
        {
            dtos.Add(await MapToDto(c));
        }
        return dtos;
    }

    public async Task<ClassResponseDto?> GetByIdAsync(int id)
    {
        var c = await _classRepository.GetByIdAsync(id);
        if (c == null) return null;
        return await MapToDto(c);
    }

    public async Task<ClassResponseDto> UpdateAsync(int id, UpdateClassDto dto)
    {
        var c = await _classRepository.GetByIdAsync(id);
        if (c == null) throw new KeyNotFoundException($"Class {id} not found");

        var day = await _dayRepository.GetByIdAsync(dto.DayId);
        if (day == null) throw new KeyNotFoundException("Day not found");

        c.Update(
            dto.Date,
            dto.DayId,
            dto.ClassTypeId,
            dto.TeamLeaderId,
            day.StartTime,
            day.EndTime
        );

        _classRepository.Update(c);
        await _classRepository.SaveChangesAsync();

        return await MapToDto(c);
    }

    public async Task DeleteAsync(int id)
    {
        var c = await _classRepository.GetByIdAsync(id);
        if (c == null) throw new KeyNotFoundException($"Class {id} not found");
        _classRepository.Remove(c);
        await _classRepository.SaveChangesAsync();
    }

    public Task<IEnumerable<ClassResponseDto>> GetByClanIdAsync(int clanId)
    {
        // Not supported by current schema (Class has no ClanId)
        return Task.FromResult(Enumerable.Empty<ClassResponseDto>());
    }

    public Task<IEnumerable<ClassResponseDto>> GetTodayClassesByClanAsync(int clanId)
    {
        // Not supported by current schema
        return Task.FromResult(Enumerable.Empty<ClassResponseDto>());
    }

    private async Task<ClassResponseDto> MapToDto(Class c)
    {
        var day = await _dayRepository.GetByIdAsync(c.DayId);
        var type = await _classTypeRepository.GetByIdAsync(c.ClassTypeId);
        var tl = await _tlRepository.GetByIdAsync(c.TeamLeaderId);

        return new ClassResponseDto
        {
            Id = c.Id,
            Name = $"{type?.Name ?? "Clase"} - {c.Date:dd/MM/yyyy}",
            Description = "", // Class doesn't have description
            Date = c.Date,
            ClassTypeId = c.ClassTypeId,
            ClassTypeName = type?.Name ?? "",
            DayId = c.DayId,
            DayName = day?.Name ?? "",
            ClanId = 0, // Not available
            ClanName = "",
            TeamLeaderId = c.TeamLeaderId,
            TeamLeaderName = tl?.FullName ?? ""
        };
    }
}
