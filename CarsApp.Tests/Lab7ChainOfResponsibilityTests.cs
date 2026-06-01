using CarsApp.Domain.Lab7.ChainOfResponsibility;
using CarsApp.Infrastructure.Lab7.ChainOfResponsibility;
using Xunit;

namespace CarsApp.Tests;

public class Lab7ChainOfResponsibilityTests
{
    [Fact]
    public void Chain_ReceptionHandlesSimpleLowCostRequest()
    {
        var chain = ServiceRequestChainFactory.CreateDefaultChain();
        var request = new ServiceRequest(
            "Ion Popescu",
            "Toyota Camry 2020",
            "Oil Change",
            ServiceRequestComplexity.Low,
            150m);

        var result = chain.Handle(request);

        Assert.NotNull(result);
        Assert.Equal(nameof(ReceptionRequestHandler), result.HandlerName);
        Assert.Equal("Reception", result.Department);
        Assert.False(result.RequiresFollowUp);
    }

    [Fact]
    public void Chain_DiagnosticsHandlesMediumRequest()
    {
        var chain = ServiceRequestChainFactory.CreateDefaultChain();
        var request = new ServiceRequest(
            "Maria Ionescu",
            "Nissan Leaf 2021",
            "Electric Diagnostics",
            ServiceRequestComplexity.Medium,
            450m);

        var result = chain.Handle(request);

        Assert.NotNull(result);
        Assert.Equal(nameof(DiagnosticsRequestHandler), result.HandlerName);
        Assert.Equal("Diagnostics", result.Department);
        Assert.True(result.RequiresFollowUp);
    }

    [Fact]
    public void Chain_SeniorTechnicianHandlesComplexRepair()
    {
        var chain = ServiceRequestChainFactory.CreateDefaultChain();
        var request = new ServiceRequest(
            "Andrei Marin",
            "BMW X5 2022",
            "Engine Repair",
            ServiceRequestComplexity.High,
            900m);

        var result = chain.Handle(request);

        Assert.NotNull(result);
        Assert.Equal(nameof(SeniorTechnicianRequestHandler), result.HandlerName);
        Assert.Equal("Senior Technician", result.Department);
        Assert.True(result.RequiresFollowUp);
    }

    [Fact]
    public void Chain_ManagerHandlesVipOrExpensiveRequest()
    {
        var chain = ServiceRequestChainFactory.CreateDefaultChain();
        var request = new ServiceRequest(
            "Elena Pop",
            "Audi A6 2023",
            "VIP Full Service Package",
            ServiceRequestComplexity.High,
            1600m,
            IsVipCustomer: true);

        var result = chain.Handle(request);

        Assert.NotNull(result);
        Assert.Equal(nameof(ManagerApprovalRequestHandler), result.HandlerName);
        Assert.Equal("Service Manager", result.Department);
        Assert.True(result.RequiresFollowUp);
    }

    [Fact]
    public void Chain_ReturnsNullWhenNoHandlerCanProcessRequest()
    {
        var chain = ServiceRequestChainFactory.CreateDefaultChain();
        var request = new ServiceRequest(
            "Test Client",
            "Unknown Vehicle",
            "Interior Cleaning",
            ServiceRequestComplexity.Low,
            500m);

        var result = chain.Handle(request);

        Assert.Null(result);
    }

    [Fact]
    public void Chain_ClientCanStartFromSubchain()
    {
        var diagnostics = new DiagnosticsRequestHandler();
        var seniorTechnician = new SeniorTechnicianRequestHandler();
        var manager = new ManagerApprovalRequestHandler();

        diagnostics
            .SetNext(seniorTechnician)
            .SetNext(manager);

        var request = new ServiceRequest(
            "Radu Stan",
            "Ford Focus 2018",
            "Brake Repair",
            ServiceRequestComplexity.High,
            700m);

        var result = diagnostics.Handle(request);

        Assert.NotNull(result);
        Assert.Equal(nameof(SeniorTechnicianRequestHandler), result.HandlerName);
    }
}
