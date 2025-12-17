using Microsoft.AspNetCore.Identity;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Application.Interfaces.Services;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Application.Services;

public class PdfService : IPdfService
{
    private readonly IGenericRepository<Coder> _coderRepository;
    private readonly IGenericRepository<CoderTechnicalSkill> _coderSkillRepository;
    private readonly IGenericRepository<TechnicalSkill> _skillRepository;
    private readonly IGenericRepository<ClanCoder> _clanCoderRepository;
    private readonly IGenericRepository<Clan> _clanRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public PdfService(
        IGenericRepository<Coder> coderRepository,
        IGenericRepository<CoderTechnicalSkill> coderSkillRepository,
        IGenericRepository<TechnicalSkill> skillRepository,
        IGenericRepository<ClanCoder> clanCoderRepository,
        IGenericRepository<Clan> clanRepository,
        UserManager<ApplicationUser> userManager)
    {
        _coderRepository = coderRepository;
        _coderSkillRepository = coderSkillRepository;
        _skillRepository = skillRepository;
        _clanCoderRepository = clanCoderRepository;
        _clanRepository = clanRepository;
        _userManager = userManager;
    }

    public async Task<byte[]> GenerateCvAsync(int coderId)
    {
        var coder = await _coderRepository.GetByIdAsync(coderId);
        if (coder == null) throw new KeyNotFoundException($"Coder with ID {coderId} not found.");

        // Fetch User for Email
        var user = await _userManager.FindByIdAsync(coder.UserId);
        var email = user?.Email ?? "No email provided";

        // Fetch Skills
        var coderSkills = await _coderSkillRepository.FindAsync(cs => cs.CoderId == coderId);
        var skills = new List<string>();
        foreach (var cs in coderSkills)
        {
            var skill = await _skillRepository.GetByIdAsync(cs.SkillId);
            if (skill != null) skills.Add(skill.Name);
        }

        // Fetch Clan
        var clanCoders = await _clanCoderRepository.FindAsync(cc => cc.CoderId == coderId && cc.IsActive);
        var clanName = "N/A";
        if (clanCoders.Any())
        {
            var clan = await _clanRepository.GetByIdAsync(clanCoders.First().ClanId);
            if (clan != null) clanName = clan.Name;
        }

        // Image Path
        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "docs", "foto-pdf.png");
        byte[] imageBytes = File.Exists(imagePath) ? File.ReadAllBytes(imagePath) : Array.Empty<byte>();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Arial));

                page.Content().Column(col =>
                {
                    // Header
                    col.Item().Row(row =>
                    {
                        // Left: Info
                        row.RelativeItem().Column(info =>
                        {
                            info.Item().Text(coder.FullName).FontSize(24).Bold();
                            info.Item().Text("Junior Developer at Riwi (in training)").FontSize(12);
                            info.Item().Text($"{coder.City}, {coder.Country} | {email}").FontSize(10).FontColor(Colors.Grey.Medium);
                            // Links placeholder
                            info.Item().Text("LinkedIn | GitHub").FontSize(10).FontColor(Colors.Blue.Medium);
                        });

                        // Right: Image
                        if (imageBytes.Length > 0)
                        {
                            row.ConstantItem(100).Image(imageBytes).FitArea();
                        }
                    });

                    col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);

                    // Professional Profile
                    col.Item().Text("PROFESSIONAL PROFILE").FontSize(14).Bold();
                    col.Item().PaddingBottom(10).Text("Junior developer with passion for learning... (Placeholder for Profile Description)").FontSize(10);

                    // Technical Skills
                    col.Item().Text("TECHNICAL SKILLS").FontSize(14).Bold();
                    col.Item().PaddingBottom(10).Text(string.Join(", ", skills)).FontSize(10);

                    // Soft Skills (Placeholder)
                    col.Item().Text("SOFT SKILLS").FontSize(14).Bold();
                    col.Item().PaddingBottom(10).Text("Teamwork, Communication, Problem Solving").FontSize(10);

                    // Education (Placeholder)
                    col.Item().Text("EDUCATION").FontSize(14).Bold();
                    col.Item().Text("Riwi - Bootcamp").FontSize(10).Bold();
                    col.Item().PaddingBottom(10).Text("Full Stack Developer Trainee").FontSize(10);

                    // Languages (Placeholder)
                    col.Item().Text("LANGUAGES").FontSize(14).Bold();
                    col.Item().Text("Spanish (Native), English (B1)").FontSize(10);
                });
            });
        });

        return document.GeneratePdf();
    }
}
