using ApplicantTracking.Application.Commands;
using ApplicantTracking.Application.DTOs;
using ApplicantTracking.Domain.Entities;
using ApplicantTracking.Domain.Enumerators;
using ApplicantTracking.Domain.Interfaces;
using MediatR;
using System.Text.Json;

namespace ApplicantTracking.Application.Commands.Handlers;

public class CreateCandidateHandler : IRequestHandler<CreateCandidateCommand, CandidateDto>
{
    private readonly ICandidateRepository _repo;
    private readonly ITimelineRepository _timeline;
    private readonly IUnitOfWork _uow;

    public CreateCandidateHandler(ICandidateRepository repo, ITimelineRepository timeline, IUnitOfWork uow)
        => (_repo, _timeline, _uow) = (repo, timeline, uow);

    public async Task<CandidateDto> Handle(CreateCandidateCommand cmd, CancellationToken ct)
    {
        var candidate = Candidate.Create(cmd.Name, cmd.Surname, cmd.Birthdate, cmd.Email);
        await _repo.AddAsync(candidate, ct);

        var json = JsonSerializer.Serialize(candidate);
        await _timeline.AddAsync(
            Timeline.Create((byte)TimelineTypes.CandidateCreated, candidate.IdCandidate, null, json),
            ct
        );

        await _uow.SaveChangesAsync(ct);

        return new CandidateDto(candidate.IdCandidate, candidate.Name, candidate.Surname, candidate.Birthdate, candidate.Email);
    }
}
