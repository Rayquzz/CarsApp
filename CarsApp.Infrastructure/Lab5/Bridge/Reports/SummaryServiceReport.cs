using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab5.Bridge;

namespace CarsApp.Infrastructure.Lab5.Bridge.Reports;

// Raport SUMAR — puține detalii, potrivit pentru client
public class SummaryServiceReport : ServiceReport
{
    public SummaryServiceReport(IReportRenderer renderer) : base(renderer) { }

    public override string ReportType => "Raport Sumar";

    public override string Generate(ReportData d)
    {
        var parts = new List<string>
        {
            _renderer.RenderTitle($"Raport Sumar — {d.OrderId}"),
            _renderer.RenderSeparator(),
            _renderer.RenderKeyValue("Client",   d.CustomerName),
            _renderer.RenderKeyValue("Vehicul",  d.VehicleInfo),
            _renderer.RenderKeyValue("Cost",     d.TotalCost.ToString("C")),
            _renderer.RenderKeyValue("Data",     d.ScheduledDate.ToString("dd.MM.yyyy")),
        };
        return _renderer.Finalize(parts);
    }
}