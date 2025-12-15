using Microsoft.AspNetCore.Identity;
using src.RiwiLens.Application.DTOs.Coder;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Application.Interfaces.Services;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Application.Services;

public class CoderService : ICoderService
{
    private readonly IGenericRepository<Coder> _coderRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public CoderService(IGenericRepository<Coder> coderRepository, UserManager<ApplicationUser> userManager)
    {
        _coderRepository = coderRepository;
        _userManager = userManager;
    }

    public async Task<CoderResponseDto?> GetByIdAsync(int id)
    {
        var coder = (await _coderRepository.FindAsync(c => c.Id == id)).FirstOrDefault();
        if (coder == null) return null;

        return await MapToDto(coder);
    }

    public async Task<IEnumerable<CoderResponseDto>> GetAllAsync()
    {
        var coders = await _coderRepository.GetAllAsync();
        var dtos = new List<CoderResponseDto>();

        foreach (var coder in coders)
        {
            dtos.Add(await MapToDto(coder));
        }

        return dtos;
    }

    public async Task<CoderResponseDto> UpdateAsync(int id, UpdateCoderDto dto)
    {
        var coder = await _coderRepository.GetByIdAsync(id);
        if (coder == null) throw new KeyNotFoundException($"Coder with ID {id} not found.");

        coder.UpdatePersonalInfo(dto.FullName, dto.BirthDate, dto.Gender);
        coder.UpdateLocation(dto.Address, dto.Country, dto.City);

        _coderRepository.Update(coder);
        await _coderRepository.SaveChangesAsync();

        return await MapToDto(coder);
    }

    private async Task<CoderResponseDto> MapToDto(Coder coder)
    {
        var user = await _userManager.FindByIdAsync(coder.UserId);
        
        return new CoderResponseDto
        {
            Id = coder.Id,
            FullName = coder.FullName,
            UserId = coder.UserId,
            Email = user?.Email ?? string.Empty,
            DocumentType = coder.DocumentType,
            Identification = coder.Identification,
            Address = coder.Address,
            BirthDate = coder.BirthDate,
            Country = coder.Country,
            City = coder.City,
            Gender = coder.Gender,
            StatusId = coder.StatusId,
            StatusName = coder.Status?.Name ?? "Unknown", // Assuming StatusCoder has a Name property
            ProfessionalProfileId = coder.ProfessionalProfileId
        };
    }
}
