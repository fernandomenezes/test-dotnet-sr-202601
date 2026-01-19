using ApplicantTracking.Application.DTOs;
using ApplicantTracking.Domain.Interfaces;
using ApplicantTracking.Application.Commands;
using ApplicantTracking.Application.Commands.Handlers;
using ApplicantTracking.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

public class CreateCandidateHandlerTests
{
    [Fact]
    public async Task Should_Create_Candidate_And_Timeline()
    {
        var repo = new Mock<ICandidateRepository>();
        var timeline = new Mock<ITimelineRepository>();
        var uow = new Mock<IUnitOfWork>();

        repo.Setup(r => r.AddAsync(It.IsAny<Candidate>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        timeline.Setup(t => t.AddAsync(It.IsAny<ApplicantTracking.Domain.Entities.Timeline>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CreateCandidateHandler(repo.Object, timeline.Object, uow.Object);

        var dto = await handler.Handle(new CreateCandidateCommand("Ana","Silva", new DateTime(1990,1,1), "ana@ex.com"), default);

        dto.Name.Should().Be("Ana");
        timeline.Verify(t => t.AddAsync(It.IsAny<ApplicantTracking.Domain.Entities.Timeline>(), It.IsAny<CancellationToken>()), Times.Once);
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
