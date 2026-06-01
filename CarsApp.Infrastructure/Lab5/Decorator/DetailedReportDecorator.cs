using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab5.Decorator;

namespace CarsApp.Infrastructure.Lab5.Decorator;

// Adaugă generarea unui raport detaliat
public class DetailedReportDecorator : ServiceDecorator
{
    public DetailedReportDecorator(IDecoratedService inner) : base(inner) { }

    public override string Name => $"{_inner.Name} + Raport";
    public override string Description => $"{_inner.Description} | Raport detaliat PDF";
    public override decimal Cost => _inner.Cost + 25m;

    public override string Execute(string vehicleInfo)
    {
        var baseResult = _inner.Execute(vehicleInfo);
        var reportId = $"RPT-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";
        return baseResult + $"\n📄 Raport generat: {reportId} pentru {vehicleInfo} (+25 RON)";
    }
}