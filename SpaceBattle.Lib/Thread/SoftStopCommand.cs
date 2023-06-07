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
            var receiver = IoC.Resolve<IReceiver>("Get Receiver", id);
            new UpdateBehaviourCommand(stopthread, () =>
            {
                if (receiver.isEmpty())
                {
                    stopthread.Stop();
                }
                else
                {
                    var cmd = receiver.Receive();

                    cmd.Execute();
                }

            }).Execute();
        }
        else
        {
            throw IoC.Resolve<Exception>("ExceptionStopThread");
        }
    }
}
