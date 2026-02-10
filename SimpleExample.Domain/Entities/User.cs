namespace SimpleExample.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    public User()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
    }

    public User(string firstName, string lastName, string email)
    {
        UpdateBasicInfo(firstName, lastName);
        UpdateEmail(email);
    }

    public void UpdateBasicInfo(string firstName, string lastName)
    {
        ArgumentNullException.ThrowIfNull(firstName);
        ArgumentNullException.ThrowIfNull(lastName);

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Etunimi ei voi olla tyhja.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Sukunimi ei voi olla tyhja.", nameof(lastName));

        if (firstName.Length < 3)
            throw new ArgumentException("Etunimen tulee olla vahintaan 3 merkkia pitka.", nameof(firstName));

        if (lastName.Length < 3)
            throw new ArgumentException("Sukunimen tulee olla vahintaan 3 merkkia pitka.", nameof(lastName));

        if (firstName.Length > 100)
            throw new ArgumentException("Etunimi voi olla enintaan 100 merkkia pitka.", nameof(firstName));

        if (lastName.Length > 100)
            throw new ArgumentException("Sukunimi voi olla enintaan 100 merkkia pitka.", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
    }

    public void UpdateEmail(string email)
    {
        ArgumentNullException.ThrowIfNull(email);

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Sahkoposti ei voi olla tyhja.", nameof(email));

        if (!email.Contains('@'))
            throw new ArgumentException("Sahkopostin tulee olla kelvollinen.", nameof(email));

        if (email.Length > 255)
            throw new ArgumentException("Sahkoposti voi olla enintaan 255 merkkia pitka.", nameof(email));

        Email = email;
    }
}


