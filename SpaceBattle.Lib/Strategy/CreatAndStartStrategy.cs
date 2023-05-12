namespace SpaceBattle.Lib;

public class CreateAndStartStrategy : IStrategy
{
    public object Strategy(params object[] args)
    {
        var id = (string)args[0];
        Action? action;

        if (args.Count() < 2)
        {
            action = null;
        }
        else
        {
            action = (Action)args[1];
        }

        Action act = () => 
        {

            BlockingCollection<ICommand> queue = new BlockingCollection<ICommand>();
            IoC.Resolve<ICommand>("Set Receiver", id, queue).Execute();
            MyThread thread = new MyThread(IoC.Resolve<IReceiver>("Get Receiver", id));
            IoC.Resolve<ICommand>("Set Sender", id, queue).Execute();
            IoC.Resolve<ICommand>("Set Thread", id, thread).Execute();
            thread.Execute();
            
            if (action != null)
            {
                action();
            }
        };

        return new ActionCommand(act);
    }
}
