using Domain.Common;
using Domain.Managers;
using Domain.Organizers;
using Domain.Organizers.Exceptions;
using Application.Features.Organizers.Commands;
using MassTransit;

namespace Application.UnitTests.Features.Organizers.Commands;
public class FinishTournamentCommandTests
{
    [Fact]
    public async Task ShouldThrowAOrganizerNotFoundException_WhenOrganizerNotFound()
    {
        var (finishTournamentCommandHandler, finishTournamentCommand, unitOfWorkMock, organizerRepoMock, busMock) = GetHandlerAndMocks(HandlerCallOption.NullOrganizer);

        Func<Task> handlerFunc = () => finishTournamentCommandHandler.Handle(finishTournamentCommand, default);

        await handlerFunc.Should().ThrowAsync<OrganizerNotFoundException>();
        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        busMock.Verify(m => m.Publish(It.IsAny<TournamentFinishedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ShouldFinishTheTournament_WhenOrganizerIsInValidState()
    {
        var (finishTournamentCommandHandler, finishTournamentCommand, unitOfWorkMock, organizerRepoMock, busMock) = GetHandlerAndMocks(HandlerCallOption.Valid);

        await finishTournamentCommandHandler.Handle(finishTournamentCommand, default);

        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        busMock.Verify(m => m.Publish(It.IsAny<TournamentFinishedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    static(
        FinishTournamentCommandHandler finishTournamentCommandHandler,
        FinishTournamentCommand finishTournamentCommand,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IOrganizerRepository> organizerRepoMock,
        Mock<IBus> busMock
    )
    GetHandlerAndMocks(HandlerCallOption option)
    {
        Organizer? organizer = option switch
        {
            HandlerCallOption.NullOrganizer => null,
            HandlerCallOption.Valid => GetOrganizerWithTournamentAndTeams(),
            _ => throw new NotImplementedException()
        };
        var busMock = new Mock<IBus>();
        var unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        unitOfWorkFactoryMock.Setup(uowf => uowf.CreateUnitOfWork(It.IsAny<string>())).Returns(unitOfWorkMock.Object);
        var organizerRepoMock = new Mock<IOrganizerRepository>();
        organizerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(organizer!);
        unitOfWorkMock.Setup(m => m.Organizers).Returns(organizerRepoMock.Object);
        var finishTournamentCommand = new FinishTournamentCommand() { OrganizerId = Guid.NewGuid() };
        var finishTournamentCommandHandler = new FinishTournamentCommandHandler(unitOfWorkFactoryMock.Object, new Mock<ILoggerManager>().Object, busMock.Object);

        return(
            finishTournamentCommandHandler,
            finishTournamentCommand,
            unitOfWorkMock,
            organizerRepoMock,
            busMock
        );
    }

    enum HandlerCallOption
    {
        NullOrganizer,
        Valid
    }

    static Organizer GetOrganizerWithTournamentAndTeams()
    {
        var t1 = Team.Create("test", Manager.Create(new("test1", "test", "test", DateTime.Now, "", "", "", "", "")));
        var t2 = Team.Create("test", Manager.Create(new("test2", "test", "test", DateTime.Now, "", "", "", "", "")));
        var organizer = Organizer.Create(new("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", ""));
        organizer.CreateTournament("test tournament");
        organizer.RegisterTeam(t1);
        organizer.RegisterTeam(t2);
        return organizer;
    }
}