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

public class DeleteCandidateHandlerTests
{
    [Fact]
    public async Task Should_Delete_Candidate_And_SaveChanges()
    {
        var repo = new Mock<ICandidateRepository>();
        var timelineRepo = new Mock<ITimelineRepository>();
        var uow = new Mock<IUnitOfWork>();

        var candidate = Candidate.Create("Ana", "Silva", new DateTime(1990, 1, 1), "ana@ex.com");

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(candidate);

        repo.Setup(r => r.Remove(It.IsAny<Candidate>()));

        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new DeleteCandidateHandler(repo.Object, timelineRepo.Object, uow.Object);

        var command = new DeleteCandidateCommand(candidate.IdCandidate);

        await handler.Handle(command, default);

        repo.Verify(r => r.GetByIdAsync(candidate.IdCandidate, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.Remove(candidate), Times.Once);
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
