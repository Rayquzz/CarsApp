using CarsApp.Domain.Entities;
using CarsApp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Infrastructure.Services
{
    public class CombustionBrakeRepair : IServiceOperation
    {
        public string Name => "Combustion Brake Repair";

        public void Perform(Vehicle vehicle)
        {
            Console.WriteLine("Repairing Combustion Engine Brake.");
        }
    }
}
