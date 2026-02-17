using CarsApp.Domain.Services;
using CarsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace CarsApp.Infrastructure.Services
{
    public class CombustionEngineRepair : IServiceOperation
    {
        public string Name => "Combustion Engine Repair";

        public void Perform(Vehicle vehicle)
        {
            Console.WriteLine("Repairing Combustion Engine.");
        }
    }
}
