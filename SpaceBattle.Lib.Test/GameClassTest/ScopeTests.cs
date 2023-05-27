using Hwdtech;
using Hwdtech.Ioc;
using Moq;


namespace SpaceBattle.Lib.Test;

public class ScopeTests
{
    public ScopeTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
    }

    [Fact]
    public void currentScopeIsEmpty()
    {
        object scope = new InitGameScope().Strategy(1000);

        Assert.Throws<ArgumentException>(
            () =>
            {
                IoC.Resolve<int>("GetQuantum");
            }
        );
        Assert.Throws<ArgumentException>(
            () =>
            {
                IoC.Resolve<ICommand>("GetOutGueue");
            }
        );
        Assert.Throws<ArgumentException>(
            () =>
            {
                IoC.Resolve<ICommand>("PutInQueue");
            }
        );
    }

    [Fact]
    public void CreateNewGameTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Operation.Game.CreateNew", (object[] args) => new ActionCommand(
            () =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateNew", (object[] args) => new CreateNewGame((int)args[0]).Strategy()).Execute();

        ICommand gameCommand = IoC.Resolve<ICommand>("Game.CreateNew", 500);
        gameCommand.Execute();

        Assert.Equal(500, IoC.Resolve<int>("GetQuantum"));
        Assert.True(IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Count() == 0);
        Assert.Throws<Exception>(
            () =>
            {
                IoC.Resolve<ICommand>("GetOutGueue", new Queue<ICommand>());
            }
        );
    }
    
    [Fact]
    public void DeleteGameTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Operation.Game.CreateNew", (object[] args) => new ActionCommand(
            () =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateNew", (object[] args) => new CreateNewGame((int)args[0]).Strategy()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.DeleteGame", (object[] args) => new DeleteGame()).Execute();

        ICommand gameCommand = IoC.Resolve<ICommand>("Game.CreateNew", 1000);
        IStrategy deleteGame = IoC.Resolve<IStrategy>("Game.DeleteGame");
        gameCommand.Execute();

        deleteGame.Strategy();
        Assert.Throws<ArgumentException>(
            () =>
            {
                IoC.Resolve<int>("GetQuantum");
            }
        );
        Assert.Throws<ArgumentException>(
            () =>
            {
                IoC.Resolve<ICommand>("GetOutGueue");
            }
        );
        Assert.Throws<ArgumentException>(
            () =>
            {
                IoC.Resolve<ICommand>("PutInQueue");
            }
        );
    }
}
