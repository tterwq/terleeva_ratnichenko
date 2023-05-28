using Hwdtech;
using Hwdtech.Ioc;
using Moq;


namespace SpaceBattle.Lib.Test;

public class GameObjectsTests
{
    public GameObjectsTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
    }

    [Fact]
    public void getItemTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameCommand", (object[] args) => new ActionCommand(
            () =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();

        ICommand gameCommand = (ICommand)new CreateNewGame().Strategy();
        gameCommand.Execute();

        var mockObj = new Mock<IUObject>();

        IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Add("0", mockObj.Object);

        var resolvedObj = IoC.Resolve<IUObject>("General.GetItem", "0");

        Assert.Equal(mockObj.Object, resolvedObj);
    }

    [Fact]
    public void removeItemTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameCommand", (object[] args) => new ActionCommand(
            () =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();

        ICommand gameCommand = (ICommand)new CreateNewGame().Strategy();
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
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameCommand", (object[] args) => new ActionCommand(
            () =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();

        ICommand gameCommand = (ICommand)new CreateNewGame().Strategy();
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
