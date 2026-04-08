using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarsApp.Domain.Lab4.Composite;

namespace CarsApp.Infrastructure.Lab4.Composite
{
    public static class ServicePackageCatalog
    {
        public static IReadOnlyList<IServiceComponent> GetPackages()
        {
            // ── Servicii individuale ──────────────────────────
            var oilChange = new SingleService("Oil Change", "Schimb ulei și filtru", 150m);
            var tireCheck = new SingleService("Tire Check", "Verificare și echilibrare anvelope", 80m);
            var brakeRepair = new SingleService("Brake Repair", "Înlocuire plăcuțe frână", 250m);
            var engineRepair = new SingleService("Engine Repair", "Diagnoză și reparație motor", 500m);
            var airFilter = new SingleService("Air Filter", "Înlocuire filtru aer", 60m);
            var batteryCheck = new SingleService("Battery Check", "Verificare și testare baterie", 50m);
            var fullDetailing = new SingleService("Full Detailing", "Curățare completă interior/exterior", 200m);
            var diagnostics = new SingleService("Diagnostics", "Diagnoză electronică completă", 120m);

            // ── Pachete compuse ───────────────────────────────

            // Basic Package (Leaf-uri simple)
            var basicPackage = new ServicePackage(
                "Basic Package",
                "Întreținere de bază — ideal pentru revizie anuală")
                .Add(oilChange)
                .Add(tireCheck)
                .Add(airFilter);

            // Engine Package
            var enginePackage = new ServicePackage(
                "Engine Package",
                "Servicii complete pentru motor")
                .Add(engineRepair)
                .Add(diagnostics);

            // Standard Package (conține Basic Package + extra)
            var standardPackage = new ServicePackage(
                "Standard Package",
                "Pachet complet — include Basic și reparații frâne")
                .Add(basicPackage)
                .Add(brakeRepair)
                .Add(batteryCheck);

            // VIP Package (conține Standard + Engine + Detailing)
            var vipPackage = new ServicePackage(
                "VIP Package",
                "Pachet premium — tot ce are nevoie mașina ta")
                .Add(standardPackage)
                .Add(enginePackage)
                .Add(fullDetailing);

            return new List<IServiceComponent>
            {
                basicPackage,
                standardPackage,
                vipPackage,
                enginePackage
            };
        }
    }
}