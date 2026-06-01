using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab6.Observer;

namespace CarsApp.Infrastructure.Lab6.Observer;

public class CustomerNotificationObserver : IServiceOrderObserver
{
    private readonly List<string> _notifications = new();

    public IReadOnlyList<string> Notifications => _notifications.AsReadOnly();

    public void Update(IServiceOrderSubject subject)
    {
        var vehicleInfo = subject.Order.Vehicle != null
            ? $"{subject.Order.Vehicle.Make} {subject.Order.Vehicle.Model}"
            : "vehicul necunoscut";

        var message =
            $"Client notification: Comanda {subject.Order.OrderId} pentru {vehicleInfo} " +
            $"a trecut din statusul {subject.PreviousStatus} in {subject.CurrentStatus}. " +
            $"Mesaj: {subject.LastStatusMessage}";

        _notifications.Add(message);
    }
}
