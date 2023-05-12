namespace SpaceBattle.Lib;

public class HardStopCommand : ICommand
{
    MyThread stopthread;
    public HardStopCommand(MyThread stopthread)
    {
        this.stopthread = stopthread;
    } 

    public void Execute()
    {
        if (Thread.CurrentThread == stopthread.thread)
        {
            stopthread.Stop();
        }
        else
        {
            throw IoC.Resolve<Exception>("ExceptionStopThread");
        }
    }
}
