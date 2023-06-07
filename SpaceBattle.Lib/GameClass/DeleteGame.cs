namespace SpaceBattle.Lib;
using Hwdtech;
public class DeleteGame : IStrategy
{
    string scopeid;
    public DeleteGame(string scopeid)
    {
        this.scopeid = scopeid;
    }
    public object Strategy(params object[] args)
    {
        IoC.Resolve<ICommand>("Game.DeleteScope", scopeid).Execute();
        return new object();
    }
}
