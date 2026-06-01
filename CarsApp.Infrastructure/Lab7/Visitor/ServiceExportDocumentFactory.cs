using CarsApp.Domain.Lab7.Visitor;

namespace CarsApp.Infrastructure.Lab7.Visitor;

public static class ServiceExportDocumentFactory
{
    public static ServiceExportDocument CreateSampleInvoice()
    {
        return new ServiceExportDocument()
            .Add(new DocumentHeaderElement(
                "DOC-2026-001",
                "Ion Popescu",
                "Toyota Camry 2020",
                new DateTime(2026, 5, 12)))
            .Add(new ServiceLineElement("Oil Change", "Oil and filter replacement", 150m))
            .Add(new ServiceLineElement("Brake Repair", "Front brake pad replacement", 250m))
            .Add(new PaymentSummaryElement(400m, 0.19m));
    }
}
