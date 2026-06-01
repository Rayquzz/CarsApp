using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab5.Proxy;

namespace CarsApp.Infrastructure.Lab5.Proxy;

// Serviciul REAL — scump de apelat (simulăm delay de BD)
// Proxy-urile îl învelesc și controlează accesul la el
public class RealVehicleHistoryService : IVehicleHistoryService
{
    private static readonly Dictionary<string, string> _db = new()
    {
        { "Toyota", "Fara probleme majore. Revizie completa efectuata la 50.000 km." },
        { "Ford",   "Revizie efectuata acum 6 luni. Schimb amortizoare in 2023." },
        { "BMW",    "Reparatie motor in urma cu 1 an. Atentie: consum ulei crescut." },
        { "Audi",   "Verificare frana recenta. Istoric curat, proprietar unic." },
        { "Honda",  "Masina de import. Dosar complet disponibil la receptie." },
    };

    public HistoryResult GetHistory(string make, string model, int year, string userRole)
    {
        // Simulăm o operație costisitoare (BD reală, API extern etc.)
        Thread.Sleep(80);

        var data = _db.TryGetValue(make, out var note)
            ? note
            : "Primul check-in in sistemul nostru. Nu exista istoric anterior.";

        return new HistoryResult(
            Success: true,
            Data: data,
            AccessLog: $"[REAL] Apel BD pentru {make} {model} ({year})",
            CacheStatus: "BYPASSED",
            FromCache: false,
            DeniedReason: ""
        );
    }
}