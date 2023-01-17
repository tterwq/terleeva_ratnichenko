using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class RealizingRepeatableStrategyTests
{
    public RealizingRepeatableStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void RealizingRepeatableStrategyPositiveTest()
    {
        Mock<ICommand> cmd = new Mock<ICommand>();
        Mock<IStrategy> PropStrat = new Mock<IStrategy>();
        PropStrat.Setup(s => s.Strategy(It.IsAny<object[]>())).Returns(cmd.Object).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Operation.MacroCommand", (object[] args) => PropStrat.Object.Strategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Operation.Inject", (object[] args) => new RealizingInjectableStart().Strategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Operation.Repeat", (object[] args) => new RealizingRepeatableStart().Strategy(args)).Execute();

        Mock<IUObject> mockobj = new Mock<IUObject>();
        RealizingRepeatableStrategy RealizRS = new RealizingRepeatableStrategy();
        var longOp = RealizRS.Strategy(It.IsAny<string>(), mockobj.Object);
        Assert.IsAssignableFrom<ICommand>(longOp);
        PropStrat.Verify();
    }

    [Fact]
    public void InjectableCommandExecutePositiveTest()
    {
        Mock<ICommand> cmd = new Mock<ICommand>();
        cmd.Setup(s => s.Execute()).Verifiable();
        InjectableCommand InjCmd = new InjectableCommand(cmd.Object);
        InjCmd.Execute();
        cmd.Verify();
    }

    [Fact]
    public void InjectableCommandInjectPositiveTest()
    {
        Mock<ICommand> cmd = new Mock<ICommand>();
        cmd.Setup(s => s.Execute()).Verifiable();
        InjectableCommand InjCmd = new InjectableCommand(cmd.Object);
        InjCmd.Inject(cmd.Object);
        InjCmd.Execute();

        cmd.Verify();
    }

    [Fact]
    public void RepeatableCommandExecutePositiveTest()
    {
        Mock<ICommand> cmd = new Mock<ICommand>();
        Mock<IStrategy> PropStrat = new Mock<IStrategy>();
        PropStrat.Setup(s => s.Strategy(It.IsAny<object[]>())).Returns(cmd.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Queue.Push", (object[] args) => PropStrat.Object.Strategy(args)).Execute();
        cmd.Setup(s => s.Execute()).Verifiable();
        RepeatableCommand RepCmd = new RepeatableCommand(cmd.Object);
        RepCmd.Execute();
        cmd.Verify();
    }

    public class RealizingInjectableStart : IStrategy
    {
        public object Strategy(params object[] args)
        {
            var cmd = (ICommand)args[0];
            return new InjectableCommand(cmd);
        }
    }

    public class RealizingRepeatableStart : IStrategy
    {
        public object Strategy(params object[] args)
        {
            var cmd = (ICommand)args[0];
            return new RepeatableCommand(cmd);
        }
    }
}
