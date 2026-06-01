using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Lab5.Decorator;

// Componenta concretă — serviciu simplu fără niciun extra
public class BaseServiceOperation : IDecoratedService
{
    private readonly List<string> _log = new();

    public string Name { get; }
    public string Description { get; }
    public decimal Cost { get; }
    public IReadOnlyList<string> Log => _log;

    public BaseServiceOperation(string name, string description, decimal cost)
    {
        Name = name;
        Description = description;
        Cost = cost;
    }

    public string Execute(string vehicleInfo)
    {
        var msg = $" Executat: {Name} pe {vehicleInfo} — Cost: {Cost:C}";
        _log.Add(msg);
        return msg;
    }
}