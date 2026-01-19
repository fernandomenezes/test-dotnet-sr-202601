using ApplicantTracking.Domain.Entities;
using ApplicantTracking.Domain.Interfaces;
using ApplicantTracking.Infrastructure.Persistence;

namespace ApplicantTracking.Infrastructure.Repositories;

public class TimelineRepository : ITimelineRepository
{
    private readonly AppDbContext _ctx;
    public TimelineRepository(AppDbContext ctx) => _ctx = ctx;

    public Task AddAsync(Timeline entity, CancellationToken ct)
        => _ctx.Timelines.AddAsync(entity, ct).AsTask();
}
