using FluentAssertions;
using Moq;
using SimpleExample.Application.DTOs;
using SimpleExample.Application.Interfaces;
using SimpleExample.Application.Services;
using SimpleExample.Domain.Entities;
using Xunit;

namespace SimpleExample.Tests.Application;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _service = new UserService(_mockRepository.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateUser()
    {
        // Arrange
        CreateUserDto dto = new CreateUserDto
        {
            FirstName = "Matti",
            LastName = "Meikäläinen",
            Email = "matti@example.com"
        };

        _mockRepository
            .Setup(x => x.GetByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);

        _mockRepository
            .Setup(x => x.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);

        // Act
        UserDto result = await _service.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be("Matti");
        result.LastName.Should().Be("Meikäläinen");
        result.Email.Should().Be("matti@example.com");

        _mockRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateEmail_ShouldThrowInvalidOperationException()
    {
        // Arrange
        CreateUserDto dto = new CreateUserDto
        {
            FirstName = "Matti",
            LastName = "Meikäläinen",
            Email = "existing@example.com"
        };

        User existingUser = new User("Maija", "Virtanen", "existing@example.com");

        _mockRepository
            .Setup(x => x.GetByEmailAsync(dto.Email))
            .ReturnsAsync(existingUser);

        // Act
        Func<Task> act = async () => await _service.CreateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*jo olemassa*");

        _mockRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
    }

    // 1. GetByIdAsync - löytyy
    [Fact]
    public async Task GetByIdAsync_Should_Return_User_When_Found()
    {
        Guid id = Guid.NewGuid();
        User user = new User("Anna", "Berg", "a@b.com");
        user.Id = id;

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);

        UserDto? result = await _service.GetByIdAsync(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(user.Id);
    }

    // 2. GetByIdAsync - ei löydy
    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
    {
        Guid id = Guid.NewGuid();

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((User?)null);

        UserDto? result = await _service.GetByIdAsync(id);

        result.Should().BeNull();
    }

    // 3. GetAllAsync - palauttaa listan
    [Fact]
    public async Task GetAllAsync_Should_Return_List_Of_Users()
    {
        List<User> list = new()
        {
            new User("Anna", "Berg", "a@b.com"),
            new User("Cecilia", "Dahl", "c@d.com")
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

        IEnumerable<UserDto> result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
    }

    // 4. UpdateAsync - onnistuu
    [Fact]
    public async Task UpdateAsync_Should_Update_User_When_Found()
    {
        Guid id = Guid.NewGuid();
        User user = new User("Old", "Name", "old@example.com");

        // Aseta Id heijastuksella
        typeof(BaseEntity)
            .GetProperty("Id")!
            .SetValue(user, id);

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);

        UpdateUserDto dto = new UpdateUserDto
        {
            FirstName = "New",
            LastName = "Name",
            Email = "new@example.com"
        };

        UserDto? result = await _service.UpdateAsync(id, dto);

        result.Should().NotBeNull();
        result!.FirstName.Should().Be("New");
        result.Email.Should().Be("new@example.com");
    }


    // 5. UpdateAsync - käyttäjää ei löydy
    [Fact]
    public async Task UpdateAsync_Should_Return_Null_When_User_Not_Found()
    {
        Guid id = Guid.NewGuid();

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((User?)null);

        UpdateUserDto dto = new UpdateUserDto
        {
            FirstName = "X",
            LastName = "Y",
            Email = "x@y.com"
        };

        UserDto? result = await _service.UpdateAsync(id, dto);

        result.Should().BeNull();
    }

    // 6. DeleteAsync - onnistuu
    [Fact]
    public async Task DeleteAsync_Should_Return_True_When_User_Deleted()
    {
        Guid id = Guid.NewGuid();
        User user = new User("Anna", "Berg", "a@b.com");

        // Aseta Id heijastuksella
        typeof(BaseEntity)
            .GetProperty("Id")!
            .SetValue(user, id);

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);
        _mockRepository.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

        bool result = await _service.DeleteAsync(id);

        result.Should().BeTrue();
    }

    // 7. DeleteAsync - käyttäjää ei löydy
    [Fact]
    public async Task DeleteAsync_Should_Return_False_When_User_Not_Found()
    {
        Guid id = Guid.NewGuid();

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((User?)null);

        bool result = await _service.DeleteAsync(id);

        result.Should().BeFalse();
    }
}



