namespace SpaceBattle.Lib;

public class MacroCommand : ICommand
{   
    IList<ICommand> list;

    public MacroCommand(IList<ICommand> list)
    {
        this.list = list;
    }
    
    public void Execute()
    {
        foreach (ICommand c in list)
        {
            c.Execute();
        }
    }
}
