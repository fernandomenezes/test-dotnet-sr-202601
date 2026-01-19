using ApplicantTracking.Application.DTOs;
using ApplicantTracking.Domain.Entities;
using ApplicantTracking.Domain.Enums;
using ApplicantTracking.Domain.Events;
using ApplicantTracking.Domain.Interfaces;
using MediatR;
using System.Text.Json;

public class UpdateCandidateHandler : IRequestHandler<UpdateCandidateCommand, CandidateDto>
{
    private readonly ICandidateRepository _repo;
    private readonly ITimelineRepository _timeline;
    private readonly IUnitOfWork _uow;

    public UpdateCandidateHandler(ICandidateRepository repo, ITimelineRepository timeline, IUnitOfWork uow)
        => (_repo, _timeline, _uow) = (repo, timeline, uow);

    public async Task<CandidateDto> Handle(UpdateCandidateCommand cmd, CancellationToken ct)
    {
        var current = await _repo.GetByIdAsync(cmd.IdCandidate, ct)
                     ?? throw new KeyNotFoundException("Candidato não localizado");

        var oldJson = JsonSerializer.Serialize(current);
        current.Update(cmd.Name, cmd.Surname, cmd.Birthdate, cmd.Email);
        _repo.Update(current);

        var evt = new CandidateChangedEvent(TimelineTypes.CandidateUpdated, current.IdCandidate, null, current);
        var newJson = JsonSerializer.Serialize(current);
        await _timeline.AddAsync(Timeline.Create((byte)evt.Type, evt.CandidateId, oldJson, newJson), ct);

        await _uow.SaveChangesAsync(ct);

        return new CandidateDto(current.IdCandidate, current.Name, current.Surname, current.Birthdate, current.Email);
    }
}
