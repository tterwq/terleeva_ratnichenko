namespace SpaceBattle.Lib.Test;

public class GameCommandTest
{   
    Dictionary<int, Dictionary<int, IStrategy>> dictException = new ();
    Dictionary<int, IStrategy> exceptionNotFoundCommand = new ();
    Mock<IStrategy> exceptionNotFoundException = new ();
    Dictionary<string, IReceiver> dictReceiver = new();
    Dictionary<string, TimeSpan> dictTime = new();

    public GameCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        var getTimeStrategy = new Mock<IStrategy>();
        getTimeStrategy.Setup(s => s.Strategy(It.IsAny<object[]>())).Returns((object[] args) => dictTime[(string)args[0]]);

        var getReceiverStrategy = new Mock<IStrategy>();
        getReceiverStrategy.Setup(s => s.Strategy(It.IsAny<object[]>())).Returns((object[] args) => dictReceiver[(string)args[0]]);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetTime", (object[] args) => getTimeStrategy.Object.Strategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetReceiver", (object[] args) => getReceiverStrategy.Object.Strategy(args)).Execute();

        exceptionNotFoundException.Setup(x => x.Strategy()).Verifiable();
            
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Exception.GetTree", (object[] args) => {
            return dictException;
        }).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Exception.NotFoundCommandSubTree", (object[] args) => {
            return exceptionNotFoundCommand;
        }).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.Exception.NotFoundExceptionHandler", (object[] args) => {
            return exceptionNotFoundException.Object;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler", (object[] args) => {
            ICommand cmd = (ICommand)args[0];
            Exception exc = (Exception)args[1];

            var excHadler = new ExceptionHandlerStrategy().Strategy(cmd, exc);

            return excHadler;
        }).Execute();
        
        var getScopeStrategy = new Mock<IStrategy>();
        getScopeStrategy.Setup(s => s.Strategy(It.IsAny<object[]>())).Returns((object[] args) => scope);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetScope", (object[] args) => getScopeStrategy.Object.Strategy(args)).Execute();  
    }

    [Fact]
    public void GameCommandExecuteTest()
    {
        var cmd1 = new Mock<ICommand>();
        cmd1.Setup(c => c.Execute()).Verifiable();

        var cmd2 = new Mock<ICommand>();
        cmd2.Setup(c => c.Execute()).Verifiable();

        var cmd3 = new Mock<ICommand>();
        cmd3.Setup(c => c.Execute()).Verifiable();

        var listCmds = new List<ICommand>() {cmd1.Object, cmd2.Object, cmd3.Object};

        dictReceiver.Add("1", new QueueAdapter(new Queue<ICommand>(listCmds)));
        dictTime.Add("1", TimeSpan.FromSeconds(5));

        var game = new GameCommand("1");
        game.Execute();

        cmd1.Verify(c => c.Execute(), Times.Once());
        cmd2.Verify(c => c.Execute(), Times.Once());
        cmd3.Verify(c => c.Execute(), Times.Once());
    }
    
    [Fact]
    public void GameCommandExecuteThrowsExceptionHandlerTest()
    {   
        var cmd = new Mock<ICommand>();
        cmd.Setup(c => c.Execute()).Throws<Exception>().Verifiable();
        var listCmds = new List<ICommand>() {cmd.Object};

        dictReceiver.Add("1", new QueueAdapter(new Queue<ICommand>(listCmds)));
        dictTime.Add("1", TimeSpan.FromSeconds(5));

        var game = new GameCommand("1");
        game.Execute();
        
        cmd.Verify();
    }

    [Fact]
    public void ExceptionHandlerTest()
    {
        int count = 0;
        var e = new Exception("123");
        var cmd = new Mock<ICommand>();
        cmd.Setup(c => c.Execute()).Throws<Exception>(() => e);

        var str = new Mock<IStrategy>();
        str.Setup(s => s.Strategy()).Callback(() => count +=1);

        exceptionNotFoundCommand.Add(e.GetType().GetHashCode(), str.Object);

        IoC.Resolve<IStrategy>("ExceptionHandler", cmd.Object, e).Strategy();

        cmd.Verify();
    }
}
