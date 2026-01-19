using ApplicantTracking.Application.DTOs;
using MediatR;

namespace ApplicantTracking.Application.Queries;

public record GetCandidatesQuery() : IRequest<List<CandidateDto>>;
