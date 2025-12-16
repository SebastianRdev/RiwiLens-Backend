using src.RiwiLens.Application.DTOs.Catalog;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Application.Interfaces.Services;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Application.Services;

public class CatalogService : ICatalogService
{
    private readonly IGenericRepository<TechnicalSkill> _techSkillRepo;
    private readonly IGenericRepository<SoftSkill> _softSkillRepo;
    private readonly IGenericRepository<ClassType> _classTypeRepo;
    private readonly IGenericRepository<Day> _dayRepo;
    private readonly IGenericRepository<Specialty> _specialtyRepo;
    private readonly IGenericRepository<StatusCoder> _statusRepo;

    public CatalogService(
        IGenericRepository<TechnicalSkill> techSkillRepo,
        IGenericRepository<SoftSkill> softSkillRepo,
        IGenericRepository<ClassType> classTypeRepo,
        IGenericRepository<Day> dayRepo,
        IGenericRepository<Specialty> specialtyRepo,
        IGenericRepository<StatusCoder> statusRepo)
    {
        _techSkillRepo = techSkillRepo;
        _softSkillRepo = softSkillRepo;
        _classTypeRepo = classTypeRepo;
        _dayRepo = dayRepo;
        _specialtyRepo = specialtyRepo;
        _statusRepo = statusRepo;
    }

    public async Task<IEnumerable<CatalogItemDto>> GetTechnicalSkillsAsync()
    {
        var items = await _techSkillRepo.GetAllAsync();
        return items.Select(x => new CatalogItemDto { Id = x.Id, Name = x.Name });
    }

    public async Task<IEnumerable<CatalogItemDto>> GetSoftSkillsAsync()
    {
        var items = await _softSkillRepo.GetAllAsync();
        return items.Select(x => new CatalogItemDto { Id = x.Id, Name = x.Name });
    }

    public async Task<IEnumerable<CatalogItemDto>> GetClassTypesAsync()
    {
        var items = await _classTypeRepo.GetAllAsync();
        return items.Select(x => new CatalogItemDto { Id = x.Id, Name = x.Name });
    }

    public async Task<IEnumerable<CatalogItemDto>> GetDaysAsync()
    {
        var items = await _dayRepo.GetAllAsync();
        return items.Select(x => new CatalogItemDto { Id = x.Id, Name = x.Name });
    }



    public async Task<IEnumerable<CatalogItemDto>> GetSpecialtiesAsync()
    {
        var items = await _specialtyRepo.GetAllAsync();
        return items.Select(x => new CatalogItemDto { Id = x.Id, Name = x.Name });
    }

    public async Task<IEnumerable<CatalogItemDto>> GetStatusCodersAsync()
    {
        var items = await _statusRepo.GetAllAsync();
        return items.Select(x => new CatalogItemDto { Id = x.Id, Name = x.Name });
    }
}
