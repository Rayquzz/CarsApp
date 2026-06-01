using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab5.Decorator;

namespace CarsApp.Infrastructure.Lab5.Decorator;

// Trimite SMS la finalizarea serviciului
public class SmsNotificationDecorator : ServiceDecorator
{
    private readonly string _phoneNumber;

    public SmsNotificationDecorator(IDecoratedService inner, string phoneNumber = "07xxxxxxxx")
        : base(inner) => _phoneNumber = phoneNumber;

    public override string Name => $"{_inner.Name} + SMS";
    public override string Description => $"{_inner.Description} | Notificare SMS la {_phoneNumber}";
    public override decimal Cost => _inner.Cost + 5m;

    public override string Execute(string vehicleInfo)
    {
        var baseResult = _inner.Execute(vehicleInfo);
        return baseResult + $"\n📱 SMS trimis la {_phoneNumber}: \"{Name} finalizat pentru {vehicleInfo}\" (+5 RON)";
    }
}