namespace SpaceBattle.Lib;

public interface IRotatable
{
    Angle angleDirection
    {
        get;
        set;
    }

    Angle angularVelocity
    {
        get;
    }
}
