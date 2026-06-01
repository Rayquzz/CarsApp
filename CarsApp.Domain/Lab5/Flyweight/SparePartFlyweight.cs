using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Lab5.Flyweight;

// Obiect IMUTABIL — starea intrinsecă nu se schimbă niciodată
public sealed class SparePartFlyweight : ISparePartFlyweight
{
    public string PartCode { get; }
    public string Name { get; }
    public string Manufacturer { get; }
    public string Category { get; }

    public SparePartFlyweight(string partCode, string name,
                              string manufacturer, string category)
    {
        PartCode = partCode;
        Name = name;
        Manufacturer = manufacturer;
        Category = category;
    }

    // Starea extrinsecă vine din afară — flyweight-ul nu o stochează
    public string GetLabel(int quantity, string location)
        => $"[{PartCode}] {Name} ({Manufacturer}) | Cant: {quantity} | Loc: {location}";
}