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
            throw new ArgumentException("Etunimi ei voi olla tyhjä.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Sukunimi ei voi olla tyhjä.", nameof(lastName));

        if (firstName.Length < 3)
            throw new ArgumentException("Etunimen tulee olla vähintään 3 merkkiä pitkä.", nameof(firstName));

        if (lastName.Length < 3)
            throw new ArgumentException("Sukunimen tulee olla vähintään 3 merkkiä pitkä.", nameof(lastName));

        if (firstName.Length > 100)
            throw new ArgumentException("Etunimi voi olla enintään 100 merkkiä pitkä.", nameof(firstName));

        if (lastName.Length > 100)
            throw new ArgumentException("Sukunimi voi olla enintään 100 merkkiä pitkä.", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
    }

    public void UpdateEmail(string email)
    {
        ArgumentNullException.ThrowIfNull(email);

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Sähköposti ei voi olla tyhjä.", nameof(email));

        if (!email.Contains('@'))
            throw new ArgumentException("Sähköpostin tulee olla kelvollinen.", nameof(email));

        if (email.Length > 255)
            throw new ArgumentException("Sähköposti voi olla enintään 255 merkkiä pitkä.", nameof(email));

        Email = email;
    }
}

