using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Entities;

namespace CarsApp.Domain.Lab6.Observer;

public class ServiceOrderStatusSubject : IServiceOrderSubject
{
    private readonly List<IServiceOrderObserver> _observers = new();

    public ServiceOrder Order { get; }
    public ServiceOrderStatus CurrentStatus { get; private set; }
    public ServiceOrderStatus PreviousStatus { get; private set; }
    public DateTime LastStatusChangedAt { get; private set; }
    public string LastStatusMessage { get; private set; }

    public ServiceOrderStatusSubject(
        ServiceOrder order,
        ServiceOrderStatus initialStatus = ServiceOrderStatus.Created)
    {
        Order = order ?? throw new ArgumentNullException(nameof(order));
        CurrentStatus = initialStatus;
        PreviousStatus = initialStatus;
        LastStatusChangedAt = DateTime.Now;
        LastStatusMessage = "Comanda a fost initializata.";
    }

    public void Attach(IServiceOrderObserver observer)
    {
        if (observer == null)
            throw new ArgumentNullException(nameof(observer));

        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void Detach(IServiceOrderObserver observer)
    {
        if (observer == null)
            throw new ArgumentNullException(nameof(observer));

        _observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in _observers.ToList())
        {
            observer.Update(this);
        }
    }

    public void ChangeStatus(ServiceOrderStatus newStatus, string message)
    {
        if (newStatus == CurrentStatus)
        {
            return;
        }

        PreviousStatus = CurrentStatus;
        CurrentStatus = newStatus;
        LastStatusChangedAt = DateTime.Now;
        LastStatusMessage = message;

        Notify();
    }
}