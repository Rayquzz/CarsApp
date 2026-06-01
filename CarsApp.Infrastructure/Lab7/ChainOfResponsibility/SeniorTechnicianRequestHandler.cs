using CarsApp.Domain.Lab7.ChainOfResponsibility;

namespace CarsApp.Infrastructure.Lab7.ChainOfResponsibility;

public class SeniorTechnicianRequestHandler : ServiceRequestHandlerBase
{
    public override ServiceRequestResult? Handle(ServiceRequest request)
    {
        if (!request.NeedsManagerReview &&
            (request.Complexity is ServiceRequestComplexity.High or ServiceRequestComplexity.Critical ||
             ContainsAny(request.RequestedService, "Engine", "Brake", "Transmission")))
        {
            return new ServiceRequestResult(
                nameof(SeniorTechnicianRequestHandler),
                "Senior Technician",
                $"Senior technician assigned to {request.RequestedService}.",
                RequiresFollowUp: true);
        }

        return base.Handle(request);
    }

    private static bool ContainsAny(string value, params string[] keywords)
    {
        return keywords.Any(keyword =>
            value.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }
}
