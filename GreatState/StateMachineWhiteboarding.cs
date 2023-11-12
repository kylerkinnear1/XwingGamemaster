namespace GreatState;

// Somehow, return the next step?
public interface IStep
{
    Task<IStep> Run();
}

public class Step<TContext> : IStep
{
    public Task<IStep> Run()
    {
        throw new NotImplementedException();
    }
}

public class EventBus
{

}

public class StateMachine
{
    private readonly IEnumerable<IStep> _steps;
}
