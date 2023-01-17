using Hwdtech;
namespace SpaceBattle.Lib;

public class EndMoveCommand : ICommand
{
    IMoveCommandEndable obj;

    public EndMoveCommand(IMoveCommandEndable obj)
    {
        this.obj = obj;
    }

    public void Execute()
    {
        obj.property.ToList().ForEach(p => IoC.Resolve<ICommand>("SpaceBattle.RemoveProperty", obj.uobj, p).Execute());
        IoC.Resolve<IInjectable>("SpaceBattle.Command.SetupCommand", obj.uobj).Inject(IoC.Resolve<ICommand>("SpaceBattle.Command.Empty"));
    }
}
