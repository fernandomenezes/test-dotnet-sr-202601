// Domain/Interfaces/ITimelineRepository.cs
using ApplicantTracking.Domain.Entities;

namespace ApplicantTracking.Domain.Interfaces;

public interface ITimelineRepository
{
    Task AddAsync(Timeline entity, CancellationToken ct);
}
