using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Entities;
using CarsApp.Domain.Lab6.Command;

namespace CarsApp.Infrastructure.Lab6.Command;

public class ChangePriorityCommand : IServiceOrderCommand
{
    private readonly ServiceOrderCommandReceiver _receiver;
    private readonly ServiceOrder _order;
    private readonly string _newPriority;
    private string _previousPriority = string.Empty;

    public string Name => "Change Priority";
    public string Description => $"Change priority of order {_order.OrderId} to '{_newPriority}'.";
    public DateTime? ExecutedAt { get; private set; }

    public ChangePriorityCommand(
        ServiceOrderCommandReceiver receiver,
        ServiceOrder order,
        string newPriority)
    {
        _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        _order = order ?? throw new ArgumentNullException(nameof(order));
        _newPriority = newPriority;
    }

    public void Execute()
    {
        _previousPriority = _order.Priority;

        _receiver.ChangePriority(_order, _newPriority);

        ExecutedAt = DateTime.Now;
    }

    public void Undo()
    {
        _receiver.ChangePriority(_order, _previousPriority);
    }
}
