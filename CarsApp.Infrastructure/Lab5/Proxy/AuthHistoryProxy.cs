using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab5.Proxy;

namespace CarsApp.Infrastructure.Lab5.Proxy;

// PROTECTION PROXY — verifică rolul înainte să lase cererea să treacă
// Client și Mecanic văd date diferite; Anonim — blocat complet
public class AuthHistoryProxy : IVehicleHistoryService
{
    private readonly IVehicleHistoryService _next;

    // Rolurile care au acces complet
    private static readonly HashSet<string> _fullAccess = new() { "Admin", "Mecanic" };
    private static readonly HashSet<string> _limitedAccess = new() { "Client" };

    public AuthHistoryProxy(IVehicleHistoryService next)
    {
        _next = next;
    }

    public HistoryResult GetHistory(string make, string model, int year, string userRole)
    {
        // Blocat complet
        if (!_fullAccess.Contains(userRole) && !_limitedAccess.Contains(userRole))
        {
            return new HistoryResult(
                Success: false,
                Data: "",
                AccessLog: $"[AUTH] BLOCAT — rol '{userRole}' nu are acces",
                CacheStatus: "BYPASSED",
                FromCache: false,
                DeniedReason: $"Rolul '{userRole}' nu are permisiunea de a accesa istoricul vehiculelor."
            );
        }

        // Client — acces limitat, nu vede notele interne
        if (_limitedAccess.Contains(userRole))
        {
            var limited = _next.GetHistory(make, model, year, userRole);
            var truncated = limited.Data.Length > 50
                ? limited.Data[..50] + "... [date restricționate]"
                : limited.Data + " [vizualizare parțială]";

            return limited with
            {
                Data      =      truncated,
                AccessLog =  $"[AUTH] Acces limitat pentru rol 'Client' | " + limited.AccessLog
            };
        }

        // Admin / Mecanic — acces complet, delegăm mai departe
        var result = _next.GetHistory(make, model, year, userRole);
        return result with
        {
            AccessLog = $"[AUTH] Acces complet acordat pentru rol '{userRole}' | " + result.AccessLog
        };
    }
}
