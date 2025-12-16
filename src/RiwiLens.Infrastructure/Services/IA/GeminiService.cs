using System.Text;
using Google.GenAI;
using Microsoft.Extensions.Logging;
using src.RiwiLens.Application.Interfaces.Services;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Infrastructure.Services.IA;

public class GeminiService : IAiService
{
    private readonly Client _client;
    private readonly ILogger<GeminiService> _logger;
    private const string MODEL = "gemini-2.5-flash";

    public GeminiService(Client client, ILogger<GeminiService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<string> GenerateCvContentAsync(Coder coder, ProfessionalProfile? profile, List<string> techSkills, List<string> softSkills)
    {
        int maxRetries = 3;
        int delay = 1000; // 1 second

        for (int i = 0; i <= maxRetries; i++)
        {
            try
            {
                var prompt = BuildPrompt(coder, profile, techSkills, softSkills);
                
                var response = await _client.Models.GenerateContentAsync(
                    model: MODEL,
                    contents: prompt
                );

                var text = response?.Candidates?[0]?.Content?.Parts?[0]?.Text?.Trim();
                return text ?? "No content generated.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Attempt {i + 1} failed: {ex.Message}");

                if (i == maxRetries)
                {
                    return $"Error generating CV after {maxRetries + 1} attempts: {ex.Message}";
                }

                if (ex.Message.Contains("overloaded") || ex.Message.Contains("503"))
                {
                    await Task.Delay(delay);
                    delay *= 2; // Exponential backoff
                }
                else
                {
                    // If it's not an overload error, maybe we shouldn't retry, but for robustness we will for now.
                    // Or rethrow if it's a bad request.
                    // For now, let's retry on everything but log it.
                    await Task.Delay(delay);
                }
            }
        }

        return "Error generating CV: Maximum retries exceeded.";
    }

    private string BuildPrompt(Coder coder, ProfessionalProfile? profile, List<string> techSkills, List<string> softSkills)
    {
        var sb = new StringBuilder();
        sb.AppendLine("You are an expert CV writer. Generate a professional CV for a developer based on the following information.");
        sb.AppendLine("You MUST follow the EXACT format provided below. Do NOT use markdown code blocks (like ```). Return plain text matching the format.");
        
        sb.AppendLine("\n--- CODER INFORMATION ---");
        sb.AppendLine($"Name: {coder.FullName}");
        sb.AppendLine($"Email: {coder.UserId}"); 
        sb.AppendLine($"Location: {coder.City}, {coder.Country}");
        sb.AppendLine($"Phone: +57 3000000000"); 
        
        if (profile != null)
        {
            sb.AppendLine($"About Me (Draft): {profile.AboutMe}");
            sb.AppendLine($"LinkedIn: {profile.LinkedIn}");
            sb.AppendLine($"GitHub: {profile.GitHub}");
            sb.AppendLine($"Portfolio: {profile.Portfolio}");
        }
        
        sb.AppendLine($"Technical Skills: {string.Join(", ", techSkills)}");
        sb.AppendLine($"Soft Skills: {string.Join(", ", softSkills)}");
        
        sb.AppendLine("\n--- INSTRUCTIONS ---");
        sb.AppendLine("1. PROFESSIONAL PROFILE: Improve the 'About Me' text to be more professional. If it's empty, generate a professional profile based on the skills.");
        sb.AppendLine("2. PROJECTS: Generate 1 or 2 FICTITIOUS projects that demonstrate the coder's technical skills. Include 'Tech stack' and 'Tools' for each.");
        sb.AppendLine("3. EDUCATION: Generate 1 or 2 FICTITIOUS education entries relevant to the coder's profile (e.g. Systems Engineering, Bootcamps).");
        sb.AppendLine("4. LANGUAGES: Include 'Spanish: Native' and 'English: Beginner/intermediate/advanced' (Choose one appropriate for a junior dev).");
        sb.AppendLine("5. FORMAT: Use the EXACT format below. Replace placeholders (XXXX) with generated content.");

        sb.AppendLine("\n--- REQUIRED FORMAT ---");
        sb.AppendLine($"{coder.FullName} Coder picture");
        sb.AppendLine("Junior Developer at Riwi (in training) | [Professional Title]");
        sb.AppendLine($"{coder.City}, {coder.Country} | Phone: +57 [Phone] | {coder.UserId} | [LinkedIn Link] | [GitHub Link]");
        sb.AppendLine("PROFESSIONAL PROFILE");
        sb.AppendLine("[Generated Professional Profile Text]");
        sb.AppendLine("TECHNICAL SKILLS");
        sb.AppendLine("[List of Technical Skills]");
        sb.AppendLine("SOFT SKILLS");
        sb.AppendLine("[List of Soft Skills]");
        sb.AppendLine("EDUCATION");
        sb.AppendLine("[Degree Name], [Institution Name], [City] - [Country]");
        sb.AppendLine("[Month Year] – [Month Year]");
        sb.AppendLine("LANGUAGES");
        sb.AppendLine("Spanish: Native");
        sb.AppendLine("English: [Level]");
        sb.AppendLine("PROFESSIONAL EXPERIENCE / RIWI PROJECTS");
        sb.AppendLine("[Project Name]");
        sb.AppendLine("Riwi Training Center - ([Month Year] – [Month Year])");
        sb.AppendLine("[Project Description]");
        sb.AppendLine("Key Achievements /Learnings");
        sb.AppendLine("• [Achievement 1]");
        sb.AppendLine("• [Achievement 2]");
        sb.AppendLine("Tech stack: [Stack]");
        sb.AppendLine("Tools: [Tools]");
        sb.AppendLine("[Optional Second Project Name]");
        sb.AppendLine("Riwi Training Center - ([Month Year] – [Month Year])");
        sb.AppendLine("[Project Description]");
        sb.AppendLine("Key Achievements/Learnings");
        sb.AppendLine("• [Achievement 1]");
        sb.AppendLine("• [Achievement 2]");
        sb.AppendLine("Tech stack: [Stack]");
        sb.AppendLine("Tools: [Tools]");

        return sb.ToString();
    }
}
