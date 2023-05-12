namespace SpaceBattle.Lib;

public class SendCommandStrategy : IStrategy
{
    public object Strategy(params object[] args)
    {
        string id = (string)args[0];
        ICommand cmd = (ICommand)args[1];
        ISender s = IoC.Resolve<ISender>("Get Sender", id);

        Action act = () => 
        {
            s.Send(cmd);
        };

        return new ActionCommand(act);
    }
}
