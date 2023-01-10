using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class MacroCommandTest
{
    public MacroCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        Mock<ICommand> commandMock = new Mock<ICommand>();
        commandMock.Setup(c => c.Execute());

        Mock<IStrategy> propertyMock = new Mock<IStrategy>();
        propertyMock.Setup(s => s.Strategy(It.IsAny<object[]>())).Returns(commandMock.Object);

        Mock<IStrategy> listMock = new Mock<IStrategy>();
        listMock.Setup(l => l.Strategy()).Returns(new string[] { "Second" });

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Operation.First", (object[] p) => listMock.Object.Strategy(p)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Second", (object[] p) => propertyMock.Object.Strategy(p)).Execute();
    }

    [Fact]
    public void TestPositiveMacroCommand()
    {
        Mock<IUObject> obj = new Mock<IUObject>();
        var newmc = new MacroCommandStrategy();

        var macrocommand = (ICommand)newmc.Strategy("First", obj.Object);

        macrocommand.Execute();
    }
}
