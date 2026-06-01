using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab6.Observer;

namespace CarsApp.Infrastructure.Lab6.Observer;

public class ReceptionDashboardObserver : IServiceOrderObserver
{
    private readonly List<string> _statusHistory = new();

    public IReadOnlyList<string> StatusHistory => _statusHistory.AsReadOnly();

    public void Update(IServiceOrderSubject subject)
    {
        var entry =
            $"[{subject.LastStatusChangedAt:yyyy-MM-dd HH:mm:ss}] " +
            $"Comanda {subject.Order.OrderId}: " +
            $"{subject.PreviousStatus} -> {subject.CurrentStatus}. " +
            $"{subject.LastStatusMessage}";

        _statusHistory.Add(entry);
    }
}