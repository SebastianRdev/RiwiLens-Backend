using src.RiwiLens.Application.DTOs.Clan;

namespace src.RiwiLens.Application.Interfaces.Services;

public interface IClanService
{
    Task<ClanResponseDto> CreateAsync(CreateClanDto dto);
    Task<IEnumerable<ClanResponseDto>> GetAllAsync();
    Task<ClanResponseDto?> GetByIdAsync(int id);
    Task<ClanResponseDto> UpdateAsync(int id, UpdateClanDto dto);
    Task DeleteAsync(int id);
    
    Task AssignCoderAsync(int clanId, AssignCoderDto dto);
    Task RemoveCoderAsync(int clanId, int coderId);
    
    Task AssignTeamLeaderAsync(int clanId, AssignTeamLeaderDto dto);
    Task RemoveTeamLeaderAsync(int clanId, int teamLeaderId);
}
