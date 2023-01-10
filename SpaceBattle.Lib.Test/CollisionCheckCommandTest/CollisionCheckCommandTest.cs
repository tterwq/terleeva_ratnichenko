using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class CollisionCheckTest
{
    public CollisionCheckTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var getStrategy = new GetPropertyStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.GetProperty", (object[] args) => getStrategy.Strategy(args)).Execute();
    }

    [Fact]
    public void TestPositiveCollisionCheck()
    {
        Mock<IUObject> obj1 = new Mock<IUObject>();
        Mock<IUObject> obj2 = new Mock<IUObject>();

        foreach (string property in new List<string>() { "Position", "Velocity" })
        {
            obj1.Setup(x => x.getProperty(property)).Returns(new Vector(It.IsAny<int>(), It.IsAny<int>()));
            obj2.Setup(x => x.getProperty(property)).Returns(new Vector(It.IsAny<int>(), It.IsAny<int>()));
        }

        Mock<IStrategy> collisionMock = new Mock<IStrategy>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.CheckCollision", (object[] args) => collisionMock.Object.Strategy(args)).Execute();
        collisionMock.Setup(c => c.Strategy(It.IsAny<object[]>())).Returns(true).Verifiable();

        var collision_check = new CollisionCheckCommand(obj1.Object, obj2.Object);

        Assert.Throws<Exception>(() => collision_check.Execute());
    }

    [Fact]
    public void TestNegativeCollisionCheck()
    {
        Mock<IUObject> obj1 = new Mock<IUObject>();
        Mock<IUObject> obj2 = new Mock<IUObject>();

        foreach (string property in new List<string>() { "Position", "Velocity" })
        {
            obj1.Setup(x => x.getProperty(property)).Returns(new Vector(It.IsAny<int>(), It.IsAny<int>()));
            obj2.Setup(x => x.getProperty(property)).Returns(new Vector(It.IsAny<int>(), It.IsAny<int>()));
        }

        Mock<IStrategy> collisionMock = new Mock<IStrategy>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SpaceBattle.CheckCollision", (object[] args) => collisionMock.Object.Strategy(args)).Execute();
        collisionMock.Setup(c => c.Strategy(It.IsAny<object[]>())).Returns(false).Verifiable();

        var collision_check = new CollisionCheckCommand(obj1.Object, obj2.Object);
        collision_check.Execute();

        collisionMock.Verify();
    }
}
