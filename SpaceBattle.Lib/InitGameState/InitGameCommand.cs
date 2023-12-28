namespace SpaceBattle.Lib;
using Hwdtech;

public class InitGameCommand : ICommand
{
    private int countGameObj;
    public InitGameCommand(int countGameObj)
    {
        this.countGameObj = countGameObj;
    }
    public void Execute()
    {
        var gameObjsID = IoC.Resolve<IEnumerable<string>>("CreateGameObjects", countGameObj);

        foreach (var gameObjID in gameObjsID)
        {
            IoC.Resolve<ICommand>("GameObj.SetProperty", gameObjID, "position", IoC.Resolve<Vector>("PositionIteratorGetNext")).Execute();
            IoC.Resolve<ICommand>("GameObj.SetProperty", gameObjID, "fuel", IoC.Resolve<int>("FuelIteratorGetNext")).Execute();
        }
    }
}
