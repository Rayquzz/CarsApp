using CarsApp.Domain.Lab7.State;

namespace CarsApp.Infrastructure.Lab7.State;

public class WaitingForPartsOrderState : ServiceOrderStateBase
{
    public override string Name => "WaitingForParts";

    public override OrderWorkflowStatus Status => OrderWorkflowStatus.WaitingForParts;

    public override StateActionResult StartWork(string technicianName)
    {
        Context.Order.TechnicianName = technicianName;

        return TransitionTo(
            new InProgressOrderState(),
            $"Parts arrived. Technician {technicianName} resumed work.");
    }

    public override StateActionResult Cancel(string reason)
    {
        Context.Order.Notes = reason;

        return TransitionTo(
            new CancelledOrderState(),
            $"Order cancelled while waiting for parts: {reason}");
    }
}
