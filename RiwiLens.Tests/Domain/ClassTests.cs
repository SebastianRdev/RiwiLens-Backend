using FluentAssertions;
using src.RiwiLens.Domain.Entities;
using Xunit;

namespace RiwiLens.Tests.Domain;

public class ClassTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateClass()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var dayId = 1;
        var classTypeId = 1;
        var teamLeaderId = 1;
        var startTime = new TimeSpan(9, 0, 0); // 9:00 AM
        var endTime = new TimeSpan(12, 0, 0);  // 12:00 PM

        // Act
        var classEntity = Class.Create(date, dayId, classTypeId, teamLeaderId, startTime, endTime);

        // Assert
        classEntity.Should().NotBeNull();
        classEntity.Date.Should().Be(date);
        classEntity.DayId.Should().Be(dayId);
        classEntity.ClassTypeId.Should().Be(classTypeId);
        classEntity.TeamLeaderId.Should().Be(teamLeaderId);
        classEntity.StartTime.Should().Be(startTime);
        classEntity.EndTime.Should().Be(endTime);
        classEntity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        classEntity.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_WithEndTimeBeforeStartTime_ShouldThrowException()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var startTime = new TimeSpan(12, 0, 0); // 12:00 PM
        var endTime = new TimeSpan(9, 0, 0);    // 9:00 AM (before start)

        // Act
        var act = () => Class.Create(date, 1, 1, 1, startTime, endTime);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The end time must be later.*");
    }

    [Fact]
    public void Create_WithSameStartAndEndTime_ShouldThrowException()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var time = new TimeSpan(10, 0, 0); // 10:00 AM

        // Act
        var act = () => Class.Create(date, 1, 1, 1, time, time);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The end time must be later.*");
    }

    [Fact]
    public void Create_WithOneMinuteDifference_ShouldCreateClass()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var startTime = new TimeSpan(10, 0, 0);  // 10:00 AM
        var endTime = new TimeSpan(10, 1, 0);    // 10:01 AM

        // Act
        var classEntity = Class.Create(date, 1, 1, 1, startTime, endTime);

        // Assert
        classEntity.Should().NotBeNull();
        classEntity.StartTime.Should().Be(startTime);
        classEntity.EndTime.Should().Be(endTime);
    }

    [Fact]
    public void Create_WithFullDayClass_ShouldCreateClass()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var startTime = new TimeSpan(8, 0, 0);   // 8:00 AM
        var endTime = new TimeSpan(18, 0, 0);    // 6:00 PM

        // Act
        var classEntity = Class.Create(date, 1, 1, 1, startTime, endTime);

        // Assert
        classEntity.Should().NotBeNull();
        classEntity.StartTime.Should().Be(startTime);
        classEntity.EndTime.Should().Be(endTime);
        (classEntity.EndTime - classEntity.StartTime).Should().Be(TimeSpan.FromHours(10));
    }

    [Fact]
    public void Create_WithMidnightCrossing_ShouldCreateClass()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var startTime = new TimeSpan(22, 0, 0);  // 10:00 PM
        var endTime = new TimeSpan(23, 59, 59);  // 11:59:59 PM

        // Act
        var classEntity = Class.Create(date, 1, 1, 1, startTime, endTime);

        // Assert
        classEntity.Should().NotBeNull();
        classEntity.StartTime.Should().Be(startTime);
        classEntity.EndTime.Should().Be(endTime);
    }

    [Theory]
    [InlineData(9, 0, 0, 17, 0, 0)]   // 9 AM to 5 PM
    [InlineData(14, 30, 0, 16, 45, 0)] // 2:30 PM to 4:45 PM
    [InlineData(0, 0, 0, 23, 59, 59)]  // Midnight to end of day
    public void Create_WithVariousValidTimeRanges_ShouldCreateClass(
        int startHour, int startMinute, int startSecond,
        int endHour, int endMinute, int endSecond)
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var startTime = new TimeSpan(startHour, startMinute, startSecond);
        var endTime = new TimeSpan(endHour, endMinute, endSecond);

        // Act
        var classEntity = Class.Create(date, 1, 1, 1, startTime, endTime);

        // Assert
        classEntity.Should().NotBeNull();
        classEntity.StartTime.Should().Be(startTime);
        classEntity.EndTime.Should().Be(endTime);
        classEntity.EndTime.Should().BeGreaterThan(classEntity.StartTime);
    }
}
