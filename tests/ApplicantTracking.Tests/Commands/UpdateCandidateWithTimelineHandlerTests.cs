using ApplicantTracking.Domain.Entities;
using ApplicantTracking.Domain.Interfaces;
using ApplicantTracking.Application.Commands;
using ApplicantTracking.Application.Commands.Handlers;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

public class UpdateCandidateWithTimelineHandlerTests
{
    [Fact]
    public async Task Should_Update_Candidate_And_Add_Timeline()
    {
        var repo = new Mock<ICandidateRepository>();
        var timelineRepo = new Mock<ITimelineRepository>();
        var uow = new Mock<IUnitOfWork>();

        var candidate = Candidate.Create("Ana", "Silva", new DateTime(1990, 1, 1), "ana@ex.com");

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(candidate);

        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new UpdateCandidateHandler(repo.Object, timelineRepo.Object, uow.Object);

        var command = new UpdateCandidateCommand(candidate.IdCandidate, "Ana Maria", "Silva", new DateTime(1990, 1, 1), "ana.maria@ex.com");

        var dto = await handler.Handle(command, default);

        dto.Name.Should().Be("Ana Maria");

        timelineRepo.Verify(t => t.AddAsync(
            It.Is<Timeline>(tl =>
                tl.IdAggregateRoot == candidate.IdCandidate &&
                tl.IdTimelineType > 0 && // tipo definido pelo handler
                tl.NewData != null &&
                tl.CreatedAt <= DateTime.UtcNow
            ),
            It.IsAny<CancellationToken>()),
            Times.Once);

        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
