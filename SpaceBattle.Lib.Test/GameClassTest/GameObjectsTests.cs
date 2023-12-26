using Hwdtech;
using Hwdtech.Ioc;
using Moq;


namespace SpaceBattle.Lib.Test;

public class GameObjectsTests
{
    Dictionary<string, object> scopes = new Dictionary<string, object>();
    public GameObjectsTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var GetScope = new Mock<IStrategy>();
        GetScope.Setup(o => o.Strategy(It.IsAny<Object[]>())).Returns((object[] args) => scopes[(string)args[0]]);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetScope", (object[] args) => GetScope.Object.Strategy(args)).Execute();
        scopes.Add("1", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root")));
    }

    [Fact]
    public void getItemTest()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameCommand", (object[] args) => new ActionCommand(
            () =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();

        ICommand gameCommand = (ICommand)new CreateNewGame("1").Strategy();
        gameCommand.Execute();

        var mockObj = new Mock<IUObject>();

        IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Add("0", mockObj.Object);

        var resolvedObj = IoC.Resolve<IUObject>("General.GetItem", "0");

        Assert.Equal(mockObj.Object, resolvedObj);
    }

    [Fact]
    public void removeItemTest()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameCommand", (object[] args) => new ActionCommand(
            () =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();

        ICommand gameCommand = (ICommand)new CreateNewGame("1").Strategy();
        gameCommand.Execute();

        var mockObj = new Mock<IUObject>();

        IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Add("0", mockObj.Object);
        Assert.True(IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Count() == 1);

        IoC.Resolve<ICommand>("General.RemoveItem", "0").Execute();
        Assert.True(IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Count() == 0);
    }
    
    [Fact]
    public void getItemTestObjectNotExists()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameCommand", (object[] args) => new ActionCommand(
            () =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();

        ICommand gameCommand = (ICommand)new CreateNewGame("1").Strategy();
        gameCommand.Execute();

        var mockObj = new Mock<IUObject>();

        IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Add("0", mockObj.Object);

        IoC.Resolve<ICommand>("General.RemoveItem", "0").Execute();
        Assert.True(IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Count() == 0);

        Assert.Throws<Exception>(
            () =>
            {
                IoC.Resolve<IUObject>("General.GetItem", "0");
            }
        );
    }

}
