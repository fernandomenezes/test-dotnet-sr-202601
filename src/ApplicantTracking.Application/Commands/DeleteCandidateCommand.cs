using MediatR;

public record DeleteCandidateCommand(int IdCandidate) : IRequest;
