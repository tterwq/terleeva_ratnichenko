using Hwdtech;


namespace SpaceBattle.Lib;

public class RemoveItemCommand : ICommand
{
    string key;
    public RemoveItemCommand(string _key)
    {
        key = _key;
    }
    public void Execute()
    {
        IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").Remove(key);
    }
}
