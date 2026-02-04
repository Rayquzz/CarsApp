using CarsApp.Domain.Entities;
using CarsApp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Infrastructure.Services
{
    public class OilChange : IServiceOperation
    {
        public string Name => "Oil Change";
        public void Perform(Vehicle vehicle)
        {
            // Implementation for performing an oil change on the vehicle
            Console.WriteLine($"Performing oil change on {vehicle.GetVehicleType()} - {vehicle.Make} {vehicle.Model} ({vehicle.Year})");
        }
    
    }
}
