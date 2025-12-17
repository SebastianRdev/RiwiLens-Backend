using src.RiwiLens.Application.DTOs.Coder;

namespace src.RiwiLens.Application.Interfaces.Services;

public interface ICoderService
{
    Task<CoderDetailResponseDto?> GetByIdAsync(int id);
    Task<CoderDetailResponseDto?> GetByIdentificationAsync(string identification);
    Task<IEnumerable<CoderResponseDto>> GetAllAsync();
    Task<CoderResponseDto> UpdateAsync(int id, UpdateCoderDto dto);
}
