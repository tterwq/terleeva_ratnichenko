using Hwdtech;
namespace SpaceBattle.Lib;

public class DeleteGame : IStrategy
{
    public object Strategy(params object[] args)
    {
        IoC.Resolve<Hwdtech.ICommand>("SpaceBattle.Scopes.Current.Set", IoC.Resolve<object>("Scopes.Current.SetScopes.Outer")).Execute();
        return new object();
    }
}
