using CarsApp.Domain.Lab7.State;

namespace CarsApp.Infrastructure.Lab7.State;

public class CompletedOrderState : ServiceOrderStateBase
{
    public override string Name => "Completed";

    public override OrderWorkflowStatus Status => OrderWorkflowStatus.Completed;
}
