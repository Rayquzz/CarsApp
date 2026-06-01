using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Entities;

namespace CarsApp.Domain.Lab6.Memento;

internal sealed class ServiceOrderMemento : IServiceOrderMemento
{
    internal string OrderId { get; }
    internal Vehicle? Vehicle { get; }
    internal List<string> Services { get; }
    internal string Priority { get; }
    internal string TechnicianName { get; }
    internal DateTime ScheduledDate { get; }
    internal decimal EstimatedCost { get; }
    internal string Notes { get; }
    internal bool IncludesLoanCar { get; }

    private readonly DateTime _createdAt;
    private readonly string _description;

    public ServiceOrderMemento(ServiceOrder order, string description)
    {
        OrderId = order.OrderId;
        Vehicle = order.Vehicle;
        Services = new List<string>(order.Services);
        Priority = order.Priority;
        TechnicianName = order.TechnicianName;
        ScheduledDate = order.ScheduledDate;
        EstimatedCost = order.EstimatedCost;
        Notes = order.Notes;
        IncludesLoanCar = order.IncludesLoanCar;

        _createdAt = DateTime.Now;
        _description = description;
    }

    public string GetName()
    {
        return $"{_createdAt:yyyy-MM-dd HH:mm:ss} / Order {OrderId}";
    }

    public DateTime GetDate()
    {
        return _createdAt;
    }

    public string GetDescription()
    {
        return _description;
    }
}