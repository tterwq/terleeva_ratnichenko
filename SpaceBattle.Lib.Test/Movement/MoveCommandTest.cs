namespace SpaceBattle.Lib.Test;

public class MoveCommandTest 
{
    [Fact]
    public void TestPositiveMove()
    {
        Mock<IMovable> movableMock = new Mock<IMovable>();
        movableMock.SetupProperty(m => m.Position, new Vector(12, 5));
        movableMock.SetupGet<Vector>(m => m.Velocity).Returns(new Vector(-7, 3));
        
        ICommand command = new MoveCommand(movableMock.Object);
        command.Execute();

        movableMock.VerifySet(m => m.Position = new Vector(5, 8));
    }

    [Fact]
    public void TestPositionError()
    {
        Mock<IMovable> movableMock = new Mock<IMovable>();
        movableMock.SetupGet(m => m.Position).Throws<Exception>();
        movableMock.SetupGet<Vector>(m => m.Velocity).Returns(new Vector(-7, 3));
        
        ICommand command = new MoveCommand(movableMock.Object);

        Assert.Throws<Exception>(() => command.Execute());
    }

    [Fact]
    public void TestVelocityError()
    {
        Mock<IMovable> movableMock = new Mock<IMovable>();
        movableMock.SetupProperty(m => m.Position, new Vector(12, 5));
        movableMock.SetupGet<Vector>(m => m.Velocity).Throws<Exception>();
        
        ICommand command = new MoveCommand(movableMock.Object);
        
        Assert.Throws<Exception>(() => command.Execute());
    }

    [Fact]
    public void TestChPositionError()
    {
        Mock<IMovable> movableMock = new Mock<IMovable>();
        movableMock.SetupProperty(m => m.Position, new Vector(12, 5));
        movableMock.SetupSet(m => m.Position = It.IsAny<Vector>()).Throws<Exception>();
        movableMock.SetupGet<Vector>(m => m.Velocity).Returns(new Vector(-7, 3));

        ICommand command = new MoveCommand(movableMock.Object);
        
        Assert.Throws<Exception>(() => command.Execute());
    }
}
