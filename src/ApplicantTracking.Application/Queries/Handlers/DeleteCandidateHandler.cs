using ApplicantTracking.Domain.Enums;
using ApplicantTracking.Domain.Events;
using ApplicantTracking.Domain.Interfaces;
using MediatR;
using System.Text.Json;

public class DeleteCandidateHandler : IRequestHandler<DeleteCandidateCommand>
{
    private readonly ICandidateRepository _repo;
    private readonly ITimelineRepository _timeline;
    private readonly IUnitOfWork _uow;

    public DeleteCandidateHandler(ICandidateRepository repo, ITimelineRepository timeline, IUnitOfWork uow)
        => (_repo, _timeline, _uow) = (repo, timeline, uow);

    public async Task<Unit> Handle(DeleteCandidateCommand cmd, CancellationToken ct)
    {
        var current = await _repo.GetByIdAsync(cmd.IdCandidate, ct)
                     ?? throw new KeyNotFoundException("Candidato não localizado");

        var oldJson = JsonSerializer.Serialize(current);
        _repo.Remove(current);

        var evt = new CandidateChangedEvent(TimelineTypes.CandidateDeleted, current.IdCandidate, current, null);
        await _timeline.AddAsync(Timeline.Create((byte)evt.Type, evt.CandidateId, oldJson, null), ct);

        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
