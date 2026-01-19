using ApplicantTracking.Application.DTOs;
using ApplicantTracking.Application.Queries;
using ApplicantTracking.Domain.Interfaces;
using MediatR;

public class GetCandidatesHandler : IRequestHandler<GetCandidatesQuery, List<CandidateDto>>
{
    private readonly ICandidateRepository _repo;
    public GetCandidatesHandler(ICandidateRepository repo) => _repo = repo;

    public async Task<List<CandidateDto>> Handle(GetCandidatesQuery request, CancellationToken ct)
    {
        var list = await _repo.GetAllAsync(ct);
        return list.Select(x => new CandidateDto(x.IdCandidate, x.Name, x.Surname, x.Birthdate, x.Email)).ToList();
    }
}
