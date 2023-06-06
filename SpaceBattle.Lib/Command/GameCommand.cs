namespace SpaceBattle.Lib;

public class GameCommand : ICommand
{
    string gameID;

    public GameCommand(string gameID)
    {
        this.gameID = gameID;
    }

    public void Execute()
    {
        var scope = IoC.Resolve<object>("GetScope", this.gameID);
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
        var queue = IoC.Resolve<IReceiver>("GetReceiver", this.gameID);
        var time = IoC.Resolve<TimeSpan>("GetTime", this.gameID);
        var stopwatch = Stopwatch.StartNew();
        
        while (stopwatch.Elapsed < time)
        {
            if (!queue.isEmpty())
            {
                var cmd = queue.Receive();
                try
                {
                    cmd.Execute();
                }
                catch (Exception e)
                {
                    IoC.Resolve<IStrategy>("ExceptionHandler", cmd, e).Strategy();
                }
            }
        }
    }
}
