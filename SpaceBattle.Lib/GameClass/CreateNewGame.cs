using Hwdtech;
namespace SpaceBattle.Lib;

public class CreateNewGame : IStrategy
{
    int quantum;

    public CreateNewGame(int _quantum =1000)
    {
        quantum = _quantum;
    }

    public object Strategy(params object[] args)
    {
        Queue<ICommand> queue = new Queue<ICommand>();
        object scope = new InitGameScope().RunStrategy(quantum);
        return IoC.Resolve<ICommand>("SpaceBattle.Operation.Game.CreateNew", scope, queue);
    }
}
