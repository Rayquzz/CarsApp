using CarsApp.Domain.Lab7.State;

namespace CarsApp.Infrastructure.Lab7.State;

public class CancelledOrderState : ServiceOrderStateBase
{
    public override string Name => "Cancelled";

    public override OrderWorkflowStatus Status => OrderWorkflowStatus.Cancelled;
}
