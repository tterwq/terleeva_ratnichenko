using Hwdtech;
using Hwdtech.Ioc;
using Moq;


namespace SpaceBattle.Lib.Test;

public class QueueStrategiesTests
{
    public QueueStrategiesTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Operation.Game.CreateNew", (object[] args) => new ActionCommand(
            () =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();
    }

    [Fact]
    public void enqueueTest()
    {
        var queue = new Queue<ICommand>();
        var cmd = new Mock<ICommand>();

        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Operation.Game.CreateNew", (object[] args) => new ActionCommand(
            () =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();

        ICommand gameCommand = (ICommand)new CreateNewGame().Strategy();
        gameCommand.Execute();

        IoC.Resolve<ICommand>("GetOutGueue", queue, cmd.Object).Execute();
        Assert.True(queue.Count() == 1);
    }
    
    [Fact]
    public void dequeueTest()
    {
        var queue = new Queue<ICommand>();
        var cmd = new Mock<ICommand>();

        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Operation.Game.CreateNew", (object[] args) => new ActionCommand(
            () =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();

        ICommand gameCommand = (ICommand)new CreateNewGame().Strategy();
        gameCommand.Execute();

        IoC.Resolve<ICommand>("PutInQueue", queue, cmd.Object).Execute();

        var cmd1 = IoC.Resolve<ICommand>("GetOutGueue", queue);
        Assert.Equal(cmd.Object, cmd1);
    }
}
