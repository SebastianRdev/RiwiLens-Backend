using FluentAssertions;
using src.RiwiLens.Domain.Entities;
using Xunit;

namespace RiwiLens.Tests.Domain;

public class ClanTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateClan()
    {
        // Arrange
        var name = "Clan Alpha";
        var description = "First clan of coders";

        // Act
        var clan = Clan.Create(name, description);

        // Assert
        clan.Should().NotBeNull();
        clan.Name.Should().Be(name);
        clan.Description.Should().Be(description);
        clan.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        clan.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithInvalidName_ShouldThrowException(string name)
    {
        // Arrange & Act
        var act = () => Clan.Create(name, "Description");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Clan name is required.*");
    }

    [Fact]
    public void Create_WithEmptyDescription_ShouldCreateClan()
    {
        // Arrange
        var name = "Clan Beta";
        var description = "";

        // Act
        var clan = Clan.Create(name, description);

        // Assert
        clan.Should().NotBeNull();
        clan.Name.Should().Be(name);
        clan.Description.Should().Be(description);
    }

    [Fact]
    public void UpdateInfo_WithValidData_ShouldUpdateClan()
    {
        // Arrange
        var clan = Clan.Create("Original Name", "Original Description");
        var newName = "Updated Name";
        var newDescription = "Updated Description";
        var originalCreatedAt = clan.CreatedAt;

        // Act
        clan.UpdateInfo(newName, newDescription);

        // Assert
        clan.Name.Should().Be(newName);
        clan.Description.Should().Be(newDescription);
        clan.CreatedAt.Should().Be(originalCreatedAt); // CreatedAt should not change
        clan.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        clan.UpdatedAt.Should().BeAfter(originalCreatedAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void UpdateInfo_WithInvalidName_ShouldThrowException(string name)
    {
        // Arrange
        var clan = Clan.Create("Original Name", "Original Description");

        // Act
        var act = () => clan.UpdateInfo(name, "New Description");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name is required.*");
    }

    [Fact]
    public void UpdateInfo_WithEmptyDescription_ShouldUpdateClan()
    {
        // Arrange
        var clan = Clan.Create("Original Name", "Original Description");
        var newName = "Updated Name";

        // Act
        clan.UpdateInfo(newName, "");

        // Assert
        clan.Name.Should().Be(newName);
        clan.Description.Should().BeEmpty();
    }

    [Fact]
    public void UpdateInfo_ShouldOnlyUpdateTimestamp()
    {
        // Arrange
        var clan = Clan.Create("Clan Name", "Description");
        var originalCreatedAt = clan.CreatedAt;
        var originalUpdatedAt = clan.UpdatedAt;

        // Wait a bit to ensure timestamp difference
        Thread.Sleep(10);

        // Act
        clan.UpdateInfo("Clan Name", "Description");

        // Assert
        clan.CreatedAt.Should().Be(originalCreatedAt);
        clan.UpdatedAt.Should().BeAfter(originalUpdatedAt);
    }
}
