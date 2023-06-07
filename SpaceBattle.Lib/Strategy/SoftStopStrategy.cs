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
                var cmd = new ActionCommand(action);
                IoC.Resolve<ICommand>("Send Command", id, cmd);
                cmd.Execute();
            }
        };

        return  new ActionCommand(act);
    }
}
