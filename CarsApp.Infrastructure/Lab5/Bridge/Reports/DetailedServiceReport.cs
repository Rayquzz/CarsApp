using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab5.Bridge;

namespace CarsApp.Infrastructure.Lab5.Bridge.Reports;

// Raport DETALIAT — toate informațiile, pentru arhivă internă
public class DetailedServiceReport : ServiceReport
{
    public DetailedServiceReport(IReportRenderer renderer) : base(renderer) { }

    public override string ReportType => "Raport Detaliat";

    public override string Generate(ReportData d)
    {
        var parts = new List<string>
        {
            _renderer.RenderTitle($"Raport Detaliat — {d.OrderId}"),
            _renderer.RenderSeparator(),
            _renderer.RenderKeyValue("Client",     d.CustomerName),
            _renderer.RenderKeyValue("Vehicul",    d.VehicleInfo),
            _renderer.RenderKeyValue("Tehnician",  d.TechnicianName),
            _renderer.RenderKeyValue("Prioritate", d.Priority),
            _renderer.RenderKeyValue("Data",       d.ScheduledDate.ToString("dd.MM.yyyy HH:mm")),
            _renderer.RenderSeparator(),
            _renderer.RenderSection("Servicii efectuate", d.Services),
            _renderer.RenderSeparator(),
            _renderer.RenderKeyValue("Cost total", d.TotalCost.ToString("C")),
        };
        return _renderer.Finalize(parts);
    }
}