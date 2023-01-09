using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class StartMoveCommandTest
{
    public StartMoveCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        Mock<ICommand> commandMock = new Mock<ICommand>();
        commandMock.Setup(c => c.Execute());

        Mock<IStrategy> strategyMock = new Mock<IStrategy>();
        strategyMock.Setup(s => s.Strategy(It.IsAny<object[]>())).Returns(commandMock.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.SetProperty", (object[] args) => strategyMock.Object.Strategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Command.Movement", (object[] args) => strategyMock.Object.Strategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.QueuePush", (object[] args) => strategyMock.Object.Strategy(args)).Execute();


    }

    [Fact]
    public void TestPositiveStartMoveCommand()
    {
        Mock<IMoveCommandStartable> mcsMock = new Mock<IMoveCommandStartable>();
        mcsMock.SetupGet(c => c.uobj).Returns(new Mock<IUObject>().Object).Verifiable();
        mcsMock.SetupGet(c => c.property).Returns(new Dictionary<string, object>() { { "Velocity", new Vector(It.IsAny<int>(), It.IsAny<int>()) } }).Verifiable();

        ICommand command = new StartMoveCommand(mcsMock.Object);
        command.Execute();

        mcsMock.Verify();
    }

    [Fact]
    public void TestNegativeStartMoveCommand1()
    {
        Mock<IMoveCommandStartable> mcsMock = new Mock<IMoveCommandStartable>();
        mcsMock.SetupGet(c => c.uobj).Throws<Exception>().Verifiable();
        mcsMock.SetupGet(c => c.property).Returns(new Dictionary<string, object>() { { "Velocity", new Vector(It.IsAny<int>(), It.IsAny<int>()) } }).Verifiable();

        ICommand command = new StartMoveCommand(mcsMock.Object);

        Assert.Throws<Exception>(() => command.Execute());
    }

    [Fact]
    public void TestNegativeStartMoveCommand2()
    {
        Mock<IMoveCommandStartable> mcsMock = new Mock<IMoveCommandStartable>();
        mcsMock.SetupGet(c => c.uobj).Returns(new Mock<IUObject>().Object).Verifiable();
        mcsMock.SetupGet(c => c.property).Throws<Exception>().Verifiable();

        ICommand command = new StartMoveCommand(mcsMock.Object);

        Assert.Throws<Exception>(() => command.Execute());
    }
}
