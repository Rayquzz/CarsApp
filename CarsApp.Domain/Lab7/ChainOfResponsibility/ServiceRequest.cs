namespace CarsApp.Domain.Lab7.ChainOfResponsibility;

public record ServiceRequest(
    string CustomerName,
    string VehicleInfo,
    string RequestedService,
    ServiceRequestComplexity Complexity,
    decimal EstimatedCost,
    bool IsVipCustomer = false,
    bool RequiresManagerApproval = false)
{
    public bool NeedsManagerReview =>
        IsVipCustomer ||
        RequiresManagerApproval ||
        EstimatedCost >= 1000m;
}
