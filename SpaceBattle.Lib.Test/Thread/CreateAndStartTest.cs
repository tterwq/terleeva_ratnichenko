namespace SpaceBattle.Lib.Test;

public class CreateAndStartThreadStrategyTests
{
    Dictionary<string, MyThread> dictThread;
    Dictionary<string, IReceiver> dictReceiver;
    Dictionary<string, ISender> dictSender;

    public CreateAndStartThreadStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        this.dictThread = new Dictionary<string, MyThread>();
        this.dictReceiver = new Dictionary<string, IReceiver>();
        this.dictSender = new Dictionary<string, ISender>();

        new ServerThreadDependecies(dictThread, dictReceiver, dictSender).Execute();
    }
    public class ServerThreadDependecies : ICommand
    {
        Dictionary<string, MyThread> dictThreads;
        Dictionary<string, IReceiver> dictReceivers;
        Dictionary<string, ISender> dictSenders;
        public ServerThreadDependecies(Dictionary<string, MyThread> dt, Dictionary<string, IReceiver> dr, Dictionary<string, ISender> ds)
        {
            this.dictThreads = dt;
            this.dictReceivers = dr;
            this.dictSenders = ds;
        }
        public void Execute()
        {
            var scope = IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root")));
            scope.Execute();

            var getDictThreadsStrategy = new Mock<IStrategy>();
            getDictThreadsStrategy.Setup(s => s.Strategy(It.IsAny<object[]>())).Returns((object[] args) => this.dictThreads);

            var getThreadStrategy = new Mock<IStrategy>();
            getThreadStrategy.Setup(s => s.Strategy(It.IsAny<object[]>())).Returns((object[] args) => this.dictThreads[(string)args[0]]);

            var setThreadStrategy = new Mock<IStrategy>();
            setThreadStrategy.Setup(c => c.Strategy(It.IsAny<object[]>())).Returns((object[] args) =>
            {
                var setThreadCommand = new Mock<ICommand>();
                setThreadCommand.Setup(c => c.Execute()).Callback(() => this.dictThreads.Add((string)args[0], (MyThread)args[1]));
                return setThreadCommand.Object;
            });

            var getReceiverStrategy = new Mock<IStrategy>();
            getReceiverStrategy.Setup(c => c.Strategy(It.IsAny<object[]>())).Returns((object[] args) => this.dictReceivers[(string)args[0]]);

            var setReceiverStrategy = new Mock<IStrategy>();
            setReceiverStrategy.Setup(c => c.Strategy(It.IsAny<object[]>())).Returns((object[] args) =>
            {
                var setReceiverCommand = new Mock<ICommand>();
                setReceiverCommand.Setup(c => c.Execute()).Callback(() => this.dictReceivers.Add((string)args[0], new ReceiverAdapter((BlockingCollection<ICommand>)args[1])));
                return setReceiverCommand.Object;
            });

            var getSenderStrategy = new Mock<IStrategy>();
            getSenderStrategy.Setup(c => c.Strategy(It.IsAny<object[]>())).Returns((object[] args) => this.dictSenders[(string)args[0]]);

            var setSenderStrategy = new Mock<IStrategy>();
            setSenderStrategy.Setup(c => c.Strategy(It.IsAny<object[]>())).Returns((object[] args) =>
            {
                var setSenderCommand = new Mock<ICommand>();
                setSenderCommand.Setup(c => c.Execute()).Callback(() => this.dictSenders.Add((string)args[0], new SenderAdapter((BlockingCollection<ICommand>)args[1])));
                return setSenderCommand.Object;
            });

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Set Thread", (object[] args) => setThreadStrategy.Object.Strategy(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get Thread", (object[] args) => getThreadStrategy.Object.Strategy(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Set Receiver", (object[] args) => setReceiverStrategy.Object.Strategy(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get Receiver", (object[] args) => getReceiverStrategy.Object.Strategy(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Set Sender", (object[] args) => setSenderStrategy.Object.Strategy(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get Sender", (object[] args) => getSenderStrategy.Object.Strategy(args)).Execute();

            var createAndStartStrategy = new CreateAndStartStrategy();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateAndStartThread", (object[] args) => createAndStartStrategy.Strategy(args)).Execute();

            var hardStopCmdStrategy = new HardStopStrategy();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "HardStopThread", (object[] args) => hardStopCmdStrategy.Strategy(args)).Execute();

            var softStopCmdStrategy = new SoftStopStrategy();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SoftStopThread", (object[] args) => softStopCmdStrategy.Strategy(args)).Execute();

            var sendCmdStrategy = new SendCommandStrategy();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Send Command", (object[] args) => sendCmdStrategy.Strategy(args)).Execute();
        }
    }

    [Fact]
    public void CreateAndStartThreadStrategyWithoutActionTest()
    {

        var CAST = IoC.Resolve<ICommand>("CreateAndStartThread", "1");
        CAST.Execute();

        IoC.Resolve<ICommand>("Send Command", "1", new ServerThreadDependecies(dictThread, dictReceiver, dictSender)).Execute();

        var thread = IoC.Resolve<MyThread>("Get Thread", "1");

        Assert.True(thread.thread.IsAlive);

        IoC.Resolve<ICommand>("HardStopThread", "1").Execute();

    }

    [Fact]
    public void CreateAndStartThreadStrategyWithActionTest()
    {

        int count = 0;
        var act = new Action(() => { count += 1; });

        var CAST = IoC.Resolve<ICommand>("CreateAndStartThread", "2", act);
        CAST.Execute();

        IoC.Resolve<ICommand>("Send Command", "2", new ServerThreadDependecies(dictThread, dictReceiver, dictSender)).Execute();

        var thread = IoC.Resolve<MyThread>("Get Thread", "2");

        Assert.True(thread.thread.IsAlive);
        Assert.True(count == 1);


        IoC.Resolve<ICommand>("HardStopThread", "2").Execute();

    }
}
