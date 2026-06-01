namespace CarsApp.Domain.Lab7.Visitor;

public class ServiceLineElement : IExportableDocumentElement
{
    public string ServiceName { get; }

    public string Description { get; }

    public decimal Price { get; }

    public ServiceLineElement(string serviceName, string description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            throw new ArgumentException("Service name is required.", nameof(serviceName));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
        }

        ServiceName = serviceName;
        Description = description ?? string.Empty;
        Price = price;
    }

    public void Accept(IDocumentExportVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        visitor.Visit(this);
    }
}
