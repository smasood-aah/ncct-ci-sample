using System.Collections.Generic;
using Moq;
using Ncct.Blazor.Sample.Services;
using Ncct.Blazor.Sample.Shared.Interfaces;
using Ncct.Blazor.Sample.Shared.Models;
using Xunit;

namespace Ncct.Blazor.Sample.Tests;

public class BasicTests
{
    [Fact]
    public void SimpleTest()
    {
        var result = 2 + 2;
        Assert.Equal(4, result);
    }

    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(2, 3, 5)]
    public void DataTests(int a, int b, int expected)
    {
        var result = a + b;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("test", true)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void StringValidationTests(string? input, bool expectedResult)
    {
        var isValid = !string.IsNullOrEmpty(input);
        Assert.Equal(expectedResult, isValid);
    }

    [Fact]
    public void ListOperationsTest()
    {
        var list = new List<int> { 1, 2, 3 };
        list.Add(4);
        
        Assert.Equal(4, list.Count);
        Assert.Contains(3, list);
    }
}

public class MoqTests
{
    [Fact]
    public void Mock_BasicSetup_ReturnsConfiguredValue()
    {
        var mockRepository = new Mock<IUserRepository>();
        var expectedUser = new User { Id = 1, Name = "John Doe" };
        
        mockRepository.Setup(repo => repo.GetById(1)).Returns(expectedUser);
        
        var result = mockRepository.Object.GetById(1);
        Assert.Equal(expectedUser.Name, result.Name);
    }

    [Fact]
    public void Mock_VerifyMethodWasCalled()
    {
        var mockRepository = new Mock<IUserRepository>();
        var user = new User { Id = 1, Name = "Jane Doe" };
        
        mockRepository.Object.Save(user);
        mockRepository.Verify(repo => repo.Save(user), Times.Once);
    }
    
    [Fact]
    public void Mock_WithAnyParameter()
    {
        var mockRepository = new Mock<IUserRepository>();
        mockRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
            .Returns(new User { Id = 999, Name = "Default User" });
        
        var result1 = mockRepository.Object.GetById(1);
        var result2 = mockRepository.Object.GetById(100);
        
        Assert.Equal("Default User", result1.Name);
        Assert.Equal("Default User", result2.Name);
    }
    
    [Fact]
    public void Mock_WithParameterMatching()
    {
        var mockRepository = new Mock<IUserRepository>();
        mockRepository.Setup(repo => repo.GetById(It.Is<int>(id => id > 0)))
            .Returns(new User { Id = 1, Name = "Valid User" });
        
        var result = mockRepository.Object.GetById(5);
        Assert.Equal("Valid User", result.Name);
    }
    
    [Fact]
    public void Mock_ReturnsSequence()
    {
        var mockRepository = new Mock<IUserRepository>();
        mockRepository.SetupSequence(repo => repo.GetById(1))
            .Returns(new User { Id = 1, Name = "First Call" })
            .Returns(new User { Id = 1, Name = "Second Call" });
        
        var first = mockRepository.Object.GetById(1);
        var second = mockRepository.Object.GetById(1);
        
        Assert.Equal("First Call", first.Name);
        Assert.Equal("Second Call", second.Name);
    }

    [Fact]
    public void Mock_ThrowsException()
    {
        var mockRepository = new Mock<IUserRepository>();
        mockRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
            .Throws<InvalidOperationException>();
        
        Assert.Throws<InvalidOperationException>(() => mockRepository.Object.GetById(1));
    }

    [Fact]
    public void Mock_CallbackExecution()
    {
        var mockRepository = new Mock<IUserRepository>();
        var savedUser = (User?)null;
        
        mockRepository.Setup(repo => repo.Save(It.IsAny<User>()))
            .Callback<User>(user => savedUser = user);
        
        var newUser = new User { Id = 1, Name = "Callback Test" };
        
        mockRepository.Object.Save(newUser);
        
        Assert.NotNull(savedUser);
        Assert.Equal("Callback Test", savedUser.Name);
    }
    
    [Fact]
    public void Mock_WithDependencyInjection()
    {
        var mockRepository = new Mock<IUserRepository>();
        var mockEmailService = new Mock<IEmailService>();
        
        mockRepository.Setup(repo => repo.Save(It.IsAny<User>()));
        mockEmailService.Setup(service =>
            service.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        
        var userService = new UserService(mockRepository.Object, mockEmailService.Object);
        var user = new User { Id = 1, Name = "Test User", Email = "test@example.com" };
        
        userService.CreateUserAndNotify(user);
        
        mockRepository.Verify(repo => repo.Save(user), Times.Once);
        mockEmailService.Verify(
            service => service.SendEmail(user.Email, "Welcome", $"Welcome {user.Name}!"),
            Times.Once);
    }
    
    [Fact]
    public void Mock_StrictBehavior()
    {
        var strictMock = new Mock<IUserRepository>(MockBehavior.Strict);
        strictMock.Setup(repo => repo.GetById(1)).Returns(new User { Id = 1, Name = "User" });
        
        var result = strictMock.Object.GetById(1);
        Assert.NotNull(result);
    }
}