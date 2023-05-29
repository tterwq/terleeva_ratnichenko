namespace SpaceBattle.Lib;

public interface IMessage
{
    public string type {get;}
    public string gameId {get;}
    public string gameItemId {get;}
    public IDictionary<string, object> properties {get;}
}
