using ApplicantTracking.Application.DTOs;
using ApplicantTracking.Domain.Entities;
using ApplicantTracking.Domain.Enums;
using ApplicantTracking.Domain.Events;
using ApplicantTracking.Domain.Interfaces;
using MediatR;
using System.Text.Json;

public class CreateCandidateHandler : IRequestHandler<CreateCandidateCommand, CandidateDto>
{
    private readonly ICandidateRepository _repo;
    private readonly ITimelineRepository _timeline;
    private readonly IUnitOfWork _uow;

    public CreateCandidateHandler(ICandidateRepository repo, ITimelineRepository timeline, IUnitOfWork uow)
        => (_repo, _timeline, _uow) = (repo, timeline, uow);

    public async Task<CandidateDto> Handle(CreateCandidateCommand cmd, CancellationToken ct)
    {
        var entity = Candidate.Create(cmd.Name, cmd.Surname, cmd.Birthdate, cmd.Email);
        await _repo.AddAsync(entity, ct);

        // Evento → Timeline
        var evt = new CandidateChangedEvent(TimelineTypes.CandidateCreated, entity.IdCandidate, null, entity);
        var newJson = JsonSerializer.Serialize(entity);
        await _timeline.AddAsync(Timeline.Create((byte)evt.Type, evt.CandidateId, null, newJson), ct);

        await _uow.SaveChangesAsync(ct);

        return new CandidateDto(entity.IdCandidate, entity.Name, entity.Surname, entity.Birthdate, entity.Email);
    }
}
