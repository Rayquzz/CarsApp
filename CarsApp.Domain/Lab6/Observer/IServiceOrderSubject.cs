using CarsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Entities;

namespace CarsApp.Domain.Lab6.Observer;

public interface IServiceOrderSubject
{
    ServiceOrder Order { get; }
    ServiceOrderStatus CurrentStatus { get; }
    ServiceOrderStatus PreviousStatus { get; }
    DateTime LastStatusChangedAt { get; }
    string LastStatusMessage { get; }

    void Attach(IServiceOrderObserver observer);
    void Detach(IServiceOrderObserver observer);
    void Notify();
}