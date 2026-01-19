// Domain/Entities/Candidate.cs
namespace ApplicantTracking.Domain.Entities;

public class Candidate
{
    public int IdCandidate { get; private set; }
    public string Name { get; private set; } = default!;
    public string Surname { get; private set; } = default!;
    public DateTime Birthdate { get; private set; }
    public string Email { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastUpdatedAt { get; private set; }

    // Fábricas e métodos de negócio
    public static Candidate Create(string name, string surname, DateTime birthdate, string email)
    {
        return new Candidate
        {
            Name = name.Trim(),
            Surname = surname.Trim(),
            Birthdate = birthdate,
            Email = email.Trim(),
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string name, string surname, DateTime birthdate, string email)
    {
        Name = name.Trim();
        Surname = surname.Trim();
        Birthdate = birthdate;
        Email = email.Trim();
        LastUpdatedAt = DateTime.UtcNow;
    }
}
