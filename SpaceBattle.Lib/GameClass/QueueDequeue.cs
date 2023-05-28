namespace SpaceBattle.Lib;


public class QueueDequeue : IStrategy
{
    public object Strategy(params object[] args)
    {
        var queue = (Queue<ICommand>)args[0];
        if (!queue.TryDequeue(out ICommand? command))
        {
            throw new Exception();
        }
        else
        {
            return command;
        }
    }
}
