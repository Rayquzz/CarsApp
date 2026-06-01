using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab5.Decorator;

namespace CarsApp.Infrastructure.Lab5.Decorator;

// Adaugă garanție extinsă — mărește costul și adaugă o linie în rezultat
public class WarrantyDecorator : ServiceDecorator
{
    private readonly int _months;

    public WarrantyDecorator(IDecoratedService inner, int months = 12)
        : base(inner) => _months = months;

    public override string Name => $"{_inner.Name} + Garanție {_months}L";
    public override string Description => $"{_inner.Description} | Garanție extinsă {_months} luni";
    public override decimal Cost => _inner.Cost + (_months * 10m);

    public override string Execute(string vehicleInfo)
    {
        var baseResult = _inner.Execute(vehicleInfo);
        return baseResult + $"\n🛡️  Garanție {_months} luni adăugată (+{_months * 10m:C})";
    }
}