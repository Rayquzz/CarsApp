using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Entities;
using CarsApp.Domain.Lab6.Command;

namespace CarsApp.Infrastructure.Lab6.Command;

public class RemoveServiceCommand : IServiceOrderCommand
{
    private readonly ServiceOrderCommandReceiver _receiver;
    private readonly ServiceOrder _order;
    private readonly string _serviceName;
    private int _previousIndex = -1;
    private bool _wasRemoved;

    public string Name => "Remove Service";
    public string Description => $"Remove service '{_serviceName}' from order {_order.OrderId}.";
    public DateTime? ExecutedAt { get; private set; }

    public RemoveServiceCommand(
        ServiceOrderCommandReceiver receiver,
        ServiceOrder order,
        string serviceName)
    {
        _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        _order = order ?? throw new ArgumentNullException(nameof(order));
        _serviceName = serviceName;
    }

    public void Execute()
    {
        _previousIndex = _order.Services.IndexOf(_serviceName);
        _wasRemoved = _previousIndex >= 0;

        if (_wasRemoved)
        {
            _receiver.RemoveService(_order, _serviceName);
        }

        ExecutedAt = DateTime.Now;
    }

    public void Undo()
    {
        if (_wasRemoved)
        {
            _receiver.InsertService(_order, _serviceName, _previousIndex);
        }
    }
}