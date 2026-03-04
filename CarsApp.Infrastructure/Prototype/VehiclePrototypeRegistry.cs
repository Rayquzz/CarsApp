using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Entities;

namespace CarsApp.Infrastructure.Prototype
{
    public class VehiclePrototypeRegistry
    {

        private readonly Dictionary<string, Vehicle> _prototypes = new();
        public void Register(string key, Vehicle prototype)
        {
            _prototypes[key] = prototype;
        }
        public Vehicle GetClone(string key)
        {
            if (!_prototypes.TryGetValue(key, out var prototype))
                throw new KeyNotFoundException($"Prototipul '{key}' nu există în registry.");
            
            return prototype.Clone();
            
        }

        public IEnumerable<string> GetAvailableKeys() => _prototypes.Keys;
    }
}
