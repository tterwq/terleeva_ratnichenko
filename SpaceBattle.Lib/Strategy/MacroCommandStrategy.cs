using Hwdtech;

namespace SpaceBattle.Lib;

public class MacroCommandStrategy : IStrategy
{   
    public object Strategy(params object[] args)
    {
        string key = (string)args[0];
        IUObject obj = (IUObject)args[1];

        IList<string> dependencies = IoC.Resolve<IList<string>>("SpaceBattle.Operation." +key);
        IList<ICommand> list = new List<ICommand>();

        foreach (string d in dependencies)
        {
            list.Add(IoC.Resolve<ICommand>(d, obj));
        }

        return new MacroCommand(list);
    }
}
