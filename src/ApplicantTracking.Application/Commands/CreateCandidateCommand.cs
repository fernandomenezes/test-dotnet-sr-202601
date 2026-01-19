using MediatR;
using ApplicantTracking.Application.DTOs;

namespace ApplicantTracking.Application.Commands;

public record CreateCandidateCommand(string Name, string Surname, DateTime Birthdate, string Email)
    : IRequest<CandidateDto>;
