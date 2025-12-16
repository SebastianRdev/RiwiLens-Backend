using src.RiwiLens.Application.DTOs.Feedback;

namespace src.RiwiLens.Application.Interfaces.Services;

public interface IFeedbackService
{
    Task<FeedbackResponseDto> CreateAsync(CreateFeedbackDto dto);
    Task<IEnumerable<FeedbackResponseDto>> GetByCoderIdAsync(int coderId);
    Task<IEnumerable<FeedbackResponseDto>> GetByTeamLeaderIdAsync(int teamLeaderId);
}
