namespace SpaceBattle.Lib.Test;

public class RotateCommandTest 
{
    [Fact]
    public void TestPositiveRotate()
    {
        Mock<IRotatable> rotatableMock = new Mock<IRotatable>();
        rotatableMock.SetupProperty(r => r.angleDirection, new Angle(45, 1));
        rotatableMock.SetupGet<Angle>(r => r.angularVelocity).Returns(new Angle(90, 1));
        
        ICommand command = new RotateCommand(rotatableMock.Object);
        command.Execute();

        rotatableMock.VerifySet(r => r.angleDirection = new Angle(135, 1));
    }

    [Fact]
    public void TestAngleDirectionError()
    {
        Mock<IRotatable> rotatableMock = new Mock<IRotatable>();
        rotatableMock.SetupGet(r => r.angleDirection).Throws<Exception>();
        rotatableMock.SetupGet<Angle>(r => r.angularVelocity).Returns(new Angle(90, 1));
        
        ICommand command = new RotateCommand(rotatableMock.Object);

        Assert.Throws<Exception>(() => command.Execute());
    }

    [Fact]
    public void TestAngularVelocityError()
    {
        Mock<IRotatable> rotatableMock = new Mock<IRotatable>();
        rotatableMock.SetupProperty(r => r.angleDirection, new Angle(45, 1));
        rotatableMock.SetupGet<Angle>(r => r.angularVelocity).Throws<Exception>();
        
        ICommand command = new RotateCommand(rotatableMock.Object);
        
        Assert.Throws<Exception>(() => command.Execute());
    }

    [Fact]
    public void TestChAngularVelocityError()
    {
        Mock<IRotatable> rotatableMock = new Mock<IRotatable>();
        rotatableMock.SetupProperty(r => r.angleDirection, new Angle(45, 1));
        rotatableMock.SetupSet<Angle>(r => r.angleDirection = It.IsAny<Angle>()).Throws<Exception>();
        rotatableMock.SetupGet<Angle>(r => r.angularVelocity).Returns(new Angle(90, 1));
        
        ICommand command = new RotateCommand(rotatableMock.Object);
        
        Assert.Throws<Exception>(() => command.Execute());
    }
}
