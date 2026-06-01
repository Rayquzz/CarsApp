using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab6.Observer;

namespace CarsApp.Infrastructure.Lab6.Observer;

public class TechnicianNotificationObserver : IServiceOrderObserver
{
    private readonly List<string> _tasks = new();

    public IReadOnlyList<string> Tasks => _tasks.AsReadOnly();

    public void Update(IServiceOrderSubject subject)
    {
        if (subject.CurrentStatus is not ServiceOrderStatus.Scheduled
            and not ServiceOrderStatus.InProgress
            and not ServiceOrderStatus.WaitingForParts)
        {
            return;
        }

        var technician = string.IsNullOrWhiteSpace(subject.Order.TechnicianName)
            ? "tehnician nealocat"
            : subject.Order.TechnicianName;

        var message =
            $"Technician task: {technician} trebuie sa verifice comanda {subject.Order.OrderId}. " +
            $"Status curent: {subject.CurrentStatus}. " +
            $"Detalii: {subject.LastStatusMessage}";

        _tasks.Add(message);
    }
}