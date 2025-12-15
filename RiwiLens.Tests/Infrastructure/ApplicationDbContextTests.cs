using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using src.RiwiLens.Domain.Entities;
using src.RiwiLens.Domain.Enums;
using src.RiwiLens.Infrastructure.Persistence;
using Xunit;

namespace RiwiLens.Tests.Infrastructure;

public class ApplicationDbContextTests : IDisposable
{
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
    }

    [Fact]
    public void DbContext_ShouldHaveAllRequiredDbSets()
    {
        // Assert
        _context.Coders.Should().NotBeNull();
        _context.TeamLeaders.Should().NotBeNull();
        _context.Attendances.Should().NotBeNull();
        _context.FaceCollections.Should().NotBeNull();
        _context.Clans.Should().NotBeNull();
        _context.Classes.Should().NotBeNull();
        _context.ClassTypes.Should().NotBeNull();
        _context.Days.Should().NotBeNull();
        _context.SoftSkills.Should().NotBeNull();
        _context.Specialties.Should().NotBeNull();
        _context.RoleTeamLeaders.Should().NotBeNull();
        _context.TechnicalSkills.Should().NotBeNull();
        _context.CategoryTechnicalSkills.Should().NotBeNull();
        _context.StatusCoders.Should().NotBeNull();
        _context.Notifications.Should().NotBeNull();
        _context.Feedbacks.Should().NotBeNull();
        _context.ProfessionalProfiles.Should().NotBeNull();
    }

    [Fact]
    public async Task DbContext_ShouldSaveAndRetrieveClan()
    {
        // Arrange
        var clan = Clan.Create("Test Clan", "Test Description");

        // Act
        _context.Clans.Add(clan);
        await _context.SaveChangesAsync();

        var retrievedClan = await _context.Clans.FirstOrDefaultAsync();

        // Assert
        retrievedClan.Should().NotBeNull();
        retrievedClan!.Name.Should().Be("Test Clan");
        retrievedClan.Description.Should().Be("Test Description");
    }

    [Fact]
    public async Task DbContext_ShouldSaveAndRetrieveClass()
    {
        // Arrange
        var classEntity = Class.Create(
            new DateTime(2024, 1, 15),
            1,
            1,
            1,
            new TimeSpan(9, 0, 0),
            new TimeSpan(12, 0, 0)
        );

        // Act
        _context.Classes.Add(classEntity);
        await _context.SaveChangesAsync();

        var retrievedClass = await _context.Classes.FirstOrDefaultAsync();

        // Assert
        retrievedClass.Should().NotBeNull();
        retrievedClass!.Date.Should().Be(new DateTime(2024, 1, 15));
        retrievedClass.StartTime.Should().Be(new TimeSpan(9, 0, 0));
        retrievedClass.EndTime.Should().Be(new TimeSpan(12, 0, 0));
    }

    [Fact]
    public async Task DbContext_ShouldUpdateClanSuccessfully()
    {
        // Arrange
        var clan = Clan.Create("Original Name", "Original Description");
        _context.Clans.Add(clan);
        await _context.SaveChangesAsync();

        // Act
        clan.UpdateInfo("Updated Name", "Updated Description");
        await _context.SaveChangesAsync();

        var updatedClan = await _context.Clans.FirstOrDefaultAsync();

        // Assert
        updatedClan.Should().NotBeNull();
        updatedClan!.Name.Should().Be("Updated Name");
        updatedClan.Description.Should().Be("Updated Description");
    }

    [Fact]
    public async Task DbContext_ShouldDeleteClanSuccessfully()
    {
        // Arrange
        var clan = Clan.Create("Test Clan", "Test Description");
        _context.Clans.Add(clan);
        await _context.SaveChangesAsync();

        // Act
        _context.Clans.Remove(clan);
        await _context.SaveChangesAsync();

        var deletedClan = await _context.Clans.FirstOrDefaultAsync();

        // Assert
        deletedClan.Should().BeNull();
    }

    [Fact]
    public async Task DbContext_ShouldHandleMultipleClans()
    {
        // Arrange
        var clan1 = Clan.Create("Clan 1", "Description 1");
        var clan2 = Clan.Create("Clan 2", "Description 2");
        var clan3 = Clan.Create("Clan 3", "Description 3");

        // Act
        _context.Clans.AddRange(clan1, clan2, clan3);
        await _context.SaveChangesAsync();

        var clans = await _context.Clans.ToListAsync();

        // Assert
        clans.Should().HaveCount(3);
        clans.Should().Contain(c => c.Name == "Clan 1");
        clans.Should().Contain(c => c.Name == "Clan 2");
        clans.Should().Contain(c => c.Name == "Clan 3");
    }

    [Fact]
    public async Task DbContext_ShouldSaveAttendanceWithCorrectStatus()
    {
        // Arrange
        var attendance = Attendance.Create(
            1,
            1,
            1,
            DateTime.UtcNow,
            "FACIAL_RECOGNITION",
            "https://example.com/image.jpg"
        );

        // Act
        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();

        var retrievedAttendance = await _context.Attendances.FirstOrDefaultAsync();

        // Assert
        retrievedAttendance.Should().NotBeNull();
        retrievedAttendance!.Status.Should().Be(AttendanceStatus.Present);
        retrievedAttendance.VerifiedBy.Should().Be("FACIAL_RECOGNITION");
    }

    [Fact]
    public async Task DbContext_ShouldPersistAttendanceStatusChanges()
    {
        // Arrange
        var attendance = Attendance.Create(
            1,
            1,
            1,
            DateTime.UtcNow,
            "FACIAL_RECOGNITION",
            "https://example.com/image.jpg"
        );
        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();

        // Act
        attendance.MarkAsAbsent();
        await _context.SaveChangesAsync();

        var updatedAttendance = await _context.Attendances.FirstOrDefaultAsync();

        // Assert
        updatedAttendance.Should().NotBeNull();
        updatedAttendance!.Status.Should().Be(AttendanceStatus.Absent);
        updatedAttendance.VerifiedBy.Should().Be("SYSTEM");
    }

    [Fact]
    public void DbContext_ShouldApplyConfigurationsFromAssembly()
    {
        // Arrange & Act
        var model = _context.Model;

        // Assert
        model.Should().NotBeNull();
        var entityTypes = model.GetEntityTypes();
        entityTypes.Should().NotBeEmpty();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
