using CarsApp.Domain.Lab7.State;

namespace CarsApp.Infrastructure.Lab7.State;

public class CreatedOrderState : ServiceOrderStateBase
{
    public override string Name => "Created";

    public override OrderWorkflowStatus Status => OrderWorkflowStatus.Created;

    public override StateActionResult Schedule(DateTime scheduledDate)
    {
        Context.Order.ScheduledDate = scheduledDate;

        return TransitionTo(
            new ScheduledOrderState(),
            $"Order scheduled for {scheduledDate:dd.MM.yyyy HH:mm}.");
    }

    public override StateActionResult Cancel(string reason)
    {
        Context.Order.Notes = reason;

        return TransitionTo(
            new CancelledOrderState(),
            $"Order cancelled before scheduling: {reason}");
    }
}
