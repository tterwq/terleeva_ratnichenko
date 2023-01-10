namespace SpaceBattle.Lib;
public interface IMoveCommandEndable
{
    IUObject uobj
    { 
        get; 
    }
    IEnumerable<string> property 
    { 
        get; 
    }
}
