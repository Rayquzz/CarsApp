using CarsApp.Domain.Lab7.State;

namespace CarsApp.Infrastructure.Lab7.State;

public class InProgressOrderState : ServiceOrderStateBase
{
    public override string Name => "InProgress";

    public override OrderWorkflowStatus Status => OrderWorkflowStatus.InProgress;

    public override StateActionResult WaitForParts(string reason)
    {
        Context.Order.Notes = reason;

        return TransitionTo(
            new WaitingForPartsOrderState(),
            $"Work paused because parts are missing: {reason}");
    }

    public override StateActionResult Complete(string notes)
    {
        Context.Order.Notes = notes;

        return TransitionTo(
            new CompletedOrderState(),
            $"Order completed: {notes}");
    }

    public override StateActionResult Cancel(string reason)
    {
        Context.Order.Notes = reason;

        return TransitionTo(
            new CancelledOrderState(),
            $"In-progress order cancelled: {reason}");
    }
}
