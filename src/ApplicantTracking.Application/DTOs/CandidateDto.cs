namespace ApplicantTracking.Application.DTOs;

public record CandidateDto(int IdCandidate, string Name, string Surname, DateTime Birthdate, string Email);
