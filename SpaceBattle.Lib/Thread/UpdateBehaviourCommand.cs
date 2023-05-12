namespace SpaceBattle.Lib;

public class UpdateBehaviourCommand : ICommand
{
    Action behaviour;
    MyThread thread;

    public UpdateBehaviourCommand(MyThread thread, Action newBehaviour)
    {
        this.behaviour = newBehaviour;
        this.thread = thread;
    }
    public void Execute()
    {
        thread.UpdateBehaviour(behaviour);
    }
}
