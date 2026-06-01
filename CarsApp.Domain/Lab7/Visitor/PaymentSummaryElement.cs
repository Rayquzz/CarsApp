namespace CarsApp.Domain.Lab7.Visitor;

public class PaymentSummaryElement : IExportableDocumentElement
{
    public decimal Subtotal { get; }

    public decimal VatRate { get; }

    public decimal VatAmount => Math.Round(Subtotal * VatRate, 2);

    public decimal Total => Subtotal + VatAmount;

    public PaymentSummaryElement(decimal subtotal, decimal vatRate)
    {
        if (subtotal < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subtotal), "Subtotal cannot be negative.");
        }

        if (vatRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(vatRate), "VAT rate cannot be negative.");
        }

        Subtotal = subtotal;
        VatRate = vatRate;
    }

    public void Accept(IDocumentExportVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        visitor.Visit(this);
    }
}
