using Hwdtech;

namespace SpaceBattle.Lib;

public class CollisionCheckCommand : ICommand
{
    private IUObject obj1, obj2;

    public CollisionCheckCommand(IUObject obj1, IUObject obj2)
    {
        this.obj1 = obj1;
        this.obj2 = obj2;
    }

    public void Execute()
    {
        var firstPos = IoC.Resolve<Vector>("SpaceBattle.GetProperty", "Position", obj1);
        var secondPos = IoC.Resolve<Vector>("SpaceBattle.GetProperty", "Position", obj2);
        var firstVel = IoC.Resolve<Vector>("SpaceBattle.GetProperty", "Velocity", obj1);
        var secondVel = IoC.Resolve<Vector>("SpaceBattle.GetProperty", "Velocity", obj2);

        bool checkCollision = IoC.Resolve<bool>("SpaceBattle.CheckCollision", firstPos - secondPos, firstVel - secondVel);

        if (checkCollision) throw new Exception();
    }
}
