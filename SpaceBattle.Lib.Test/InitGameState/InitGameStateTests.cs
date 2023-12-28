using Hwdtech;
namespace SpaceBattle.Lib.Test;

public class InitGameStateTests
{
    public Dictionary<string, object> gameObjsDict = new();
    public Dictionary<string, object> gameIteratorDict = new();

    public InitGameStateTests()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateGameObj", (object[] args) =>
        new ActionCommand(() => gameObjsDict.Add((string)args[0], new Dictionary<string, object>()))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameObj.SetProperty", (object[] args) => new ActionCommand(() =>
        {
            var gameObjID = (string)args[0];
            var propName = (string)args[1];
            var prop = args[2];

            var itemPropDict = (IDictionary<string, object>)gameObjsDict[gameObjID];
            itemPropDict[propName] = prop;
        })).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SetPositionIterator", (object[] args) =>
        new ActionCommand(() => gameIteratorDict.Add("posIter", new GetPositionIterator((Vector)args[0], (Vector)args[1], (IEnumerable<int>)args[2])))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SetFuelIterator", (object[] args) =>
        new ActionCommand(() => gameIteratorDict.Add("fuelIter", new GetFuelIterator((IEnumerable<int>)args[0])))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateGameObjects", (object[] args) => new CreateGameObjectsStrategy().Strategy((int)args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameObjId", (object[] args) =>
        {
            var gameObjsID = new List<string>();
            for (int i = 1; i <= (int)args[0]; i++)
            {
                gameObjsID.Add(i.ToString());
            }
            return gameObjsID;
        }).Execute();


        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "PositionIteratorGetNext", (object[] args) =>
        {
            var posIter = (GetPositionIterator)gameIteratorDict["posIter"];
            posIter.MoveNext();
            return posIter.Current;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "FuelIteratorGetNext", (object[] args) =>
        {
            var fuelIter = (GetFuelIterator)gameIteratorDict["fuelIter"];
            fuelIter.MoveNext();
            return fuelIter.Current;
        }).Execute();
    }

    [Fact]
    public void InitializationGameCommandTest()
    {
        Queue<int> queuePos = new Queue<int>();
        foreach (int i in new int[6] { 0, 1, 2, 3, 4, 5 })
        {
            queuePos.Enqueue(i);
        }
        IEnumerable<int> fuel = new List<int>() { 100, 100, 100, 100, 100, 100 };

        IoC.Resolve<ICommand>("SetPositionIterator", new Vector(0, 0), new Vector(0, -1), queuePos).Execute();
        IoC.Resolve<ICommand>("SetFuelIterator", fuel).Execute();

        new InitGameCommand(6).Execute();

        var gameObj1 = (Dictionary<string, object>)gameObjsDict["1"];
        var gameObj4 = (Dictionary<string, object>)gameObjsDict["4"];
        var gameObj5 = (Dictionary<string, object>)gameObjsDict["5"];

        Assert.Equal(new Vector(0, 0), gameObj1["position"]);
        Assert.Equal(new Vector(1, -2), gameObj4["position"]);
        Assert.Equal(new Vector(1, -1), gameObj5["position"]);

        Assert.Equal(100, gameObj1["fuel"]);
        Assert.Equal(100, gameObj4["fuel"]);
        Assert.Equal(100, gameObj5["fuel"]);
    }
}

