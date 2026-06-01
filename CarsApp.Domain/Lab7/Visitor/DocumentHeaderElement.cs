namespace CarsApp.Domain.Lab7.Visitor;

public class DocumentHeaderElement : IExportableDocumentElement
{
    public string DocumentNumber { get; }

    public string CustomerName { get; }

    public string VehicleInfo { get; }

    public DateTime CreatedAt { get; }

    public DocumentHeaderElement(
        string documentNumber,
        string customerName,
        string vehicleInfo,
        DateTime createdAt)
    {
        DocumentNumber = RequireValue(documentNumber, nameof(documentNumber));
        CustomerName = RequireValue(customerName, nameof(customerName));
        VehicleInfo = RequireValue(vehicleInfo, nameof(vehicleInfo));
        CreatedAt = createdAt;
    }

    public void Accept(IDocumentExportVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        visitor.Visit(this);
    }

    private static string RequireValue(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", parameterName);
        }

        return value;
    }
}
