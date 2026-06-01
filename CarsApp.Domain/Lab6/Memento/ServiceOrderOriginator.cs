using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Entities;

namespace CarsApp.Domain.Lab6.Memento;

public class ServiceOrderOriginator
{
    public ServiceOrder Order { get; }

    public ServiceOrderOriginator(ServiceOrder order)
    {
        Order = order ?? throw new ArgumentNullException(nameof(order));
    }

    public IServiceOrderMemento Save(string description)
    {
        return new ServiceOrderMemento(Order, description);
    }

    public void Restore(IServiceOrderMemento memento)
    {
        if (memento is not ServiceOrderMemento serviceOrderMemento)
        {
            throw new ArgumentException("Invalid memento type.", nameof(memento));
        }

        Order.OrderId = serviceOrderMemento.OrderId;
        Order.Vehicle = serviceOrderMemento.Vehicle;
        Order.Services = new List<string>(serviceOrderMemento.Services);
        Order.Priority = serviceOrderMemento.Priority;
        Order.TechnicianName = serviceOrderMemento.TechnicianName;
        Order.ScheduledDate = serviceOrderMemento.ScheduledDate;
        Order.EstimatedCost = serviceOrderMemento.EstimatedCost;
        Order.Notes = serviceOrderMemento.Notes;
        Order.IncludesLoanCar = serviceOrderMemento.IncludesLoanCar;
    }
}