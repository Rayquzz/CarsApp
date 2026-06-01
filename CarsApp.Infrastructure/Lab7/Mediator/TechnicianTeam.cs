using CarsApp.Domain.Lab7.Mediator;

namespace CarsApp.Infrastructure.Lab7.Mediator;

public class TechnicianTeam : WorkshopComponentBase
{
    private readonly List<string> _assignedJobs = new();
    private readonly List<string> _partsRequests = new();
    private readonly List<string> _workNotes = new();
    private readonly List<string> _completedRepairs = new();

    public IReadOnlyList<string> AssignedJobs => _assignedJobs.AsReadOnly();

    public IReadOnlyList<string> PartsRequests => _partsRequests.AsReadOnly();

    public IReadOnlyList<string> WorkNotes => _workNotes.AsReadOnly();

    public IReadOnlyList<string> CompletedRepairs => _completedRepairs.AsReadOnly();

    public string LastRequestedPart { get; private set; } = string.Empty;

    public string LastCompletedRepair { get; private set; } = string.Empty;

    public void AssignDiagnostic(string serviceRequest)
    {
        var job = $"Diagnostic assigned for {serviceRequest}";
        _assignedJobs.Add(job);
    }

    public void RequestParts(string partName)
    {
        if (string.IsNullOrWhiteSpace(partName))
        {
            throw new ArgumentException("Part name is required.", nameof(partName));
        }

        LastRequestedPart = partName;
        _partsRequests.Add(partName);

        Mediator.Notify(this, WorkshopEvent.PartsRequested);
    }

    public void ResumeRepairWithPart(string partName)
    {
        _workNotes.Add($"Repair resumed with reserved part: {partName}");
    }

    public void PutRepairOnHold(string partName)
    {
        _workNotes.Add($"Repair put on hold. Missing part: {partName}");
    }

    public void CompleteRepair(string repairSummary)
    {
        if (string.IsNullOrWhiteSpace(repairSummary))
        {
            throw new ArgumentException("Repair summary is required.", nameof(repairSummary));
        }

        LastCompletedRepair = repairSummary;
        _completedRepairs.Add(repairSummary);

        Mediator.Notify(this, WorkshopEvent.RepairCompleted);
    }
}
