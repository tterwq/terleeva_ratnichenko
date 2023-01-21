namespace SpaceBattle.Lib;
using Hwdtech;

public class RepeatableCommand : ICommand
{
    ICommand cmd;

    public RepeatableCommand(ICommand cmd)
    {
        this.cmd = cmd;
    }
    public void Execute()
    {
        this.cmd.Execute();
        IoC.Resolve<ICommand>("SpaceBattle.Queue.Push", this).Execute();
    }
}
