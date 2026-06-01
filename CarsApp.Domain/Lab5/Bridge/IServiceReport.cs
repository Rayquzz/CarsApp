using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Lab5.Bridge;

// DATE care descriu o comandă de service — folosite de toate rapoartele
public record ReportData(
    string OrderId,
    string CustomerName,
    string VehicleInfo,
    string TechnicianName,
    List<string> Services,
    decimal TotalCost,
    DateTime ScheduledDate,
    string Priority
);

// ABSTRACȚIE — definește tipul de raport
// Primește renderer-ul prin constructor (bridge-ul)
public abstract class ServiceReport
{
    protected readonly IReportRenderer _renderer; // bridge spre implementor

    protected ServiceReport(IReportRenderer renderer)
    {
        _renderer = renderer;
    }

    public string RendererName => _renderer.RendererName;
    public abstract string ReportType { get; }

    // Fiecare subclasă decide CE include în raport
    // Renderer-ul decide CUM arată
    public abstract string Generate(ReportData data);
}