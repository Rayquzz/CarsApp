namespace CarsApp.Domain.Lab7.State;

public enum OrderWorkflowStatus
{
    Created,
    Scheduled,
    InProgress,
    WaitingForParts,
    Completed,
    Cancelled
}
