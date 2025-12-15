using src.RiwiLens.Application.DTOs.User;

namespace src.RiwiLens.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserResponseDto> CreateUserAsync(CreateUserDto dto);
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
    Task<UserResponseDto?> GetByIdAsync(string id);
}
