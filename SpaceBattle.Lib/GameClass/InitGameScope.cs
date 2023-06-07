namespace SpaceBattle.Lib;
using Hwdtech;

public class InitGameScope : IStrategy
{
    public object Strategy(params object[] _args)
    {

        object scope = IoC.Resolve<object>("Scopes.Current");
        object scopenew = IoC.Resolve<object>("Game.GetScope", (string)_args[0]);

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scopenew).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "QueueDequeue", (Func<object[], ICommand>)(args => (ICommand)new QueueDequeue().Strategy((Queue<ICommand>)args[0]))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "QueueEnqueue", (Func<object[], ICommand>)(args => new QueueEnqueueCommand((Queue<ICommand>)args[0], (ICommand)args[1]))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetQuantum", (Func<object[], object>)(args => (int)_args[1])).Execute();

        Dictionary<string, IUObject> objects = new Dictionary<string, IUObject>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.Objects", (Func<object[], Dictionary<string, IUObject>>)(args => objects)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.GetObject", (Func<object[], IUObject>)(args => (IUObject)new GetItem().Strategy(args[0]))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.RemoveObject", (Func<object[], ICommand>)(args => new RemoveItemCommand((string)args[0]))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        return scopenew;
    }
}
