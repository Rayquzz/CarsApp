namespace CarsApp.Domain.Lab7.Visitor;

public class ServiceExportDocument
{
    private readonly List<IExportableDocumentElement> _elements = new();

    public IReadOnlyList<IExportableDocumentElement> Elements => _elements.AsReadOnly();

    public ServiceExportDocument Add(IExportableDocumentElement element)
    {
        _elements.Add(element ?? throw new ArgumentNullException(nameof(element)));
        return this;
    }

    public string Export(IDocumentExportVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        foreach (var element in _elements)
        {
            element.Accept(visitor);
        }

        return visitor.Result;
    }
}
