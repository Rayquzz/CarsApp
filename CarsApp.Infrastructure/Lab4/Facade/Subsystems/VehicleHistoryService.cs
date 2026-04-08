using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Infrastructure.Lab4.Facade.Subsystems
{
    // Subsistem 1 — verifica istoricul vehiculului
    public class VehicleHistoryService
    {
        public string GetHistory(string make, string model, int year)
        {
            // Simuleaza o baza de date de istoric
            var histories = new Dictionary<string, string>
            {
                { "Toyota", "Fara probleme majore inregistrate." },
                { "Ford",   "Revizie efectuata acum 6 luni." },
                { "BMW",    "Reparatie motor in urma cu 1 an." },
                { "Audi",   "Verificare frana efectuata recent." },
            };

            return histories.TryGetValue(make, out var note)
                ? note
                : "Primul check-in in sistemul nostru.";
        }
    }
}