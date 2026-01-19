// Domain/Interfaces/IUnitOfWork.cs
namespace ApplicantTracking.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct);
}
