using src.RiwiLens.Application.DTOs.Clan;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Application.Interfaces.Services;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Application.Services;

public class ClanService : IClanService
{
    private readonly IGenericRepository<Clan> _clanRepository;
    private readonly IGenericRepository<ClanCoder> _clanCoderRepository;
    private readonly IGenericRepository<ClanTeamLeader> _clanTeamLeaderRepository;
    private readonly IGenericRepository<TeamLeader> _tlRepository;

    public ClanService(
        IGenericRepository<Clan> clanRepository,
        IGenericRepository<ClanCoder> clanCoderRepository,
        IGenericRepository<ClanTeamLeader> clanTeamLeaderRepository,
        IGenericRepository<TeamLeader> tlRepository)
    {
        _clanRepository = clanRepository;
        _clanCoderRepository = clanCoderRepository;
        _clanTeamLeaderRepository = clanTeamLeaderRepository;
        _tlRepository = tlRepository;
    }

    public async Task<ClanResponseDto> CreateAsync(CreateClanDto dto)
    {
        var clan = Clan.Create(dto.Name, dto.Description);
        await _clanRepository.AddAsync(clan);
        await _clanRepository.SaveChangesAsync();
        return await MapToDto(clan);
    }

    public async Task<IEnumerable<ClanResponseDto>> GetAllAsync()
    {
        var clans = await _clanRepository.GetAllAsync();
        var dtos = new List<ClanResponseDto>();
        foreach (var clan in clans)
        {
            dtos.Add(await MapToDto(clan));
        }
        return dtos;
    }

    public async Task<ClanResponseDto?> GetByIdAsync(int id)
    {
        var clan = await _clanRepository.GetByIdAsync(id);
        if (clan == null) return null;
        return await MapToDto(clan);
    }

    public async Task<ClanResponseDto> UpdateAsync(int id, UpdateClanDto dto)
    {
        var clan = await _clanRepository.GetByIdAsync(id);
        if (clan == null) throw new KeyNotFoundException($"Clan {id} not found");

        clan.UpdateInfo(dto.Name, dto.Description);
        _clanRepository.Update(clan);
        await _clanRepository.SaveChangesAsync();
        return await MapToDto(clan);
    }

    public async Task DeleteAsync(int id)
    {
        var clan = await _clanRepository.GetByIdAsync(id);
        if (clan == null) throw new KeyNotFoundException($"Clan {id} not found");
        
        // Note: Soft delete or hard delete? Usually hard delete for simple CRUD unless specified.
        // But Clan might have relationships. EF Core Cascade Delete might handle it if configured.
        // Or we should check. For now, simple remove.
        _clanRepository.Remove(clan);
        await _clanRepository.SaveChangesAsync();
    }

    public async Task AssignCoderAsync(int clanId, AssignCoderDto dto)
    {
        // Check if already assigned and active
        var existing = (await _clanCoderRepository.FindAsync(cc => cc.ClanId == clanId && cc.CoderId == dto.CoderId && cc.IsActive)).FirstOrDefault();
        if (existing != null) return; // Already assigned

        var assignment = ClanCoder.Create(clanId, dto.CoderId);
        await _clanCoderRepository.AddAsync(assignment);
        await _clanCoderRepository.SaveChangesAsync();
    }

    public async Task RemoveCoderAsync(int clanId, int coderId)
    {
        var assignment = (await _clanCoderRepository.FindAsync(cc => cc.ClanId == clanId && cc.CoderId == coderId && cc.IsActive)).FirstOrDefault();
        if (assignment == null) throw new KeyNotFoundException("Assignment not found");

        assignment.Deactivate();
        _clanCoderRepository.Update(assignment);
        await _clanCoderRepository.SaveChangesAsync();
    }

    public async Task AssignTeamLeaderAsync(int clanId, AssignTeamLeaderDto dto)
    {
        var existing = (await _clanTeamLeaderRepository.FindAsync(ctl => ctl.ClanId == clanId && ctl.TeamLeaderId == dto.TeamLeaderId && ctl.EndDate == null)).FirstOrDefault();
        if (existing != null) return; // Already assigned

        var assignment = ClanTeamLeader.Create(clanId, dto.TeamLeaderId, dto.RoleId);
        await _clanTeamLeaderRepository.AddAsync(assignment);
        await _clanTeamLeaderRepository.SaveChangesAsync();
    }

    public async Task RemoveTeamLeaderAsync(int clanId, int teamLeaderId)
    {
        var assignment = (await _clanTeamLeaderRepository.FindAsync(ctl => ctl.ClanId == clanId && ctl.TeamLeaderId == teamLeaderId && ctl.EndDate == null)).FirstOrDefault();
        if (assignment == null) throw new KeyNotFoundException("Assignment not found");

        assignment.EndAssignment();
        _clanTeamLeaderRepository.Update(assignment);
        await _clanTeamLeaderRepository.SaveChangesAsync();
    }

    private async Task<ClanResponseDto> MapToDto(Clan clan)
    {
        // Need to fetch related data to count active coders and list TLs
        // Since GenericRepository returns IEnumerable, we might need to query specifically if performance is an issue.
        // But for now, we use FindAsync.
        
        var activeCoders = await _clanCoderRepository.FindAsync(cc => cc.ClanId == clan.Id && cc.IsActive);
        var activeTLs = await _clanTeamLeaderRepository.FindAsync(ctl => ctl.ClanId == clan.Id && ctl.EndDate == null);
        
        var tlNames = new List<string>();
        foreach (var ctl in activeTLs)
        {
            var tl = await _tlRepository.GetByIdAsync(ctl.TeamLeaderId);
            if (tl != null) tlNames.Add(tl.FullName);
        }

        return new ClanResponseDto
        {
            Id = clan.Id,
            Name = clan.Name,
            Description = clan.Description,
            ActiveCodersCount = activeCoders.Count(),
            TeamLeaders = tlNames
        };
    }
}
