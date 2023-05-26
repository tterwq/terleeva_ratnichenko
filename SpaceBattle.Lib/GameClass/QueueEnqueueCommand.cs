namespace SpaceBattle.Lib;

public class PutInQueue : ICommand
{
    Queue<ICommand> target;
    ICommand command;
    public PutInQueue(Queue<ICommand> _target, ICommand cmd)
    {
        this.target = _target;
        this.command = cmd;
    }
    public void Execute()
    {
        target.Enqueue(command);
    }
}
