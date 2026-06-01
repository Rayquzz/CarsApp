using CarsApp.Domain.Lab7.TemplateMethod;

namespace CarsApp.Infrastructure.Lab7.TemplateMethod;

public static class ServiceReportTemplateFactory
{
    public static IReadOnlyList<ServiceReportTemplate> CreateDefaultReports()
    {
        return new ServiceReportTemplate[]
        {
            new CustomerServiceReportTemplate(),
            new InternalWorkshopReportTemplate(),
            new FinancialServiceReportTemplate()
        };
    }
}
