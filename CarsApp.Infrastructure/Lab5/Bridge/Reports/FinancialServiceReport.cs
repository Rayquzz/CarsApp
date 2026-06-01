using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab5.Bridge;

namespace CarsApp.Infrastructure.Lab5.Bridge.Reports;

// Raport FINANCIAR — focus pe cost, TVA, defalcare pe servicii
public class FinancialServiceReport : ServiceReport
{
    private const decimal TvaRate = 0.19m;

    public FinancialServiceReport(IReportRenderer renderer) : base(renderer) { }

    public override string ReportType => "Raport Financiar";

    public override string Generate(ReportData d)
    {
        var fara_tva = Math.Round(d.TotalCost / (1 + TvaRate), 2);
        var tva = d.TotalCost - fara_tva;

        // Distribuie costul uniform per serviciu (simplificat)
        var perService = d.Services.Count > 0
            ? d.Services.Select(s => $"{s}: {fara_tva / d.Services.Count:C}").ToList()
            : new List<string> { "N/A" };

        var parts = new List<string>
        {
            _renderer.RenderTitle($"Factură Servicii — {d.OrderId}"),
            _renderer.RenderSeparator(),
            _renderer.RenderKeyValue("Client",     d.CustomerName),
            _renderer.RenderKeyValue("Vehicul",    d.VehicleInfo),
            _renderer.RenderKeyValue("Data",       d.ScheduledDate.ToString("dd.MM.yyyy")),
            _renderer.RenderSeparator(),
            _renderer.RenderSection("Defalcare servicii", perService),
            _renderer.RenderSeparator(),
            _renderer.RenderKeyValue("Subtotal (fără TVA)", fara_tva.ToString("C")),
            _renderer.RenderKeyValue($"TVA ({TvaRate:P0})",  tva.ToString("C")),
            _renderer.RenderKeyValue("TOTAL",               d.TotalCost.ToString("C")),
        };
        return _renderer.Finalize(parts);
    }
}