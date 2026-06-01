using CarsApp.Domain.Lab7.ChainOfResponsibility;

namespace CarsApp.Infrastructure.Lab7.ChainOfResponsibility;

public class ManagerApprovalRequestHandler : ServiceRequestHandlerBase
{
    public override ServiceRequestResult? Handle(ServiceRequest request)
    {
        if (request.NeedsManagerReview)
        {
            return new ServiceRequestResult(
                nameof(ManagerApprovalRequestHandler),
                "Service Manager",
                $"Manager approval required for {request.CustomerName}: {request.RequestedService}.",
                RequiresFollowUp: true);
        }

        return base.Handle(request);
    }
}
