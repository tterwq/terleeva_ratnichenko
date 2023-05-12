namespace SpaceBattle.Lib;

public class SoftStopStrategy : IStrategy
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

        MyThread thread = IoC.Resolve<MyThread>("Get Thread", id);
        ICommand SScmd = new SoftStopCommand(id, thread);

        Action act = () => {
            SScmd.Execute();
            
            if (action != null)
            {
                action();
            }
        };

        ICommand cmd = IoC.Resolve<ICommand>("Send Command", id, new ActionCommand(act));
        return cmd;
    }
}
