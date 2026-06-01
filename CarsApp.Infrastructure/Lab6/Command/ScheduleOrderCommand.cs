using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Entities;
using CarsApp.Domain.Lab6.Command;

namespace CarsApp.Infrastructure.Lab6.Command;

public class ScheduleOrderCommand : IServiceOrderCommand
{
    private readonly ServiceOrderCommandReceiver _receiver;
    private readonly ServiceOrder _order;
    private readonly DateTime _newScheduledDate;
    private DateTime _previousScheduledDate;

    public string Name => "Schedule Order";
    public string Description => $"Schedule order {_order.OrderId} for {_newScheduledDate:yyyy-MM-dd HH:mm}.";
    public DateTime? ExecutedAt { get; private set; }

    public ScheduleOrderCommand(
        ServiceOrderCommandReceiver receiver,
        ServiceOrder order,
        DateTime newScheduledDate)
    {
        _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        _order = order ?? throw new ArgumentNullException(nameof(order));
        _newScheduledDate = newScheduledDate;
    }

    public void Execute()
    {
        _previousScheduledDate = _order.ScheduledDate;

        _receiver.Schedule(_order, _newScheduledDate);

        ExecutedAt = DateTime.Now;
    }

    public void Undo()
    {
        _receiver.Schedule(_order, _previousScheduledDate);
    }
}