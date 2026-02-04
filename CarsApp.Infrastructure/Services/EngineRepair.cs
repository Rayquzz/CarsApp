using CarsApp.Domain.Entities;
using CarsApp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Infrastructure.Services
{
    public class EngineRepair : IServiceOperation
    {
        public string Name => "Engine Repair";
        public void Perform(Vehicle vehicle)
        {
            // Implementation for performing engine repair on the vehicle
            Console.WriteLine($"Performing engine repair on {vehicle.GetVehicleType()} - {vehicle.Make} {vehicle.Model} ({vehicle.Year})");
        }
    
    }
}
