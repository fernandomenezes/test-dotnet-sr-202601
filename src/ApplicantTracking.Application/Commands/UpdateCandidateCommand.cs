using MediatR;
using ApplicantTracking.Application.DTOs;

public record UpdateCandidateCommand(int IdCandidate, string Name, string Surname, DateTime Birthdate, string Email)
    : IRequest<CandidateDto>;
