using CarsApp.Domain.Entities;
using CarsApp.Domain.Lab7.State;

namespace CarsApp.Infrastructure.Lab7.State;

public static class ServiceOrderWorkflowFactory
{
    public static ServiceOrderWorkflow Create(ServiceOrder order)
    {
        return new ServiceOrderWorkflow(order, new CreatedOrderState());
    }
}
