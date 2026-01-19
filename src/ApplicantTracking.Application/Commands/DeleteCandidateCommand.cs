using MediatR;

namespace ApplicantTracking.Application.Commands;

public record DeleteCandidateCommand(int IdCandidate) : IRequest;
