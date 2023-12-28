namespace SpaceBattle.Lib;

public class ActionCommand : ICommand
{
    Action act;

    public ActionCommand(Action act)
    {
        this.act = act;
    }

    public void Execute()
    {
        act();
    }
}
