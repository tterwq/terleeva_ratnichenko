using Hwdtech;
namespace SpaceBattle.Lib.Test;

public class IteratorsTests
{
    public IteratorsTests()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void PositionIteratorPositiveTest()
    {
        Queue<int> queuePos = new Queue<int>();
        foreach (int i in new int[4] { 0, 1, 2, 3 })
        {
            queuePos.Enqueue(i);
        }
        var iterator = new GetPositionIterator(new Vector(0, 0), new Vector(0, -1), queuePos);
        iterator.MoveNext();
        Assert.Equal(new Vector(0, 0), iterator.Current);
        iterator.MoveNext();
        Assert.Equal(new Vector(0, -1), iterator.Current);
        iterator.MoveNext();
        Assert.Equal(new Vector(1, -1), iterator.Current);
        iterator.MoveNext();
        Assert.Equal(new Vector(1, 0), iterator.Current);

        Assert.Throws<System.NotImplementedException>(() => iterator.Reset());
    }

    [Fact]
    public void FuelIteratorTest()
    {
        var fuel = new List<int>() { 10, 20, 30 };

        var iterator = new GetFuelIterator(fuel);
        iterator.MoveNext();
        Assert.Equal(10, iterator.Current);
        iterator.MoveNext();
        Assert.Equal(20, iterator.Current);
        iterator.MoveNext();
        Assert.Equal(30, iterator.Current);

        Assert.Throws<System.NotImplementedException>(() => iterator.Reset());
    }
    [Fact]
    public void PositionIteratorNegativeTest()
    {
        Assert.Throws<Exception>(() => new GetPositionIterator(new Vector(0, 0), new Vector(0, -1), new List<int>() { 0, 1, 2 }));
    }
}
