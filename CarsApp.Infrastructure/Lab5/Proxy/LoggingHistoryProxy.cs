using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab5.Proxy;

namespace CarsApp.Infrastructure.Lab5.Proxy;

// LOGGING PROXY — înregistrează orice acces, indiferent de rezultat
// Transparent pentru client — nu modifică datele, doar le observă
public class LoggingHistoryProxy : IVehicleHistoryService
{
    private readonly IVehicleHistoryService _next;
    private readonly List<string> _auditTrail = new();

    public LoggingHistoryProxy(IVehicleHistoryService next)
    {
        _next = next;
    }

    public IReadOnlyList<string> AuditTrail => _auditTrail;

    public HistoryResult GetHistory(string make, string model, int year, string userRole)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        _auditTrail.Add($"[{timestamp}] REQUEST  — {make} {model} ({year}) | Rol: {userRole}");

        var result = _next.GetHistory(make, model, year, userRole);

        var status = result.Success ? "OK" : "DENIED";
        _auditTrail.Add($"[{timestamp}] RESPONSE — {status} | Cache: {result.CacheStatus}");

        return result with
        {
            AccessLog = $"[LOG] Acces audit la {timestamp} | " + result.AccessLog
        };
    }

    public List<string> GetAuditTrail() => new(_auditTrail);
}