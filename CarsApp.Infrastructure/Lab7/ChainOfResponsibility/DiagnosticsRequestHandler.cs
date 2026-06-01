using CarsApp.Domain.Lab7.ChainOfResponsibility;

namespace CarsApp.Infrastructure.Lab7.ChainOfResponsibility;

public class DiagnosticsRequestHandler : ServiceRequestHandlerBase
{
    public override ServiceRequestResult? Handle(ServiceRequest request)
    {
        if (!request.NeedsManagerReview &&
            (request.Complexity == ServiceRequestComplexity.Medium ||
             ContainsAny(request.RequestedService, "Diagnostics", "Electric", "Battery")))
        {
            return new ServiceRequestResult(
                nameof(DiagnosticsRequestHandler),
                "Diagnostics",
                $"Diagnostics team scheduled checks for {request.RequestedService}.",
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
