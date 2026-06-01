using CarsApp.Domain.Builder;
using CarsApp.Domain.Entities;
using CarsApp.Domain.Lab7.TemplateMethod;
using CarsApp.Infrastructure.Lab7.TemplateMethod;
using Xunit;

namespace CarsApp.Tests;

public class Lab7TemplateMethodTests
{
    [Fact]
    public void TemplateMethod_CustomerReport_UsesCommonStructureAndCustomSections()
    {
        var order = CreateOrder(priority: "High");
        ServiceReportTemplate reportTemplate = new CustomerServiceReportTemplate();

        var report = reportTemplate.Generate(order);

        Assert.Equal("Customer Service Report", report.ReportType);
        Assert.StartsWith("Customer Service Report | Order", report.Sections[0]);
        Assert.Contains("Vehicle: Car Toyota Camry 2020", report.Content);
        Assert.Contains("Services completed: Oil Change, Brake Repair", report.Content);
        Assert.Contains("Amount to pay", report.Content);
        Assert.Contains("follow-up inspection", report.Content);
    }

    [Fact]
    public void TemplateMethod_InternalReport_OverridesHookAndFooter()
    {
        var order = CreateOrder(notes: "Customer requested urgent delivery.");
        ServiceReportTemplate reportTemplate = new InternalWorkshopReportTemplate();

        var report = reportTemplate.Generate(order);

        Assert.Contains("Technician: Maria Ionescu", report.Content);
        Assert.Contains("Workshop notes: Customer requested urgent delivery.", report.Content);
        Assert.EndsWith("Verify parts and labor before closing the order.", report.Sections[^1]);
    }

    [Fact]
    public void TemplateMethod_FinancialReport_ReusesSkeletonWithDifferentCostStep()
    {
        var order = CreateOrder(estimatedCost: 1190m);
        ServiceReportTemplate reportTemplate = new FinancialServiceReportTemplate();

        var report = reportTemplate.Generate(order);

        Assert.Equal("Financial Service Report", report.ReportType);
        Assert.Contains("Billable services:", report.Content);
        Assert.Contains("VAT (19 %)", report.Content);
        Assert.Contains("Total:", report.Content);
        Assert.Equal(5, report.Sections.Count);
    }

    [Fact]
    public void TemplateMethod_ClientCanGenerateAllReportsThroughBaseClass()
    {
        var order = CreateOrder();
        var reports = ServiceReportTemplateFactory
            .CreateDefaultReports()
            .Select(template => template.Generate(order))
            .ToList();

        Assert.Equal(3, reports.Count);
        Assert.Contains(reports, report => report.ReportType == "Customer Service Report");
        Assert.Contains(reports, report => report.ReportType == "Internal Workshop Report");
        Assert.Contains(reports, report => report.ReportType == "Financial Service Report");
        Assert.All(reports, report => Assert.Contains("Vehicle: Car Toyota Camry 2020", report.Content));
    }

    [Fact]
    public void TemplateMethod_ValidationRejectsOrderWithoutServices()
    {
        var builder = new ServiceOrderBuilder();
        builder
            .ForVehicle(new Car("Toyota", "Camry", 2020))
            .WithEstimatedCost(100m);
        var order = builder.GetProduct();

        var reportTemplate = new CustomerServiceReportTemplate();

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            reportTemplate.Generate(order);
        });
        Assert.Contains("at least one service", exception.Message);
    }

    private static ServiceOrder CreateOrder(
        string priority = "Standard",
        string notes = "Template Method test order",
        decimal estimatedCost = 600m)
    {
        var builder = new ServiceOrderBuilder();

        builder
            .ForVehicle(new Car("Toyota", "Camry", 2020))
            .AddService("Oil Change")
            .AddService("Brake Repair")
            .WithPriority(priority)
            .WithTechnician("Maria Ionescu")
            .ScheduledOn(new DateTime(2026, 5, 14, 10, 30, 0))
            .WithLoanCar(true)
            .WithEstimatedCost(estimatedCost)
            .WithNotes(notes);

        return builder.GetProduct();
    }
}
