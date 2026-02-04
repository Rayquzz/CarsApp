using CarsApp.Domain.Entities;
using CarsApp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Infrastructure.Services
{
    public class BrakeRepair : IServiceOperation
    {
        public string Name => "Brake Repair";
        public void Perform(Vehicle vehicle)
        {
            // Implementation for performing brake repair on the vehicle
            Console.WriteLine($"Performing brake repair on {vehicle.GetVehicleType()} - {vehicle.Make} {vehicle.Model} ({vehicle.Year})");
        }
    
    }
}
