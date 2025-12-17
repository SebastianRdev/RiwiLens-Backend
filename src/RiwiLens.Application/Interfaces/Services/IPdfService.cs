namespace src.RiwiLens.Application.Interfaces.Services;

public interface IPdfService
{
    Task<byte[]> GenerateCvAsync(int coderId);
}
