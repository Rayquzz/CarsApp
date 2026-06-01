using System.Globalization;
using System.Xml.Linq;
using CarsApp.Domain.Lab7.Visitor;

namespace CarsApp.Infrastructure.Lab7.Visitor;

public class XmlDocumentExportVisitor : IDocumentExportVisitor
{
    private readonly XElement _root = new("serviceDocument");

    public string Result => _root.ToString(SaveOptions.DisableFormatting);

    public void Visit(DocumentHeaderElement element)
    {
        _root.Add(new XElement(
            "header",
            new XAttribute("number", element.DocumentNumber),
            new XElement("customer", element.CustomerName),
            new XElement("vehicle", element.VehicleInfo),
            new XElement("createdAt", element.CreatedAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))));
    }

    public void Visit(ServiceLineElement element)
    {
        _root.Add(new XElement(
            "service",
            new XAttribute("price", element.Price.ToString("0.00", CultureInfo.InvariantCulture)),
            new XElement("name", element.ServiceName),
            new XElement("description", element.Description)));
    }

    public void Visit(PaymentSummaryElement element)
    {
        _root.Add(new XElement(
            "payment",
            new XElement("subtotal", element.Subtotal.ToString("0.00", CultureInfo.InvariantCulture)),
            new XElement("vatRate", element.VatRate.ToString("0.00", CultureInfo.InvariantCulture)),
            new XElement("vatAmount", element.VatAmount.ToString("0.00", CultureInfo.InvariantCulture)),
            new XElement("total", element.Total.ToString("0.00", CultureInfo.InvariantCulture))));
    }
}
