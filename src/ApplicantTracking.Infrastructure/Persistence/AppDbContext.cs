// Infrastructure/Persistence/AppDbContext.cs
using ApplicantTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicantTracking.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<Timeline> Timelines => Set<Timeline>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Candidate>(e =>
        {
            e.ToTable("candidates");
            e.HasKey(x => x.IdCandidate);
            e.Property(x => x.Name).HasMaxLength(80).IsRequired();
            e.Property(x => x.Surname).HasMaxLength(150).IsRequired();
            e.Property(x => x.Email).HasMaxLength(250).IsRequired();
            e.Property(x => x.CreatedAt).IsRequired();
            e.Property(x => x.LastUpdatedAt);
        });

        b.Entity<Timeline>(e =>
        {
            e.ToTable("timelines");
            e.HasKey(x => x.IdTimeline);
            e.Property(x => x.IdTimelineType).IsRequired();
            e.Property(x => x.IdAggregateRoot).IsRequired();
            e.Property(x => x.OldData);
            e.Property(x => x.NewData);
            e.Property(x => x.CreatedAt).IsRequired();
        });
    }
}
