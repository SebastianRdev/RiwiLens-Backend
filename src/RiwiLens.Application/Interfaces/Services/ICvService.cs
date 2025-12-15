using src.RiwiLens.Application.DTOs.Cv;

namespace src.RiwiLens.Application.Interfaces.Services;

public interface ICvService
{
    Task<CvResponseDto> GenerateCvAsync(int coderId);
}
