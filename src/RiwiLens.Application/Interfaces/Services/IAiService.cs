using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Application.Interfaces.Services;

public interface IAiService
{
    Task<string> GenerateCvContentAsync(Coder coder, ProfessionalProfile? profile, List<string> techSkills, List<string> softSkills);
}
