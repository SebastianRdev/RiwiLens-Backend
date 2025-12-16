using src.RiwiLens.Application.DTOs.Catalog;

namespace src.RiwiLens.Application.Interfaces.Services;

public interface ICatalogService
{
    Task<IEnumerable<CatalogItemDto>> GetTechnicalSkillsAsync();
    Task<IEnumerable<CatalogItemDto>> GetSoftSkillsAsync();
    Task<IEnumerable<CatalogItemDto>> GetClassTypesAsync();
    Task<IEnumerable<CatalogItemDto>> GetDaysAsync();

    Task<IEnumerable<CatalogItemDto>> GetSpecialtiesAsync();
    Task<IEnumerable<CatalogItemDto>> GetStatusCodersAsync();
}
