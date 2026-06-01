using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Lab5.Proxy;

// Interfața pe care o implementează ATÂT serviciul real, CÂT ȘI toți proxy-ii
// Clientul nu știe niciodată cu cine vorbește de fapt
public interface IVehicleHistoryService
{
    HistoryResult GetHistory(string make, string model, int year, string userRole);
}

public record HistoryResult(
    bool Success,
    string Data,          // istoricul propriu-zis
    string AccessLog,     // ce a înregistrat proxy-ul de logging
    string CacheStatus,   // HIT / MISS / BYPASSED
    bool FromCache,
    string DeniedReason   // motivul dacă accesul a fost refuzat
);