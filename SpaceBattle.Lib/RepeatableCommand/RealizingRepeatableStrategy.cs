namespace SpaceBattle.Lib;
using Hwdtech;

public class RealizingRepeatableStrategy : IStrategy
{
    public object Strategy(params object[] args)
    {
        var nameofdepend = (string)args[0];
        var obj = (IUObject)args[1];
        var macro = IoC.Resolve<ICommand>("SpaceBattle.Operation.MacroCommand", nameofdepend, obj);
        var inj = IoC.Resolve<ICommand>("SpaceBattle.Operation.Inject", macro);
        var cmd = IoC.Resolve<ICommand>("SpaceBattle.Operation.Repeat", inj);
        return cmd;
    }
}
