using Domain.Common;
using Domain.Organizers;
using Domain.Organizers.Exceptions;
using Application.Features.Organizers.Commands;
using MassTransit;

namespace Application.UnitTests.Features.Organizers.Commands;
public class CreateTournamentCommandTests
{
    [Fact]
    public async Task ShouldThrowAOrganizerNotFoundException_WhenOrganizerNotFound()
    {
        var (createTournamentCommandHandler, createTournamentCommand, unitOfWorkMock, organizerRepoMock, tournamentRepoMock, busMock) = GetHandlerAndMocks(HandlerCallOption.NullOrganizer);

        Func<Task<Guid?>> handlerFunc = () => createTournamentCommandHandler.Handle(createTournamentCommand, default);

        await handlerFunc.Should().ThrowAsync<OrganizerNotFoundException>();
        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        tournamentRepoMock.Verify(m => m.CreateAsync(It.IsAny<Tournament>(), It.IsAny<CancellationToken>()), Times.Never);
        busMock.Verify(m => m.Publish(It.IsAny<TournamentCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ShouldCreateATournamnet_WhenOrganizerIsInValidState()
    {
        var (createTournamentCommandHandler, createTournamentCommand, unitOfWorkMock, organizerRepoMock, tournamentRepoMock, busMock) = GetHandlerAndMocks(HandlerCallOption.Valid);

        Guid? tournamentIdCreated = await createTournamentCommandHandler.Handle(createTournamentCommand, default);

        tournamentIdCreated.Should().NotBeEmpty();
        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        tournamentRepoMock.Verify(m => m.CreateAsync(It.IsAny<Tournament>(), It.IsAny<CancellationToken>()), Times.Once);
        busMock.Verify(m => m.Publish(It.IsAny<TournamentCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    static (
        CreateTournamentCommandHandler createTournamentCommandHandler,
        CreateTournamentCommand createTournamentCommand,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IOrganizerRepository> organizerRepoMock,
        Mock<ITournamentRepository> tournamentRepoMock,
        Mock<IBus> busMock
    )
    GetHandlerAndMocks(HandlerCallOption option)
    {
        Organizer? organizer = option switch{
            HandlerCallOption.NullOrganizer => null,
            HandlerCallOption.Valid => Organizer.Create(new OrganizerPersonalInfo("", "", "", DateTime.Today, "", "", "", "", "")),
            _ => throw new NotImplementedException()
        };
        var busMock = new Mock<IBus>();
        var tournamentRepoMock = new Mock<ITournamentRepository>();
        var unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        unitOfWorkFactoryMock.Setup(uowf => uowf.CreateUnitOfWork(It.IsAny<string>())).Returns(unitOfWorkMock.Object);
        var organizerRepoMock = new Mock<IOrganizerRepository>();
        organizerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(organizer!);
        unitOfWorkMock.Setup(m => m.Tournaments).Returns(tournamentRepoMock.Object);
        unitOfWorkMock.Setup(m => m.Organizers).Returns(organizerRepoMock.Object);
        var createTournamentCommand = new CreateTournamentCommand()
        {
            OrganizerId = Guid.NewGuid(),
            TournamentName = "test"
        };
        var createTournamentCommandHandler = new CreateTournamentCommandHandler(new Mock<ILoggerManager>().Object, unitOfWorkFactoryMock.Object, busMock.Object);

        return(
            createTournamentCommandHandler,
            createTournamentCommand,
            unitOfWorkMock,
            organizerRepoMock,
            tournamentRepoMock,
            busMock
        );
    }

    enum HandlerCallOption
    {
        NullOrganizer,
        Valid
    }
}