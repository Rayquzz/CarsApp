namespace CarsApp.Domain.Lab7.Visitor;

public interface IDocumentExportVisitor
{
    string Result { get; }

    void Visit(DocumentHeaderElement element);

    void Visit(ServiceLineElement element);

    void Visit(PaymentSummaryElement element);
}
