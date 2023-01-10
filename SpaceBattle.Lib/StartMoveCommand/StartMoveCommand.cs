using Hwdtech;

namespace SpaceBattle.Lib;

public class StartMoveCommand : ICommand
{
    IMoveCommandStartable obj;
    
    public StartMoveCommand(IMoveCommandStartable obj)
    {
        this.obj = obj;
    }

    public void Execute()
    {
        obj.property.ToList().ForEach(o => IoC.Resolve<ICommand>("SpaceBattle.SetProperty", obj.uobj, o.Key, o.Value).Execute());
        ICommand Mcommand = IoC.Resolve<ICommand>("SpaceBattle.Command.Movement", obj.uobj);
        IoC.Resolve<ICommand>("SpaceBattle.SetProperty", obj.uobj, "Command.Movement", Mcommand).Execute();
        IoC.Resolve<ICommand>("SpaceBattle.QueuePush", Mcommand).Execute();
    }
}
