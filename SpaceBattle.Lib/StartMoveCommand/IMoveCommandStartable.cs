namespace SpaceBattle.Lib;

public interface IMoveCommandStartable
{
    IUObject uobj 
    { 
        get; 
    }

    IDictionary<string, object> property
    {
        get;
    }
}
