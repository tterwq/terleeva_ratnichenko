using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class EndMoveCommandeTest
{
    public EndMoveCommandeTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        Mock<SpaceBattle.Lib.ICommand> mockCommand = new Mock<SpaceBattle.Lib.ICommand>();
        mockCommand.Setup(x => x.Execute());

        Mock<IInjectable> mockInjecting = new Mock<IInjectable>();
        mockInjecting.Setup(x => x.Inject(It.IsAny<SpaceBattle.Lib.ICommand>()));

        Mock<IStrategy> mockStrategyReturnCommand = new Mock<IStrategy>();
        mockStrategyReturnCommand.Setup(x => x.Strategy(It.IsAny<object[]>())).Returns(mockCommand.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.RemoveProperty", (object[] args) => mockStrategyReturnCommand.Object.Strategy(args)).Execute();

        Mock<IStrategy> mockStrategyReturnIInjectable = new Mock<IStrategy>();
        mockStrategyReturnIInjectable.Setup(x => x.Strategy(It.IsAny<object[]>())).Returns(mockInjecting.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Command.SetupCommand", (object[] args) => mockStrategyReturnIInjectable.Object.Strategy(args)).Execute();

        Mock<IStrategy> mockStrategyReturnEmpty = new Mock<IStrategy>();
        mockStrategyReturnEmpty.Setup(x => x.Strategy()).Returns(mockCommand.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Command.Empty", (object[] args) => mockStrategyReturnEmpty.Object.Strategy(args)).Execute();

    }

    [Fact]
    public void EndMoveCommandPositive()
    {
        Mock<IMoveCommandEndable> endable = new Mock<IMoveCommandEndable>();
        Mock<IUObject> obj = new Mock<IUObject>();
        endable.SetupGet(a => a.uobj).Returns(obj.Object).Verifiable();
        endable.SetupGet(a => a.property).Returns(new List<string>() { "Velocity" }).Verifiable();
        ICommand emc = new EndMoveCommand(endable.Object);
        emc.Execute();
        endable.Verify();
    }

    [Fact]
    public void ExceptionFromUobjectNegative()
    {
        Mock<IMoveCommandEndable> endable = new Mock<IMoveCommandEndable>();
        endable.SetupGet(a => a.uobj).Throws<Exception>().Verifiable();
        endable.SetupGet(a => a.property).Returns(new List<string>() { "Velocity" }).Verifiable();
        ICommand emc = new EndMoveCommand(endable.Object);
        Assert.Throws<Exception>(() => emc.Execute());
    }

    [Fact]
    public void ExceptionFromVelocityNegative()
    {
        Mock<IMoveCommandEndable> endable = new Mock<IMoveCommandEndable>();
        Mock<IUObject> obj = new Mock<IUObject>();
        endable.SetupGet(a => a.uobj).Returns(obj.Object).Verifiable();
        endable.SetupGet(a => a.property).Throws<Exception>().Verifiable();
        ICommand emc = new EndMoveCommand(endable.Object);
        Assert.Throws<Exception>(() => emc.Execute());
    }
}
