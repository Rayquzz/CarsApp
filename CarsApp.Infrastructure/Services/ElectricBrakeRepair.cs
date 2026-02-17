using CarsApp.Domain.Services;
using CarsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Infrastructure.Services
{
    public class ElectricBrakeRepair : IServiceOperation
    {
        public string Name => "Electric Brake Repair";
        public void Perform(Vehicle vehicle)
        {
            Console.WriteLine("Repairing Electric Brake.");
        }
    }
}
