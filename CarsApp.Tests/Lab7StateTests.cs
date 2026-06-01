using CarsApp.Domain.Builder;
using CarsApp.Domain.Entities;
using CarsApp.Domain.Lab7.State;
using CarsApp.Infrastructure.Lab7.State;
using Xunit;

namespace CarsApp.Tests;

public class Lab7StateTests
{
    [Fact]
    public void State_WorkflowMovesFromCreatedToCompleted()
    {
        var order = CreateOrder();
        var workflow = ServiceOrderWorkflowFactory.Create(order);
        var scheduledDate = DateTime.Today.AddDays(2).AddHours(10);

        var schedule = workflow.Schedule(scheduledDate);
        var start = workflow.StartWork("Ioana Stan");
        var complete = workflow.Complete("Service completed successfully.");

        Assert.True(schedule.Success);
        Assert.True(start.Success);
        Assert.True(complete.Success);
        Assert.Equal(OrderWorkflowStatus.Completed, workflow.CurrentStatus);
        Assert.Equal(scheduledDate, order.ScheduledDate);
        Assert.Equal("Ioana Stan", order.TechnicianName);
        Assert.Equal("Service completed successfully.", order.Notes);
        Assert.Contains(workflow.History, item => item.Contains("InProgress -> Completed"));
    }

    [Fact]
    public void State_CurrentStateRejectsInvalidAction()
    {
        var order = CreateOrder();
        var workflow = ServiceOrderWorkflowFactory.Create(order);

        var result = workflow.StartWork("Mihai Pop");

        Assert.False(result.Success);
        Assert.Equal("Created", result.FromState);
        Assert.Equal("Created", result.ToState);
        Assert.Equal(OrderWorkflowStatus.Created, workflow.CurrentStatus);
        Assert.Contains("Cannot start work", result.Message);
    }

    [Fact]
    public void State_InProgressCanWaitForPartsAndResume()
    {
        var order = CreateOrder();
        var workflow = ServiceOrderWorkflowFactory.Create(order);

        workflow.Schedule(DateTime.Today.AddDays(1));
        workflow.StartWork("Radu Ionescu");

        var wait = workflow.WaitForParts("Brake pads are missing.");
        var resume = workflow.StartWork("Radu Ionescu");

        Assert.True(wait.Success);
        Assert.Equal("WaitingForParts", wait.ToState);
        Assert.True(resume.Success);
        Assert.Equal(OrderWorkflowStatus.InProgress, workflow.CurrentStatus);
        Assert.Contains(workflow.History, item => item.Contains("InProgress -> WaitingForParts"));
        Assert.Contains(workflow.History, item => item.Contains("WaitingForParts -> InProgress"));
    }

    [Fact]
    public void State_CompletedOrderIsTerminal()
    {
        var order = CreateOrder();
        var workflow = ServiceOrderWorkflowFactory.Create(order);

        workflow.Schedule(DateTime.Today.AddDays(1));
        workflow.StartWork("Elena Marin");
        workflow.Complete("Ready for pickup.");

        var cancel = workflow.Cancel("Client changed mind.");

        Assert.False(cancel.Success);
        Assert.Equal("Completed", cancel.FromState);
        Assert.Equal("Completed", cancel.ToState);
        Assert.Equal(OrderWorkflowStatus.Completed, workflow.CurrentStatus);
    }

    [Fact]
    public void State_ClientCanProvideInitialState()
    {
        var order = CreateOrder();
        var workflow = new ServiceOrderWorkflow(order, new ScheduledOrderState());

        var result = workflow.StartWork("Victor Ene");

        Assert.True(result.Success);
        Assert.Equal("Scheduled", result.FromState);
        Assert.Equal("InProgress", result.ToState);
        Assert.Equal(OrderWorkflowStatus.InProgress, workflow.CurrentStatus);
    }

    private static ServiceOrder CreateOrder()
    {
        var builder = new ServiceOrderBuilder();

        builder
            .ForVehicle(new Car("Toyota", "Camry", 2020))
            .AddService("Oil Change")
            .WithPriority("Standard")
            .WithNotes("Lab7 State test order");

        return builder.GetProduct();
    }
}
