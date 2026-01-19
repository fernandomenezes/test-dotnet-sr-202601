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

public class DeleteCandidateWithTimelineHandlerTests
{
    [Fact]
    public async Task Should_Delete_Candidate_And_Add_Timeline()
    {
        var repo = new Mock<ICandidateRepository>();
        var timelineRepo = new Mock<ITimelineRepository>();
        var uow = new Mock<IUnitOfWork>();

        var candidate = Candidate.Create("Ana", "Silva", new DateTime(1990, 1, 1), "ana@ex.com");

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(candidate);

        repo.Setup(r => r.Remove(It.IsAny<Candidate>()));

        timelineRepo.Setup(t => t.AddAsync(It.IsAny<Timeline>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new DeleteCandidateHandler(repo.Object, timelineRepo.Object, uow.Object);

        var command = new DeleteCandidateCommand(candidate.IdCandidate);

        await handler.Handle(command, default);

        repo.Verify(r => r.GetByIdAsync(candidate.IdCandidate, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.Remove(candidate), Times.Once);
        timelineRepo.Verify(t => t.AddAsync(
            It.Is<Timeline>(tl =>
                tl.IdAggregateRoot == candidate.IdCandidate &&
                tl.IdTimelineType > 0 &&
                tl.OldData != null
            ),
            It.IsAny<CancellationToken>()),
            Times.Once);
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
