using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Lab5.Decorator;

// Interfață extinsă față de IServiceOperation
// Decoratorii lucrează cu aceasta
public interface IDecoratedService
{
    string Name { get; }
    string Description { get; }
    decimal Cost { get; }
    IReadOnlyList<string> Log { get; }  // înregistrează ce s-a adăugat

    string Execute(string vehicleInfo); // returnează rezultat ca string (pentru UI)
}