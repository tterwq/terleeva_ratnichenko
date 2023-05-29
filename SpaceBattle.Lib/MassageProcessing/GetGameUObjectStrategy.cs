namespace SpaceBattle.Lib;
using Hwdtech;


public class GetGameUObjectStrategy : IStrategy  
{
     public object Strategy(params object[] args)
    {
        int objectid = (int)args[0];

        IUObject? uObject;
        
        if (IoC.Resolve<IDictionary<int, IUObject>>("Game.UObject.Map").TryGetValue(objectid, out uObject))
        {
            return uObject;
        }
        throw new Exception();
    }
}
