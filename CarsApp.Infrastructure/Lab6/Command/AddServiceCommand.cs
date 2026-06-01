using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Entities;
using CarsApp.Domain.Lab6.Command;

namespace CarsApp.Infrastructure.Lab6.Command;

public class AddServiceCommand : IServiceOrderCommand
{
    private readonly ServiceOrderCommandReceiver _receiver;
    private readonly ServiceOrder _order;
    private readonly string _serviceName;
    private bool _wasAdded;

    public string Name => "Add Service";
    public string Description => $"Add service '{_serviceName}' to order {_order.OrderId}.";
    public DateTime? ExecutedAt { get; private set; }

    public AddServiceCommand(
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
        _wasAdded = !_order.Services.Contains(_serviceName);

        _receiver.AddService(_order, _serviceName);

        ExecutedAt = DateTime.Now;
    }

    public void Undo()
    {
        if (_wasAdded)
        {
            _receiver.RemoveService(_order, _serviceName);
        }
    }
}