using CarsApp.Domain.Lab7.ChainOfResponsibility;

namespace CarsApp.Infrastructure.Lab7.ChainOfResponsibility;

public class ReceptionRequestHandler : ServiceRequestHandlerBase
{
    public override ServiceRequestResult? Handle(ServiceRequest request)
    {
        if (request.Complexity == ServiceRequestComplexity.Low &&
            request.EstimatedCost <= 300m &&
            !request.NeedsManagerReview)
        {
            return new ServiceRequestResult(
                nameof(ReceptionRequestHandler),
                "Reception",
                $"Reception accepted {request.RequestedService} for {request.VehicleInfo}.",
                RequiresFollowUp: false);
        }

        return base.Handle(request);
    }
}
