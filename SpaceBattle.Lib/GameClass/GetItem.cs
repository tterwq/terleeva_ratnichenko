using Hwdtech;
namespace SpaceBattle.Lib;

public class GetItem : IStrategy
{

    public object Strategy(params object[] args)
    {
        IoC.Resolve<Dictionary<string, IUObject>>("General.Objects").TryGetValue((string)args[0], out IUObject? obj);

        if (obj != null)
        {
            return obj;
        }
        throw new Exception();
    }
}
