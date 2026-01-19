// Infrastructure/Repositories/CandidateRepository.cs
using ApplicantTracking.Domain.Entities;
using ApplicantTracking.Domain.Interfaces;
using ApplicantTracking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApplicantTracking.Infrastructure.Repositories;

public class CandidateRepository : ICandidateRepository
{
    private readonly AppDbContext _ctx;
    public CandidateRepository(AppDbContext ctx) => _ctx = ctx;

    public Task<List<Candidate>> GetAllAsync(CancellationToken ct)
        => _ctx.Candidates.AsNoTracking().ToListAsync(ct);

    public Task<Candidate?> GetByIdAsync(int id, CancellationToken ct)
        => _ctx.Candidates.FirstOrDefaultAsync(x => x.IdCandidate == id, ct);

    public Task AddAsync(Candidate entity, CancellationToken ct)
        => _ctx.Candidates.AddAsync(entity, ct).AsTask();

    public void Update(Candidate entity) => _ctx.Candidates.Update(entity);
    public void Remove(Candidate entity) => _ctx.Candidates.Remove(entity);
}
