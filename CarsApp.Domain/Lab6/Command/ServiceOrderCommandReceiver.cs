using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Entities;

namespace CarsApp.Domain.Lab6.Command;

public class ServiceOrderCommandReceiver
{
    public void AddService(ServiceOrder order, string serviceName)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        if (string.IsNullOrWhiteSpace(serviceName))
            throw new ArgumentException("Service name cannot be empty.", nameof(serviceName));

        if (!order.Services.Contains(serviceName))
        {
            order.Services.Add(serviceName);
        }
    }

    public void InsertService(ServiceOrder order, string serviceName, int index)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        if (index < 0 || index > order.Services.Count)
        {
            order.Services.Add(serviceName);
            return;
        }

        order.Services.Insert(index, serviceName);
    }

    public void RemoveService(ServiceOrder order, string serviceName)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        order.Services.Remove(serviceName);
    }

    public void ChangePriority(ServiceOrder order, string newPriority)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        if (string.IsNullOrWhiteSpace(newPriority))
            throw new ArgumentException("Priority cannot be empty.", nameof(newPriority));

        order.Priority = newPriority;
    }

    public void AssignTechnician(ServiceOrder order, string technicianName)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        order.TechnicianName = technicianName;
    }

    public void Schedule(ServiceOrder order, DateTime scheduledDate)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        order.ScheduledDate = scheduledDate;
    }
}