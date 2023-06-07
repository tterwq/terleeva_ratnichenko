namespace SpaceBattle.Lib;
using Hwdtech;

public class GameQueuePushCommandStrategy : IStrategy  // "Game.Command.Create.FromMessage"
{
    public object Strategy(params object[] args)
    {
        int id = (int)args[0];

        ICommand cmd = (ICommand)args[1];

        var queue = IoC.Resolve<Queue<ICommand>>("Game.Get.Queue", id);

        return new ActionCommand(() => { queue.Enqueue(cmd); });
    }
}
