namespace CarsApp.Domain.Lab7.Visitor;

public interface IExportableDocumentElement
{
    void Accept(IDocumentExportVisitor visitor);
}
