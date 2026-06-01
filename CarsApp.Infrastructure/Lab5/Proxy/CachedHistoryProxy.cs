using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab5.Proxy;

namespace CarsApp.Infrastructure.Lab5.Proxy;

// VIRTUAL / CACHING PROXY — apelează serviciul real doar la primul request
// Al doilea apel pentru același vehicul → returnează din cache instant
public class CachedHistoryProxy : IVehicleHistoryService
{
    private readonly IVehicleHistoryService _next;
    private readonly Dictionary<string, HistoryResult> _cache = new();
    private readonly TimeSpan _ttl;
    private readonly Dictionary<string, DateTime> _cacheTime = new();

    public int CacheHits { get; private set; }
    public int CacheMisses { get; private set; }

    public CachedHistoryProxy(IVehicleHistoryService next, TimeSpan? ttl = null)
    {
        _next = next;
        _ttl = ttl ?? TimeSpan.FromMinutes(5);
    }

    public HistoryResult GetHistory(string make, string model, int year, string userRole)
    {
        var key = $"{make}:{model}:{year}";

        // Verifică cache-ul
        if (_cache.TryGetValue(key, out var cached))
        {
            var age = DateTime.Now - _cacheTime[key];
            if (age < _ttl)
            {
                CacheHits++;
                return cached with
                {
                    CacheStatus = "HIT",
                    FromCache =   true,
                    AccessLog =    $"[CACHE] HIT (vârstă: {age.TotalSeconds:F1}s) | " + cached.AccessLog
                };
            }
            // Expirat — scoatem din cache
            _cache.Remove(key);
            _cacheTime.Remove(key);
        }

        // Cache miss — apelăm serviciul următor din lanț
        CacheMisses++;
        var result = _next.GetHistory(make, model, year, userRole);

        if (result.Success)
        {
            _cache[key] = result;
            _cacheTime[key] = DateTime.Now;
        }

        return result with
        {
            CacheStatus =  "MISS",
            FromCache =    false,
            AccessLog =    $"[CACHE] MISS — stocat în cache | " + result.AccessLog
        };
    }

    public void InvalidateCache() => _cache.Clear();
}