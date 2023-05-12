namespace SpaceBattle.Lib;

public class HardStopStrategy : IStrategy
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
        ICommand HScmd = new HardStopCommand(thread);

        Action act = () => 
        {
            HScmd.Execute();

            if (action != null)
            {
                action();
            }
        };

        ICommand cmd = IoC.Resolve<ICommand>("Send Command", id, new ActionCommand(act));
        return cmd;
    }
}
