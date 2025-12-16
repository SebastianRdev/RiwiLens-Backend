using src.RiwiLens.Domain.Entities;
using src.RiwiLens.Infrastructure.Persistence;

namespace src.RiwiLens.Infrastructure.Data.Seed;

public static class SoftSkillSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (context.SoftSkills.Any()) return;

        var skills = new List<SoftSkill>
        {
            SoftSkill.Create("Communication", "Clear and effective communication of ideas"),
            SoftSkill.Create("Teamwork", "Ability to work collaboratively in a team"),
            SoftSkill.Create("Problem Solving", "Ability to analyze and solve problems effectively"),
            SoftSkill.Create("Critical Thinking", "Logical and analytical reasoning skills"),
            SoftSkill.Create("Adaptability", "Ability to adapt to change and new situations"),
            SoftSkill.Create("Time Management", "Effective management of time and priorities"),
            SoftSkill.Create("Leadership", "Ability to guide and motivate others"),
            SoftSkill.Create("Responsibility", "Commitment and accountability for tasks"),
            SoftSkill.Create("Self Learning", "Ability to learn independently"),
            SoftSkill.Create("Creativity", "Generating innovative ideas and solutions"),
            SoftSkill.Create("Emotional Intelligence", "Understanding and managing emotions effectively"),
            SoftSkill.Create("Conflict Resolution", "Ability to resolve disagreements constructively"),
            SoftSkill.Create("Public Speaking", "Confidence and clarity when speaking in public"),
            SoftSkill.Create("Decision Making", "Making informed and timely decisions"),
            SoftSkill.Create("Work Ethics", "Professional behavior and integrity at work"),
            SoftSkill.Create("Attention to Detail", "Focus on accuracy and quality"),
            SoftSkill.Create("Stress Management", "Ability to work under pressure"),
            SoftSkill.Create("Empathy", "Understanding others' perspectives and feelings"),
            SoftSkill.Create("Feedback Reception", "Ability to receive and apply feedback constructively"),
            SoftSkill.Create("Proactivity", "Taking initiative without being asked"),
            SoftSkill.Create("Resilience", "Ability to recover from setbacks"),
            SoftSkill.Create("Negotiation", "Reaching agreements through discussion"),
            SoftSkill.Create("Collaboration", "Working effectively with diverse teams"),
            SoftSkill.Create("Active Listening", "Listening attentively and responding appropriately"),
            SoftSkill.Create("Accountability", "Taking ownership of actions and results"),
            SoftSkill.Create("Conflict Management", "Managing conflicts in a professional manner"),
            SoftSkill.Create("Growth Mindset", "Belief in continuous learning and improvement"),
            SoftSkill.Create("Adaptability to Feedback", "Adjusting behavior based on constructive feedback")
        };

        context.SoftSkills.AddRange(skills);
        await context.SaveChangesAsync();
    }
}
