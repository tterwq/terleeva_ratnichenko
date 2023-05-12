namespace SpaceBattle.Lib;

public class SoftStopCommand : ICommand
{
    MyThread stopthread;
    string id;

    public SoftStopCommand(string id, MyThread stopthread)
    {
        this.stopthread = stopthread;
        this.id = id;
    }

    public void Execute()
    {
        if (Thread.CurrentThread == stopthread.thread)
        {
            var receiver = IoC.Resolve<IReceiver>("GetReceiver", id);
            var queue = new BlockingCollection<ICommand>();

            IoC.Resolve<ICommand>("SetReceiver", id, queue).Execute();

            new UpdateBehaviourCommand(stopthread, () =>
            {
                if (receiver.isEmpty())
                {
                    stopthread.Stop();
                }
            });
        }
        else
        {
            throw IoC.Resolve<Exception>("ExceptionStopThread");
        }
    }
}
