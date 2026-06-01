using CarsApp.Domain.Lab7.Mediator;

namespace CarsApp.Infrastructure.Lab7.Mediator;

public class WorkshopMediator : IWorkshopMediator
{
    private readonly List<WorkshopCoordinationEntry> _coordinationLog = new();
    private readonly ReceptionDesk _receptionDesk;
    private readonly TechnicianTeam _technicianTeam;
    private readonly PartsDepartment _partsDepartment;
    private readonly CustomerNotificationCenter _notificationCenter;

    public IReadOnlyList<WorkshopCoordinationEntry> CoordinationLog => _coordinationLog.AsReadOnly();

    public WorkshopMediator(
        ReceptionDesk receptionDesk,
        TechnicianTeam technicianTeam,
        PartsDepartment partsDepartment,
        CustomerNotificationCenter notificationCenter)
    {
        _receptionDesk = receptionDesk ?? throw new ArgumentNullException(nameof(receptionDesk));
        _technicianTeam = technicianTeam ?? throw new ArgumentNullException(nameof(technicianTeam));
        _partsDepartment = partsDepartment ?? throw new ArgumentNullException(nameof(partsDepartment));
        _notificationCenter = notificationCenter ?? throw new ArgumentNullException(nameof(notificationCenter));

        _receptionDesk.SetMediator(this);
        _technicianTeam.SetMediator(this);
        _partsDepartment.SetMediator(this);
    }

    public void Notify(object sender, string ev)
    {
        if (sender == null)
        {
            throw new ArgumentNullException(nameof(sender));
        }

        if (string.IsNullOrWhiteSpace(ev))
        {
            throw new ArgumentException("Workshop event is required.", nameof(ev));
        }

        switch (ev)
        {
            case WorkshopEvent.ServiceRequestCreated when sender is ReceptionDesk:
                CoordinateServiceRequest();
                break;

            case WorkshopEvent.PartsRequested when sender is TechnicianTeam:
                CoordinatePartsRequest();
                break;

            case WorkshopEvent.PartsAvailable when sender is PartsDepartment:
                CoordinateAvailablePart();
                break;

            case WorkshopEvent.PartsUnavailable when sender is PartsDepartment:
                CoordinateUnavailablePart();
                break;

            case WorkshopEvent.RepairCompleted when sender is TechnicianTeam:
                CoordinateCompletedRepair();
                break;

            default:
                AddLog(sender, ev, "No coordination rule matched this event.");
                break;
        }
    }

    private void CoordinateServiceRequest()
    {
        _technicianTeam.AssignDiagnostic(_receptionDesk.LastServiceRequest);
        _notificationCenter.Send($"Service request registered: {_receptionDesk.LastServiceRequest}");
        AddLog(_receptionDesk, WorkshopEvent.ServiceRequestCreated, "Technician diagnostic assigned and customer notified.");
    }

    private void CoordinatePartsRequest()
    {
        AddLog(_technicianTeam, WorkshopEvent.PartsRequested, $"Parts department checks stock for {_technicianTeam.LastRequestedPart}.");
        _partsDepartment.CheckAvailability(_technicianTeam.LastRequestedPart);
    }

    private void CoordinateAvailablePart()
    {
        _technicianTeam.ResumeRepairWithPart(_partsDepartment.LastProcessedPart);
        _notificationCenter.Send($"Part reserved: {_partsDepartment.LastProcessedPart}. Repair continues.");
        AddLog(_partsDepartment, WorkshopEvent.PartsAvailable, "Technician team resumed repair and customer was updated.");
    }

    private void CoordinateUnavailablePart()
    {
        _technicianTeam.PutRepairOnHold(_partsDepartment.LastProcessedPart);
        _notificationCenter.Send($"Part unavailable: {_partsDepartment.LastProcessedPart}. Repair is waiting for stock.");
        AddLog(_partsDepartment, WorkshopEvent.PartsUnavailable, "Technician team paused repair and customer was updated.");
    }

    private void CoordinateCompletedRepair()
    {
        _receptionDesk.PrepareCheckout(_technicianTeam.LastCompletedRepair);
        _notificationCenter.Send($"Repair completed: {_technicianTeam.LastCompletedRepair}. Vehicle is ready for pickup.");
        AddLog(_technicianTeam, WorkshopEvent.RepairCompleted, "Reception prepared checkout and customer was notified.");
    }

    private void AddLog(object sender, string ev, string action)
    {
        _coordinationLog.Add(new WorkshopCoordinationEntry(sender.GetType().Name, ev, action));
    }
}
