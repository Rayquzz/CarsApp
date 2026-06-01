using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab5.Flyweight;

namespace CarsApp.Infrastructure.Lab5.Flyweight;

// NU stochează datele piesei — doar referința la flyweight + starea proprie
public class StockEntry
{
    private readonly ISparePartFlyweight _part;  // referință partajată

    public int Quantity { get; set; }   // stare extrinsecă
    public string Location { get; set; }  // stare extrinsecă
    public decimal Price { get; set; }  // stare extrinsecă

    public StockEntry(ISparePartFlyweight part, int quantity,
                      string location, decimal price)
    {
        _part = part;
        Quantity = quantity;
        Location = location;
        Price = price;
    }

    public string PartCode => _part.PartCode;
    public string GetLabel() => _part.GetLabel(Quantity, Location);

    // Demonstrează că referința e aceeași
    public int FlyweightHashCode => _part.GetHashCode();
}