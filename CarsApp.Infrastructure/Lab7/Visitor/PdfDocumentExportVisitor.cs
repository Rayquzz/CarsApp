using System.Globalization;
using System.Text;
using CarsApp.Domain.Lab7.Visitor;

namespace CarsApp.Infrastructure.Lab7.Visitor;

public class PdfDocumentExportVisitor : IDocumentExportVisitor
{
    private readonly StringBuilder _builder = new();

    public string Result => _builder.ToString().TrimEnd();

    public void Visit(DocumentHeaderElement element)
    {
        _builder
            .AppendLine("%PDF-CarsApp-ServiceDocument")
            .AppendLine($"Document: {element.DocumentNumber}")
            .AppendLine($"Customer: {element.CustomerName}")
            .AppendLine($"Vehicle: {element.VehicleInfo}")
            .AppendLine($"Date: {element.CreatedAt:dd.MM.yyyy}")
            .AppendLine();
    }

    public void Visit(ServiceLineElement element)
    {
        _builder.AppendLine(
            $"- {element.ServiceName}: {element.Description} ({element.Price.ToString("0.00", CultureInfo.InvariantCulture)})");
    }

    public void Visit(PaymentSummaryElement element)
    {
        _builder
            .AppendLine()
            .AppendLine($"Subtotal: {element.Subtotal.ToString("0.00", CultureInfo.InvariantCulture)}")
            .AppendLine($"VAT: {element.VatAmount.ToString("0.00", CultureInfo.InvariantCulture)}")
            .AppendLine($"Total: {element.Total.ToString("0.00", CultureInfo.InvariantCulture)}")
            .AppendLine("%%EOF");
    }
}
