namespace SpaceBattle.Lib;

public interface IInterpretingMessage
{
    public int GameID { get; }
    public int ObjectID { get; }
    public string TypeCommand { get; }
    public IDictionary<string, object> Parameters { get; }
}
