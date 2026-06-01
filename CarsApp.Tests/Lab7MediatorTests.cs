using CarsApp.Domain.Lab7.Mediator;
using CarsApp.Infrastructure.Lab7.Mediator;
using Xunit;

namespace CarsApp.Tests;

public class Lab7MediatorTests
{
    [Fact]
    public void Mediator_ServiceRequestCreated_AssignsDiagnosticAndNotifiesCustomer()
    {
        var mediator = WorkshopMediatorFactory.CreateDefault(
            out var reception,
            out var technicians,
            out _,
            out var notifications);

        reception.CreateServiceRequest("Ion Popescu", "Toyota Camry 2020", "Oil Change");

        Assert.Single(reception.ServiceRequests);
        Assert.Single(technicians.AssignedJobs);
        Assert.Contains("Toyota Camry 2020", technicians.AssignedJobs[0]);
        Assert.Single(notifications.Notifications);
        Assert.Contains("Service request registered", notifications.Notifications[0]);
        Assert.Contains(mediator.CoordinationLog, entry =>
            entry.Event == WorkshopEvent.ServiceRequestCreated &&
            entry.Sender == nameof(ReceptionDesk));
    }

    [Fact]
    public void Mediator_PartsRequested_WhenPartIsAvailable_ReservesPartAndResumesRepair()
    {
        var mediator = WorkshopMediatorFactory.CreateDefault(
            out _,
            out var technicians,
            out var parts,
            out var notifications);
        parts.AddStock("Brake pads", 2);

        technicians.RequestParts("Brake pads");

        Assert.Equal(1, parts.Stock["Brake pads"]);
        Assert.Contains("Brake pads", parts.ReservedParts);
        Assert.Contains(technicians.WorkNotes, note => note.Contains("Repair resumed"));
        Assert.Contains(notifications.Notifications, note => note.Contains("Part reserved"));
        Assert.Contains(mediator.CoordinationLog, entry => entry.Event == WorkshopEvent.PartsRequested);
        Assert.Contains(mediator.CoordinationLog, entry => entry.Event == WorkshopEvent.PartsAvailable);
    }

    [Fact]
    public void Mediator_PartsRequested_WhenPartIsMissing_PutsRepairOnHold()
    {
        var mediator = WorkshopMediatorFactory.CreateDefault(
            out _,
            out var technicians,
            out var parts,
            out var notifications);

        technicians.RequestParts("ABS sensor");

        Assert.Contains("ABS sensor", parts.MissingParts);
        Assert.Contains(technicians.WorkNotes, note => note.Contains("Repair put on hold"));
        Assert.Contains(notifications.Notifications, note => note.Contains("Part unavailable"));
        Assert.Contains(mediator.CoordinationLog, entry => entry.Event == WorkshopEvent.PartsUnavailable);
    }

    [Fact]
    public void Mediator_RepairCompleted_PreparesCheckoutAndNotifiesCustomer()
    {
        var mediator = WorkshopMediatorFactory.CreateDefault(
            out var reception,
            out var technicians,
            out _,
            out var notifications);

        technicians.CompleteRepair("Engine repair completed successfully");

        Assert.Single(technicians.CompletedRepairs);
        Assert.Single(reception.CheckoutPreparations);
        Assert.Contains("Engine repair completed successfully", reception.CheckoutPreparations[0]);
        Assert.Contains(notifications.Notifications, note => note.Contains("Vehicle is ready for pickup"));
        Assert.Contains(mediator.CoordinationLog, entry =>
            entry.Event == WorkshopEvent.RepairCompleted &&
            entry.Action.Contains("Reception prepared checkout"));
    }
}
