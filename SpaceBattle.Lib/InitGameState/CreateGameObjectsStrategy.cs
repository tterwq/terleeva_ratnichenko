using Hwdtech;
namespace SpaceBattle.Lib;

public class CreateGameObjectsStrategy : IStrategy
{
    public object Strategy(params object[] args)
    {
        var countGameObj = (int)args[0];
        var gameObjsID = IoC.Resolve<IEnumerable<string>>("GetGameObjId", countGameObj);

        foreach (var gameObjID in gameObjsID)
        {
            IoC.Resolve<ICommand>("CreateGameObj", gameObjID).Execute();
        }
        return gameObjsID;
    }
}
