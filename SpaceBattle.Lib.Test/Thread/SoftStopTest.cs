using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib.Test;

public class SoftStopCommandStrategyTests
{ 
	Dictionary<string, MyThread> dictThread;
	Dictionary<string, IReceiver> dictReceiver;
	Dictionary<string, ISender> dictSender;

	public SoftStopCommandStrategyTests()
	{
        new InitScopeBasedIoCImplementationCommand().Execute();
        this.dictThread = new Dictionary<string, MyThread>();
		this.dictReceiver = new Dictionary<string, IReceiver>();
		//dictReceivers.Add("5", new Mock<IReceiver>().Object);
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
		//dictSenders.Add("5", new Mock<ISender>().Object);


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
            setSenderCommand.Setup(c => c.Execute()).Callback(() => this.dictSenders.Add((string)args[0],new SenderAdapter((BlockingCollection<ICommand>)args[1])));
            return setSenderCommand.Object;
        });

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetDict", (object[] args) => getDictThreadsStrategy.Object.Strategy(args)).Execute();
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
     public void SoftStopCommandStrategyWithActionTests()
     {
         AutoResetEvent mre = new AutoResetEvent(false);
         var cmd1 = new Mock<ICommand>();
         cmd1.Setup(c => c.Execute()).Verifiable();
		 IoC.Resolve<ICommand>("CreateAndStartThread", "5").Execute();
		 IoC.Resolve<ICommand>("Send Command", "5", new ServerThreadDependecies(dictThread, dictReceiver, dictSender)).Execute();

         IoC.Resolve<ICommand>("SoftStopThread", "5", () => {mre.Set();}).Execute();
         IoC.Resolve<ICommand>("Send Command", "5", cmd1.Object).Execute();
       
         mre.WaitOne();
         cmd1.Verify(c => c.Execute(), Times.Once());
         var rc = IoC.Resolve<IReceiver>("Get Receiver", "5");
         Assert.True(rc.isEmpty());
        
    }

    [Fact]
    public void SoftStopCommandStrategyWithoutActionTests()
    {
		 ManualResetEvent mre = new ManualResetEvent(false);

         var cmd1 = new Mock<ICommand>();
         cmd1.Setup(c => c.Execute()).Callback(() => mre.Set()).Verifiable();

         IoC.Resolve<ICommand>("CreateAndStartThread", "5").Execute();
		 IoC.Resolve<ICommand>("Send Command", "5", new ServerThreadDependecies(dictThread, dictReceiver, dictSender)).Execute();

       
         IoC.Resolve<ICommand>("SoftStopThread", "5").Execute();
         IoC.Resolve<ICommand>("Send Command", "5", cmd1.Object).Execute();
        
         mre.WaitOne();
         cmd1.Verify(c => c.Execute(), Times.Once());
        
         var receiver = IoC.Resolve<IReceiver>("Get Receiver", "5");
         Assert.True(receiver.isEmpty());

    }

}
