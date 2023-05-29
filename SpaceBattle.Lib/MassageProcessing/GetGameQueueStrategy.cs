namespace SpaceBattle.Lib;
using Hwdtech;


public class GetGameQueueStrategy : IStrategy  // "Game.Get.Queue"
{
     public object Strategy(params object[] args)
    {
        int gameid = (int)args[0];

        Queue<ICommand>? queue;
        
        if (IoC.Resolve<IDictionary<int, Queue<ICommand>>>("Game.Queue.Map").TryGetValue(gameid, out queue))
        {
            return queue;
        }

        throw new Exception();
    }
}
