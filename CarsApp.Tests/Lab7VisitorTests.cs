using CarsApp.Domain.Lab7.Visitor;
using CarsApp.Infrastructure.Lab7.Visitor;
using Xunit;

namespace CarsApp.Tests;

public class Lab7VisitorTests
{
    [Fact]
    public void Visitor_CsvExporter_VisitsAllDocumentElements()
    {
        var document = ServiceExportDocumentFactory.CreateSampleInvoice();
        var visitor = new CsvDocumentExportVisitor();

        var result = document.Export(visitor);

        Assert.Contains("Type,Field1,Field2,Field3,Field4", result);
        Assert.Contains("Header,DOC-2026-001,Ion Popescu,Toyota Camry 2020,2026-05-12", result);
        Assert.Contains("Service,Oil Change,Oil and filter replacement,150.00,", result);
        Assert.Contains("Payment,400.00,76.00,476.00,19 %", result);
    }

    [Fact]
    public void Visitor_XmlExporter_AppliesDifferentOperationOnSameStructure()
    {
        var document = ServiceExportDocumentFactory.CreateSampleInvoice();
        var visitor = new XmlDocumentExportVisitor();

        var result = document.Export(visitor);

        Assert.StartsWith("<serviceDocument>", result);
        Assert.Contains("<header number=\"DOC-2026-001\">", result);
        Assert.Contains("<service price=\"250.00\">", result);
        Assert.Contains("<total>476.00</total>", result);
    }

    [Fact]
    public void Visitor_PdfExporter_AddsNewExportBehaviorWithoutChangingDocumentElements()
    {
        var document = ServiceExportDocumentFactory.CreateSampleInvoice();
        var visitor = new PdfDocumentExportVisitor();

        var result = document.Export(visitor);

        Assert.StartsWith("%PDF-CarsApp-ServiceDocument", result);
        Assert.Contains("Customer: Ion Popescu", result);
        Assert.Contains("- Brake Repair: Front brake pad replacement (250.00)", result);
        Assert.EndsWith("%%EOF", result);
    }

    [Fact]
    public void Visitor_ClientWorksWithAnyVisitorThroughBaseInterface()
    {
        var document = ServiceExportDocumentFactory.CreateSampleInvoice();
        var visitors = new IDocumentExportVisitor[]
        {
            new CsvDocumentExportVisitor(),
            new XmlDocumentExportVisitor(),
            new PdfDocumentExportVisitor()
        };

        var exports = visitors
            .Select(document.Export)
            .ToList();

        Assert.Equal(3, exports.Count);
        Assert.All(exports, export => Assert.Contains("DOC-2026-001", export));
    }

    [Fact]
    public void Visitor_ElementsDispatchToSpecificVisitorMethod()
    {
        var visitor = new CountingDocumentExportVisitor();
        var document = new ServiceExportDocument()
            .Add(new DocumentHeaderElement("DOC-1", "Maria Ionescu", "Ford Focus 2018", DateTime.Today))
            .Add(new ServiceLineElement("Diagnostics", "Electronic diagnostics", 120m))
            .Add(new PaymentSummaryElement(120m, 0.19m));

        document.Export(visitor);

        Assert.Equal(1, visitor.HeadersVisited);
        Assert.Equal(1, visitor.ServicesVisited);
        Assert.Equal(1, visitor.PaymentsVisited);
    }

    private class CountingDocumentExportVisitor : IDocumentExportVisitor
    {
        public int HeadersVisited { get; private set; }

        public int ServicesVisited { get; private set; }

        public int PaymentsVisited { get; private set; }

        public string Result => "Counting complete";

        public void Visit(DocumentHeaderElement element)
        {
            HeadersVisited++;
        }

        public void Visit(ServiceLineElement element)
        {
            ServicesVisited++;
        }

        public void Visit(PaymentSummaryElement element)
        {
            PaymentsVisited++;
        }
    }
}
