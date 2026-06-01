using CarsApp.Domain.Entities;

namespace CarsApp.Domain.Lab7.State;

public class ServiceOrderWorkflow
{
    private readonly List<string> _history = new();
    private IServiceOrderState _state;

    public ServiceOrder Order { get; }

    public string CurrentStateName => _state.Name;

    public OrderWorkflowStatus CurrentStatus => _state.Status;

    public IReadOnlyList<string> History => _history.AsReadOnly();

    public ServiceOrderWorkflow(ServiceOrder order, IServiceOrderState initialState)
    {
        Order = order ?? throw new ArgumentNullException(nameof(order));
        _state = initialState ?? throw new ArgumentNullException(nameof(initialState));
        _state.SetContext(this);
        _history.Add($"Initial state: {_state.Name}");
    }

    public void TransitionTo(IServiceOrderState state, string reason)
    {
        if (state == null)
        {
            throw new ArgumentNullException(nameof(state));
        }

        var previousState = _state.Name;
        _state = state;
        _state.SetContext(this);
        _history.Add($"{previousState} -> {_state.Name}: {reason}");
    }

    public StateActionResult Schedule(DateTime scheduledDate)
    {
        return _state.Schedule(scheduledDate);
    }

    public StateActionResult StartWork(string technicianName)
    {
        return _state.StartWork(technicianName);
    }

    public StateActionResult WaitForParts(string reason)
    {
        return _state.WaitForParts(reason);
    }

    public StateActionResult Complete(string notes)
    {
        return _state.Complete(notes);
    }

    public StateActionResult Cancel(string reason)
    {
        return _state.Cancel(reason);
    }
}
