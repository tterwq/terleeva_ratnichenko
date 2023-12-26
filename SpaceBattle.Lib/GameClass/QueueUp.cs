namespace SpaceBattle.Lib;


public class QueueEnqueueCommand : ICommand
{
    Queue<ICommand> target;
    ICommand command;
    public QueueEnqueueCommand(Queue<ICommand> _target, ICommand cmd)
    {
        this.target = _target;
        this.command = cmd;
    }
    public void Execute()
    {
        target.Enqueue(command);
    }
}
