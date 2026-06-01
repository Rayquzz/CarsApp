using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Entities;
using CarsApp.Domain.Lab6.Command;

namespace CarsApp.Infrastructure.Lab6.Command;

public class AssignTechnicianCommand : IServiceOrderCommand
{
    private readonly ServiceOrderCommandReceiver _receiver;
    private readonly ServiceOrder _order;
    private readonly string _newTechnicianName;
    private string _previousTechnicianName = string.Empty;

    public string Name => "Assign Technician";
    public string Description => $"Assign technician '{_newTechnicianName}' to order {_order.OrderId}.";
    public DateTime? ExecutedAt { get; private set; }

    public AssignTechnicianCommand(
        ServiceOrderCommandReceiver receiver,
        ServiceOrder order,
        string newTechnicianName)
    {
        _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        _order = order ?? throw new ArgumentNullException(nameof(order));
        _newTechnicianName = newTechnicianName;
    }

    public void Execute()
    {
        _previousTechnicianName = _order.TechnicianName;

        _receiver.AssignTechnician(_order, _newTechnicianName);

        ExecutedAt = DateTime.Now;
    }

    public void Undo()
    {
        _receiver.AssignTechnician(_order, _previousTechnicianName);
    }
}