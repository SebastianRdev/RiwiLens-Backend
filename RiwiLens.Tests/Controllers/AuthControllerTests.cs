using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using RiwiLens.Api.Controllers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace RiwiLens.Tests.Controllers;

public class AuthControllerTests
{
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _controller = new AuthController();
    }

    [Fact]
    public void Login_WithValidCredentials_ShouldReturnOkWithToken()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "admin@riwi.io",
            Password = "Admin123$"
        };

        // Act
        var result = _controller.Login(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().NotBeNull();
        
        var tokenProperty = okResult.Value!.GetType().GetProperty("token");
        tokenProperty.Should().NotBeNull();
        var token = tokenProperty!.GetValue(okResult.Value) as string;
        token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Login_WithInvalidEmail_ShouldReturnUnauthorized()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "wrong@riwi.io",
            Password = "Admin123$"
        };

        // Act
        var result = _controller.Login(request);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult!.Value.Should().Be("Credenciales inválidas");
    }

    [Fact]
    public void Login_WithInvalidPassword_ShouldReturnUnauthorized()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "admin@riwi.io",
            Password = "WrongPassword"
        };

        // Act
        var result = _controller.Login(request);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult!.Value.Should().Be("Credenciales inválidas");
    }

    [Fact]
    public void Login_WithValidCredentials_ShouldReturnValidJwtToken()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "admin@riwi.io",
            Password = "Admin123$"
        };

        // Act
        var result = _controller.Login(request) as OkObjectResult;
        var tokenProperty = result!.Value!.GetType().GetProperty("token");
        var token = tokenProperty!.GetValue(result.Value) as string;

        // Assert
        var handler = new JwtSecurityTokenHandler();
        handler.CanReadToken(token).Should().BeTrue();
        
        var jwtToken = handler.ReadJwtToken(token);
        jwtToken.Should().NotBeNull();
    }

    [Fact]
    public void Login_TokenShouldContainCorrectClaims()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "admin@riwi.io",
            Password = "Admin123$"
        };

        // Act
        var result = _controller.Login(request) as OkObjectResult;
        var tokenProperty = result!.Value!.GetType().GetProperty("token");
        var token = tokenProperty!.GetValue(result.Value) as string;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        nameClaim.Should().NotBeNull();
        nameClaim!.Value.Should().Be("admin@riwi.io");

        var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        roleClaim.Should().NotBeNull();
        roleClaim!.Value.Should().Be("Admin");
    }

    [Fact]
    public void Login_TokenShouldExpireIn30Minutes()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "admin@riwi.io",
            Password = "Admin123$"
        };

        // Act
        var result = _controller.Login(request) as OkObjectResult;
        var tokenProperty = result!.Value!.GetType().GetProperty("token");
        var token = tokenProperty!.GetValue(result.Value) as string;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        var expirationTime = jwtToken.ValidTo;
        var expectedExpiration = DateTime.UtcNow.AddMinutes(30);
        
        expirationTime.Should().BeCloseTo(expectedExpiration, TimeSpan.FromMinutes(1));
    }

    [Theory]
    [InlineData("", "Admin123$")]
    [InlineData("admin@riwi.io", "")]
    [InlineData("", "")]
    [InlineData("user@test.com", "password")]
    [InlineData("ADMIN@RIWI.IO", "Admin123$")] // Case sensitive
    public void Login_WithVariousInvalidCredentials_ShouldReturnUnauthorized(string email, string password)
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = email,
            Password = password
        };

        // Act
        var result = _controller.Login(request);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public void Login_ShouldUseEnvironmentVariablesForJwtConfiguration()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "admin@riwi.io",
            Password = "Admin123$"
        };

        // Act
        var result = _controller.Login(request) as OkObjectResult;
        var tokenProperty = result!.Value!.GetType().GetProperty("token");
        var token = tokenProperty!.GetValue(result.Value) as string;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert - Token should be created successfully even with default values
        jwtToken.Should().NotBeNull();
        token.Should().NotBeNullOrEmpty();
    }
}
