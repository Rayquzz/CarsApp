using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab5.Decorator;

namespace CarsApp.Infrastructure.Lab5.Decorator;

// Aplică discount pentru clienți fideli — scade costul
public class LoyaltyDiscountDecorator : ServiceDecorator
{
    private readonly int _discountPercent;

    public LoyaltyDiscountDecorator(IDecoratedService inner, int discountPercent = 10)
        : base(inner) => _discountPercent = discountPercent;

    public override string Name => $"{_inner.Name} [{_discountPercent}% Discount]";
    public override string Description => $"{_inner.Description} | Discount client fidel {_discountPercent}%";
    public override decimal Cost => _inner.Cost * (1 - _discountPercent / 100m);

    public override string Execute(string vehicleInfo)
    {
        var saved = _inner.Cost * (_discountPercent / 100m);
        var baseResult = _inner.Execute(vehicleInfo);
        return baseResult + $"\n💛 Discount {_discountPercent}% aplicat (economie: {saved:C})";
    }
}