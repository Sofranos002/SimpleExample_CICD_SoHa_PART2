using FluentAssertions;
using SimpleExample.Domain.Entities;

namespace SimpleExample.Tests.Domain;

public class UserTests
{
    [Fact]
    public void Creating_User_With_Valid_Data_Should_Succeed()
    {
        string firstName = "Matti";
        string lastName = "Meikäläinen";
        string email = "matti@example.com";

        User user = new User(firstName, lastName, email);

        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Email.Should().Be(email);
    }

    [Fact]
    public void Creating_User_With_Too_Short_FirstName_Should_Throw()
    {
        Action act = () => new User("Ma", "Virtanen", "maija@example.com");

        act.Should().Throw<ArgumentException>()
            .WithMessage("*");
    }

    [Fact]
    public void Updating_Email_To_Invalid_Should_Throw()
    {
        User user = new User("Teppo", "Testaaja", "teppo@example.com");

        Action act = () => user.UpdateEmail("invalid-email");

        act.Should().Throw<ArgumentException>()
            .WithMessage("*");
    }

    [Fact]
    public void Creating_User_With_Too_Short_LastName_Should_Throw()
    {
        Action act = () => new User("Maija", "V", "maija@example.com");

        act.Should().Throw<ArgumentException>()
            .WithMessage("*");
    }

    [Fact]
    public void Creating_User_With_Empty_LastName_Should_Throw()
    {
        Action act = () => new User("Maija", "", "maija@example.com");

        act.Should().Throw<ArgumentException>()
            .WithMessage("*");
    }

    [Fact]
    public void Creating_User_With_Too_Long_LastName_Should_Throw()
    {
        string longName = new string('A', 101);

        Action act = () => new User("Maija", longName, "maija@example.com");

        act.Should().Throw<ArgumentException>()
            .WithMessage("*");
    }

    [Fact]
    public void Creating_User_With_Empty_FirstName_Should_Throw()
    {
        Action act = () => new User("", "Virtanen", "maija@example.com");

        act.Should().Throw<ArgumentException>()
            .WithMessage("*");
    }

    [Fact]
    public void Creating_User_With_Too_Long_FirstName_Should_Throw()
    {
        string longName = new string('A', 101);

        Action act = () => new User(longName, "Virtanen", "maija@example.com");

        act.Should().Throw<ArgumentException>()
            .WithMessage("*");
    }

    [Fact]
    public void Creating_User_With_Empty_Email_Should_Throw()
    {
        Action act = () => new User("Maija", "Virtanen", "");

        act.Should().Throw<ArgumentException>()
            .WithMessage("*");
    }

    [Fact]
    public void Creating_User_With_Too_Long_Email_Should_Throw()
    {
        string longEmail = new string('a', 256) + "@example.com";

        Action act = () => new User("Maija", "Virtanen", longEmail);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*");
    }

    [Fact]
    public void Creating_User_With_Email_Without_At_Should_Throw()
    {
        Action act = () => new User("Maija", "Virtanen", "invalidemail");

        act.Should().Throw<ArgumentException>()
            .WithMessage("*");
    }

    [Fact]
    public void Creating_User_With_Null_Email_Should_Throw()
    {
        Action act = () => new User("Maija", "Virtanen", null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void UpdateBasicInfo_With_Valid_Data_Should_Update_Names()
    {
        User user = new User("Old", "Name", "old@example.com");

        user.UpdateBasicInfo("New", "Person");

        user.FirstName.Should().Be("New");
        user.LastName.Should().Be("Person");
    }

    [Fact]
    public void UpdateBasicInfo_With_Too_Long_FirstName_Should_Throw()
    {
        User user = new User("Old", "Name", "old@example.com");
        string longName = new string('A', 101);

        Action act = () => user.UpdateBasicInfo(longName, "Valid");

        act.Should().Throw<ArgumentException>()
            .WithMessage("*");
    }

    [Fact]
    public void UpdateBasicInfo_With_Too_Long_LastName_Should_Throw()
    {
        User user = new User("Old", "Name", "old@example.com");
        string longName = new string('A', 101);

        Action act = () => user.UpdateBasicInfo("Valid", longName);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*");
    }
}


