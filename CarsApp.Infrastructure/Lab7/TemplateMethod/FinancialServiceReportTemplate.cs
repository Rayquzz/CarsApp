using CarsApp.Domain.Entities;
using CarsApp.Domain.Lab7.TemplateMethod;

namespace CarsApp.Infrastructure.Lab7.TemplateMethod;

public class FinancialServiceReportTemplate : ServiceReportTemplate
{
    private const decimal VatRate = 0.19m;

    public override string ReportType => "Financial Service Report";

    protected override string BuildServiceDetailsSection(ServiceOrder order)
    {
        var serviceCount = order.Services.Count;
        var costPerService = serviceCount == 0
            ? 0m
            : Math.Round(order.EstimatedCost / serviceCount, 2);

        var serviceRows = order.Services.Select(service => $"{service}: {costPerService:C}");
        return $"Billable services:{Environment.NewLine}{string.Join(Environment.NewLine, serviceRows)}";
    }

    protected override string BuildCostSection(ServiceOrder order)
    {
        var subtotal = Math.Round(order.EstimatedCost / (1 + VatRate), 2);
        var vat = order.EstimatedCost - subtotal;

        return
            $"Subtotal: {subtotal:C}{Environment.NewLine}" +
            $"VAT ({VatRate:P0}): {vat:C}{Environment.NewLine}" +
            $"Total: {order.EstimatedCost:C}";
    }

    protected override string BuildFooter(ServiceOrder order)
    {
        return "Financial report generated for billing and accounting.";
    }
}
