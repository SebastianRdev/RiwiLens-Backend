using FluentAssertions;
using src.RiwiLens.Domain.Entities;
using src.RiwiLens.Domain.Enums;
using Xunit;

namespace RiwiLens.Tests.Domain;

public class CoderTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateCoder()
    {
        // Arrange
        var fullName = "John Doe";
        var userId = "user123";
        var documentType = DocumentType.CC;
        var identification = "123456789";
        var birthDate = new DateTime(1990, 1, 1);
        var address = "123 Main St";
        var country = "Colombia";
        var city = "Medellín";
        var gender = Gender.Male;
        var professionalProfileId = 1;
        var statusId = 1;

        // Act
        var coder = Coder.Create(
            fullName,
            userId,
            documentType,
            identification,
            birthDate,
            address,
            country,
            city,
            gender,
            professionalProfileId,
            statusId
        );

        // Assert
        coder.Should().NotBeNull();
        coder.FullName.Should().Be(fullName);
        coder.UserId.Should().Be(userId);
        coder.DocumentType.Should().Be(documentType);
        coder.Identification.Should().Be(identification);
        coder.BirthDate.Kind.Should().Be(DateTimeKind.Utc);
        coder.Address.Should().Be(address);
        coder.Country.Should().Be(country);
        coder.City.Should().Be(city);
        coder.Gender.Should().Be(gender);
        coder.ProfessionalProfileId.Should().Be(professionalProfileId);
        coder.StatusId.Should().Be(statusId);
        coder.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        coder.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_WithUnknownDocumentType_ShouldThrowException()
    {
        // Arrange & Act
        var act = () => Coder.Create(
            "John Doe",
            "user123",
            DocumentType.Unknown,
            "123456789",
            new DateTime(1990, 1, 1),
            "123 Main St",
            "Colombia",
            "Medellín",
            Gender.Male,
            1,
            1
        );

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Invalid document type.");
    }

    [Fact]
    public void Create_WithUnknownGender_ShouldThrowException()
    {
        // Arrange & Act
        var act = () => Coder.Create(
            "John Doe",
            "user123",
            DocumentType.CC,
            "123456789",
            new DateTime(1990, 1, 1),
            "123 Main St",
            "Colombia",
            "Medellín",
            Gender.Unknown,
            1,
            1
        );

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Invalid gender.");
    }

    [Fact]
    public void UpdatePersonalInfo_WithValidData_ShouldUpdateCoder()
    {
        // Arrange
        var coder = CreateValidCoder();
        var newFullName = "Jane Doe";
        var newBirthDate = new DateTime(1995, 5, 5);
        var newGender = Gender.Female;

        // Act
        coder.UpdatePersonalInfo(newFullName, newBirthDate, newGender);

        // Assert
        coder.FullName.Should().Be(newFullName);
        coder.BirthDate.Should().Be(DateTime.SpecifyKind(newBirthDate, DateTimeKind.Utc));
        coder.Gender.Should().Be(newGender);
        coder.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void UpdatePersonalInfo_WithUnknownGender_ShouldThrowException()
    {
        // Arrange
        var coder = CreateValidCoder();

        // Act
        var act = () => coder.UpdatePersonalInfo("Jane Doe", new DateTime(1995, 5, 5), Gender.Unknown);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Invalid gender.");
    }

    [Fact]
    public void UpdateLocation_WithValidData_ShouldUpdateLocation()
    {
        // Arrange
        var coder = CreateValidCoder();
        var newAddress = "456 New St";
        var newCountry = "USA";
        var newCity = "New York";

        // Act
        coder.UpdateLocation(newAddress, newCountry, newCity);

        // Assert
        coder.Address.Should().Be(newAddress);
        coder.Country.Should().Be(newCountry);
        coder.City.Should().Be(newCity);
        coder.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ChangeDocument_WithValidData_ShouldUpdateDocument()
    {
        // Arrange
        var coder = CreateValidCoder();
        var newDocumentType = DocumentType.Passport;
        var newIdentification = "ABC123456";

        // Act
        coder.ChangeDocument(newDocumentType, newIdentification);

        // Assert
        coder.DocumentType.Should().Be(newDocumentType);
        coder.Identification.Should().Be(newIdentification);
        coder.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ChangeDocument_WithUnknownDocumentType_ShouldThrowException()
    {
        // Arrange
        var coder = CreateValidCoder();

        // Act
        var act = () => coder.ChangeDocument(DocumentType.Unknown, "123456789");

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Invalid document type.");
    }

    [Fact]
    public void ChangeStatus_ShouldUpdateStatus()
    {
        // Arrange
        var coder = CreateValidCoder();
        var newStatusId = 2;

        // Act
        coder.ChangeStatus(newStatusId);

        // Assert
        coder.StatusId.Should().Be(newStatusId);
        coder.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    private static Coder CreateValidCoder()
    {
        return Coder.Create(
            "John Doe",
            "user123",
            DocumentType.CC,
            "123456789",
            new DateTime(1990, 1, 1),
            "123 Main St",
            "Colombia",
            "Medellín",
            Gender.Male,
            1,
            1
        );
    }
}
