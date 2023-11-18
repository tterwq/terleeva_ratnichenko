namespace SpaceBattle.Lib;

using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class APITest
{
	[Fact]
	public void ApiSendJson()
	{
        new InitScopeBasedIoCImplementationCommand().Execute();

		var ic = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", ic).Execute();
		
		var fs = new Mock<IStrategy>();
		fs.Setup(o => o.Strategy(It.IsAny<object[]>())).Returns(new Dictionary<string, object>());

		var command = new Mock<ICommand>();
		command.Setup(o => o.Execute()).Verifiable();

		var ss = new Mock<IStrategy>();
		ss.Setup(o => o.Strategy(It.IsAny<object[]>())).Returns(command.Object);

		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SerializeContract", (object[] args) => fs.Object.Strategy()).Execute();
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SendCommand", (object[] args) => ss.Object.Strategy()).Execute();

		var mc = new MessageContract();
		var wa = new WebApi();


		wa.GetOperationMessage(mc);


		command.Verify();
	}

    [Fact]
    public void ApiThrowsException()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
		var ic = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", ic).Execute();
		
		var fs = new Mock<IStrategy>();
		fs.Setup(o => o.Strategy(It.IsAny<object[]>())).Throws<Exception>();

		var command = new Mock<ICommand>();
		command.Setup(o => o.Execute()).Verifiable();

		var ss = new Mock<IStrategy>();
		ss.Setup(o => o.Strategy(It.IsAny<object[]>())).Returns(command.Object);

		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SerializeContract", (object[] args) => fs.Object.Strategy()).Execute();
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SendCommand", (object[] args) => ss.Object.Strategy()).Execute();

		var mc = new MessageContract();
		var wa = new WebApi();

        Assert.ThrowsAny<Exception>(() => wa.GetOperationMessage(mc));
    }
}
