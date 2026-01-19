using ApplicantTracking.Domain.Entities;

namespace ApplicantTracking.Domain.Interfaces;

public interface ICandidateRepository
{
    Task<List<Candidate>> GetAllAsync(CancellationToken ct);
    Task<Candidate?> GetByIdAsync(int id, CancellationToken ct);
    Task AddAsync(Candidate entity, CancellationToken ct);
    void Update(Candidate entity);
    void Remove(Candidate entity);
}
