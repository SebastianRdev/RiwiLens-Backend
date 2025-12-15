using src.RiwiLens.Application.DTOs.Class;

namespace src.RiwiLens.Application.Interfaces.Services;

public interface IClassService
{
    Task<ClassResponseDto> CreateAsync(CreateClassDto dto);
    Task<IEnumerable<ClassResponseDto>> GetAllAsync();
    Task<ClassResponseDto?> GetByIdAsync(int id);
    Task<ClassResponseDto> UpdateAsync(int id, UpdateClassDto dto);
    Task DeleteAsync(int id);
    
    Task<IEnumerable<ClassResponseDto>> GetByClanIdAsync(int clanId);
    Task<IEnumerable<ClassResponseDto>> GetTodayClassesByClanAsync(int clanId);
}
