namespace CarsApp.Domain.Lab7.State;

public abstract class ServiceOrderStateBase : IServiceOrderState
{
    private ServiceOrderWorkflow? _context;

    protected ServiceOrderWorkflow Context =>
        _context ?? throw new InvalidOperationException("State context was not assigned.");

    public abstract string Name { get; }

    public abstract OrderWorkflowStatus Status { get; }

    public void SetContext(ServiceOrderWorkflow context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public virtual StateActionResult Schedule(DateTime scheduledDate)
    {
        return Invalid("schedule");
    }

    public virtual StateActionResult StartWork(string technicianName)
    {
        return Invalid("start work");
    }

    public virtual StateActionResult WaitForParts(string reason)
    {
        return Invalid("wait for parts");
    }

    public virtual StateActionResult Complete(string notes)
    {
        return Invalid("complete");
    }

    public virtual StateActionResult Cancel(string reason)
    {
        return Invalid("cancel");
    }

    protected StateActionResult TransitionTo(IServiceOrderState nextState, string message)
    {
        var fromState = Name;
        Context.TransitionTo(nextState, message);

        return new StateActionResult(
            Success: true,
            FromState: fromState,
            ToState: nextState.Name,
            Message: message);
    }

    protected StateActionResult Invalid(string action)
    {
        return new StateActionResult(
            Success: false,
            FromState: Name,
            ToState: Name,
            Message: $"Cannot {action} while order is {Name}.");
    }
}
