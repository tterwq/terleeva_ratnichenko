namespace SpaceBattle.Lib;
using Hwdtech;

public class CreateGameCommandFromMessageStrategy : IStrategy  // "Game.Command.Create.FromMessage"
{
    public object Strategy(params object[] args)
    {
        IInterpretingMessage message = (IInterpretingMessage)args[0];

        var obj = IoC.Resolve<IUObject>("Game.Get.UObject", message.ObjectID);

        message.Parameters.ToList().ForEach(x => obj.setProperty(x.Key, x.Value));
        
        return IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Command." + message.TypeCommand, obj);
    }
}
