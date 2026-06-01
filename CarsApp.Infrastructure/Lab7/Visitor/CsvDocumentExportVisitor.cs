using System.Globalization;
using CarsApp.Domain.Lab7.Visitor;

namespace CarsApp.Infrastructure.Lab7.Visitor;

public class CsvDocumentExportVisitor : IDocumentExportVisitor
{
    private readonly List<string> _rows = new() { "Type,Field1,Field2,Field3,Field4" };

    public string Result => string.Join(Environment.NewLine, _rows);

    public void Visit(DocumentHeaderElement element)
    {
        _rows.Add(string.Join(
            ",",
            "Header",
            Escape(element.DocumentNumber),
            Escape(element.CustomerName),
            Escape(element.VehicleInfo),
            Escape(element.CreatedAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))));
    }

    public void Visit(ServiceLineElement element)
    {
        _rows.Add(string.Join(
            ",",
            "Service",
            Escape(element.ServiceName),
            Escape(element.Description),
            element.Price.ToString("0.00", CultureInfo.InvariantCulture),
            string.Empty));
    }

    public void Visit(PaymentSummaryElement element)
    {
        _rows.Add(string.Join(
            ",",
            "Payment",
            element.Subtotal.ToString("0.00", CultureInfo.InvariantCulture),
            element.VatAmount.ToString("0.00", CultureInfo.InvariantCulture),
            element.Total.ToString("0.00", CultureInfo.InvariantCulture),
            element.VatRate.ToString("P0", CultureInfo.InvariantCulture)));
    }

    private static string Escape(string value)
    {
        if (!value.Contains(',') &&
            !value.Contains('"') &&
            !value.Contains('\r') &&
            !value.Contains('\n'))
        {
            return value;
        }

        return $"\"{value.Replace("\"", "\"\"", StringComparison.Ordinal)}\"";
    }
}
