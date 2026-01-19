using ApplicantTracking.Application.Commands;
using ApplicantTracking.Domain.Interfaces;
using ApplicantTracking.Domain.Enumerators;
using ApplicantTracking.Domain.Entities;
using MediatR;
using System.Text.Json;

namespace ApplicantTracking.Application.Commands.Handlers;

public class DeleteCandidateHandler : IRequestHandler<DeleteCandidateCommand>
{
    private readonly ICandidateRepository _repo;
    private readonly ITimelineRepository _timeline;
    private readonly IUnitOfWork _uow;

    public DeleteCandidateHandler(ICandidateRepository repo, ITimelineRepository timeline, IUnitOfWork uow)
        => (_repo, _timeline, _uow) = (repo, timeline, uow);

    public async Task Handle(DeleteCandidateCommand cmd, CancellationToken ct)
    {
        var candidate = await _repo.GetByIdAsync(cmd.IdCandidate, ct)
                        ?? throw new KeyNotFoundException("Candidate not found");

        var oldJson = JsonSerializer.Serialize(candidate);
        _repo.Remove(candidate);

        await _timeline.AddAsync(
            Timeline.Create((byte)TimelineTypes.CandidateDeleted, candidate.IdCandidate, oldJson, null),
            ct
        );

        await _uow.SaveChangesAsync(ct);
    }
}
