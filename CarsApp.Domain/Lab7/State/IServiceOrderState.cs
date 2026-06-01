namespace CarsApp.Domain.Lab7.State;

public interface IServiceOrderState
{
    string Name { get; }

    OrderWorkflowStatus Status { get; }

    void SetContext(ServiceOrderWorkflow context);

    StateActionResult Schedule(DateTime scheduledDate);

    StateActionResult StartWork(string technicianName);

    StateActionResult WaitForParts(string reason);

    StateActionResult Complete(string notes);

    StateActionResult Cancel(string reason);
}
