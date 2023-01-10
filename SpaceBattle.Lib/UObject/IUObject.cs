namespace SpaceBattle.Lib;

public interface IUObject
{
    object getProperty(string key);
    void setProperty(string key, object value);
}
