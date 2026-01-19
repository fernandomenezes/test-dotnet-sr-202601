// Infrastructure/Persistence/UnitOfWork.cs
using ApplicantTracking.Domain.Interfaces;

namespace ApplicantTracking.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _ctx;
    public UnitOfWork(AppDbContext ctx) => _ctx = ctx;
    public Task<int> SaveChangesAsync(CancellationToken ct) => _ctx.SaveChangesAsync(ct);
}
