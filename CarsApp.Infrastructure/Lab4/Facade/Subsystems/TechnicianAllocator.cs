using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Infrastructure.Lab4.Facade.Subsystems
{
    // Subsistem 2 — aloca un tehnician disponibil
    public class TechnicianAllocator
    {
        private static readonly string[] _technicians =
        {
            "Ion Popescu", "Maria Ionescu", "Andrei Marin",
            "Elena Dumitrescu", "Alexandru Constantin"
        };

        public string AllocateTechnician(string packageName)
        {
            // VIP primeste senior technician
            if (packageName.Contains("VIP"))
                return "Alexandru Constantin";

            var index = Math.Abs(packageName.GetHashCode()) % _technicians.Length;
            return _technicians[index];
        }
    }
}