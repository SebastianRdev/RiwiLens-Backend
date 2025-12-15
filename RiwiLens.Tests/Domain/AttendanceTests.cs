using FluentAssertions;
using src.RiwiLens.Domain.Entities;
using src.RiwiLens.Domain.Enums;
using Xunit;

namespace RiwiLens.Tests.Domain;

public class AttendanceTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateAttendance()
    {
        // Arrange
        var clanId = 1;
        var classId = 1;
        var coderId = 1;
        var timestampIn = DateTime.UtcNow;
        var verifiedBy = "FACIAL_RECOGNITION";
        var imageUrl = "https://example.com/image.jpg";

        // Act
        var attendance = Attendance.Create(clanId, classId, coderId, timestampIn, verifiedBy, imageUrl);

        // Assert
        attendance.Should().NotBeNull();
        attendance.ClanId.Should().Be(clanId);
        attendance.ClassId.Should().Be(classId);
        attendance.CoderId.Should().Be(coderId);
        attendance.TimestampIn.Should().Be(timestampIn);
        attendance.Status.Should().Be(AttendanceStatus.Present);
        attendance.VerifiedBy.Should().Be(verifiedBy);
        attendance.ImageUrl.Should().Be(imageUrl);
        attendance.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        attendance.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData(0, 1, 1)]
    [InlineData(-1, 1, 1)]
    [InlineData(1, 0, 1)]
    [InlineData(1, -1, 1)]
    [InlineData(1, 1, 0)]
    [InlineData(1, 1, -1)]
    public void Create_WithInvalidIds_ShouldThrowException(int clanId, int classId, int coderId)
    {
        // Arrange & Act
        var act = () => Attendance.Create(
            clanId,
            classId,
            coderId,
            DateTime.UtcNow,
            "FACIAL_RECOGNITION",
            "https://example.com/image.jpg"
        );

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithDefaultTimestamp_ShouldThrowException()
    {
        // Arrange & Act
        var act = () => Attendance.Create(
            1,
            1,
            1,
            default,
            "FACIAL_RECOGNITION",
            "https://example.com/image.jpg"
        );

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid timestamp.*");
    }

    [Fact]
    public void MarkAsJustified_WithValidJustification_ShouldUpdateStatus()
    {
        // Arrange
        var attendance = CreateValidAttendance();
        var justification = "Medical appointment";

        // Act
        attendance.MarkAsJustified(justification);

        // Assert
        attendance.Status.Should().Be(AttendanceStatus.Justified);
        attendance.VerifiedBy.Should().Be("MANUAL");
        attendance.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void MarkAsJustified_WithInvalidJustification_ShouldThrowException(string justification)
    {
        // Arrange
        var attendance = CreateValidAttendance();

        // Act
        var act = () => attendance.MarkAsJustified(justification);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Justification is mandatory.*");
    }

    [Fact]
    public void MarkAsAbsent_ShouldUpdateStatus()
    {
        // Arrange
        var attendance = CreateValidAttendance();

        // Act
        attendance.MarkAsAbsent();

        // Assert
        attendance.Status.Should().Be(AttendanceStatus.Absent);
        attendance.VerifiedBy.Should().Be("SYSTEM");
        attendance.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void MarkAsPresent_WithValidVerifier_ShouldUpdateStatus()
    {
        // Arrange
        var attendance = CreateValidAttendance();
        attendance.MarkAsAbsent(); // First mark as absent
        var verifier = "MANUAL_CHECK";

        // Act
        attendance.MarkAsPresent(verifier);

        // Assert
        attendance.Status.Should().Be(AttendanceStatus.Present);
        attendance.VerifiedBy.Should().Be(verifier);
        attendance.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void MarkAsPresent_WithInvalidVerifier_ShouldThrowException(string verifier)
    {
        // Arrange
        var attendance = CreateValidAttendance();

        // Act
        var act = () => attendance.MarkAsPresent(verifier);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Verification method required.*");
    }

    [Fact]
    public void StatusTransitions_ShouldWorkCorrectly()
    {
        // Arrange
        var attendance = CreateValidAttendance();

        // Act & Assert - Present to Absent
        attendance.Status.Should().Be(AttendanceStatus.Present);
        
        attendance.MarkAsAbsent();
        attendance.Status.Should().Be(AttendanceStatus.Absent);
        
        // Absent to Justified
        attendance.MarkAsJustified("Valid reason");
        attendance.Status.Should().Be(AttendanceStatus.Justified);
        
        // Justified to Present
        attendance.MarkAsPresent("MANUAL");
        attendance.Status.Should().Be(AttendanceStatus.Present);
    }

    private static Attendance CreateValidAttendance()
    {
        return Attendance.Create(
            1,
            1,
            1,
            DateTime.UtcNow,
            "FACIAL_RECOGNITION",
            "https://example.com/image.jpg"
        );
    }
}
