using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Lab5.Flyweight;

public interface ISparePartFlyweight
{
    string PartCode { get; }   // stare intrinsecă (partajată)
    string Name { get; }
    string Manufacturer { get; }
    string Category { get; }

    // Metoda primește starea extrinsecă (diferită per instanță)
    string GetLabel(int quantity, string location);
}