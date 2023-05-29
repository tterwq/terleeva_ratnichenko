namespace SpaceBattle.Lib;

using System.Collections.Concurrent;

public class SenderAdapter : ISender
{
    BlockingCollection<ICommand> queue;

    public SenderAdapter(BlockingCollection<ICommand> queue) => this.queue = queue;

    public void Send(ICommand cmd)
    {
        queue.Add(cmd);
    }
}
