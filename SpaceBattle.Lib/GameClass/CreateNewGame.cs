namespace SpaceBattle.Lib;
using Hwdtech;

public class CreateNewGame : IStrategy
{
    int quantum;
    string scopeid;
    public CreateNewGame(string scopeid, int quantum = 1000)
    {
        this.quantum = quantum;
        this.scopeid = scopeid;
    }
    public object Strategy(params object[] args)
    {
        Queue<ICommand> queue = new Queue<ICommand>();
        object newscope = new InitGameScope().Strategy(scopeid, quantum);

        return IoC.Resolve<ICommand>("Commands.GameCommand", newscope, queue);
    }
}
