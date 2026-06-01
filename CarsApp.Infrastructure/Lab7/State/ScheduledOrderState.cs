using CarsApp.Domain.Lab7.State;

namespace CarsApp.Infrastructure.Lab7.State;

public class ScheduledOrderState : ServiceOrderStateBase
{
    public override string Name => "Scheduled";

    public override OrderWorkflowStatus Status => OrderWorkflowStatus.Scheduled;

    public override StateActionResult StartWork(string technicianName)
    {
        Context.Order.TechnicianName = technicianName;

        return TransitionTo(
            new InProgressOrderState(),
            $"Technician {technicianName} started work.");
    }

    public override StateActionResult Cancel(string reason)
    {
        Context.Order.Notes = reason;

        return TransitionTo(
            new CancelledOrderState(),
            $"Scheduled order cancelled: {reason}");
    }
}
