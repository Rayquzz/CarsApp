using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab5.Flyweight;

namespace CarsApp.Infrastructure.Lab5.Flyweight;

// FABRICA — inima paternului Flyweight
// Menține pool-ul de obiecte partajate
public class SparePartFactory
{
    // Pool-ul: PartCode → instanță unică
    private readonly Dictionary<string, ISparePartFlyweight> _pool = new();

    public ISparePartFlyweight GetOrCreate(string partCode, string name,
                                           string manufacturer, string category)
    {
        if (!_pool.TryGetValue(partCode, out var part))
        {
            part = new SparePartFlyweight(partCode, name, manufacturer, category);
            _pool[partCode] = part;
        }
        // Dacă există deja → returnăm ACEEAȘI instanță
        return part;
    }

    public int PoolSize => _pool.Count;

    public IReadOnlyDictionary<string, ISparePartFlyweight> Pool => _pool;
}
