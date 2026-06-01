using CarsApp.Domain.Entities;
using CarsApp.Domain.Lab7.TemplateMethod;

namespace CarsApp.Infrastructure.Lab7.TemplateMethod;

public class CustomerServiceReportTemplate : ServiceReportTemplate
{
    public override string ReportType => "Customer Service Report";

    protected override string BuildServiceDetailsSection(ServiceOrder order)
    {
        return $"Services completed: {string.Join(", ", order.Services)}";
    }

    protected override string BuildCostSection(ServiceOrder order)
    {
        return $"Amount to pay: {order.EstimatedCost:C}";
    }

    protected override string? BuildRecommendationSection(ServiceOrder order)
    {
        return order.Priority.Equals("High", StringComparison.OrdinalIgnoreCase)
            ? "Recommendation: schedule a follow-up inspection after 14 days."
            : null;
    }
}
