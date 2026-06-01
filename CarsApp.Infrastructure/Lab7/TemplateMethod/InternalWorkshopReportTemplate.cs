using CarsApp.Domain.Entities;
using CarsApp.Domain.Lab7.TemplateMethod;

namespace CarsApp.Infrastructure.Lab7.TemplateMethod;

public class InternalWorkshopReportTemplate : ServiceReportTemplate
{
    public override string ReportType => "Internal Workshop Report";

    protected override string BuildServiceDetailsSection(ServiceOrder order)
    {
        var technician = string.IsNullOrWhiteSpace(order.TechnicianName)
            ? "Unassigned"
            : order.TechnicianName;

        return
            $"Technician: {technician}{Environment.NewLine}" +
            $"Priority: {order.Priority}{Environment.NewLine}" +
            $"Scheduled: {order.ScheduledDate:dd.MM.yyyy HH:mm}{Environment.NewLine}" +
            $"Work items: {string.Join(" | ", order.Services)}";
    }

    protected override string BuildCostSection(ServiceOrder order)
    {
        return
            $"Internal estimate: {order.EstimatedCost:C}{Environment.NewLine}" +
            $"Loan car included: {(order.IncludesLoanCar ? "Yes" : "No")}";
    }

    protected override string? BuildRecommendationSection(ServiceOrder order)
    {
        return string.IsNullOrWhiteSpace(order.Notes)
            ? null
            : $"Workshop notes: {order.Notes}";
    }

    protected override string BuildFooter(ServiceOrder order)
    {
        return "Internal use only. Verify parts and labor before closing the order.";
    }
}
