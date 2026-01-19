using MediatR;
using ApplicantTracking.Application.DTOs;

public record CreateCandidateCommand(string Name, string Surname, DateTime Birthdate, string Email)
    : IRequest<CandidateDto>;
