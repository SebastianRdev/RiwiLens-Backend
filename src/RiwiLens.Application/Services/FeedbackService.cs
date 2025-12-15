using src.RiwiLens.Application.DTOs.Feedback;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Application.Interfaces.Services;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Application.Services;

public class FeedbackService : IFeedbackService
{
    private readonly IGenericRepository<Feedback> _feedbackRepository;
    private readonly IGenericRepository<Coder> _coderRepository;
    private readonly IGenericRepository<TeamLeader> _tlRepository;

    public FeedbackService(
        IGenericRepository<Feedback> feedbackRepository,
        IGenericRepository<Coder> coderRepository,
        IGenericRepository<TeamLeader> tlRepository)
    {
        _feedbackRepository = feedbackRepository;
        _coderRepository = coderRepository;
        _tlRepository = tlRepository;
    }

    public async Task<FeedbackResponseDto> CreateAsync(CreateFeedbackDto dto)
    {
        var coder = await _coderRepository.GetByIdAsync(dto.CoderId);
        if (coder == null) throw new KeyNotFoundException("Coder not found");

        var tl = await _tlRepository.GetByIdAsync(dto.TeamLeaderId);
        if (tl == null) throw new KeyNotFoundException("TeamLeader not found");

        var feedback = Feedback.Create(dto.CoderId, dto.TeamLeaderId, dto.Message);

        await _feedbackRepository.AddAsync(feedback);
        await _feedbackRepository.SaveChangesAsync();

        return await MapToDto(feedback);
    }

    public async Task<IEnumerable<FeedbackResponseDto>> GetByCoderIdAsync(int coderId)
    {
        var feedbacks = await _feedbackRepository.FindAsync(f => f.CoderId == coderId);
        var dtos = new List<FeedbackResponseDto>();
        foreach (var f in feedbacks)
        {
            dtos.Add(await MapToDto(f));
        }
        return dtos;
    }

    public async Task<IEnumerable<FeedbackResponseDto>> GetByTeamLeaderIdAsync(int teamLeaderId)
    {
        var feedbacks = await _feedbackRepository.FindAsync(f => f.TeamLeaderId == teamLeaderId);
        var dtos = new List<FeedbackResponseDto>();
        foreach (var f in feedbacks)
        {
            dtos.Add(await MapToDto(f));
        }
        return dtos;
    }

    private async Task<FeedbackResponseDto> MapToDto(Feedback f)
    {
        var coder = await _coderRepository.GetByIdAsync(f.CoderId);
        var tl = await _tlRepository.GetByIdAsync(f.TeamLeaderId);

        return new FeedbackResponseDto
        {
            Id = f.Id,
            CoderId = f.CoderId,
            CoderName = coder?.FullName ?? "",
            TeamLeaderId = f.TeamLeaderId,
            TeamLeaderName = tl?.FullName ?? "",
            Message = f.Message,
            CreatedAt = f.CreatedAt
        };
    }
}
