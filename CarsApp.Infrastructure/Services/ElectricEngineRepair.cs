using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Services;
using CarsApp.Domain.Entities;

namespace CarsApp.Infrastructure.Services
{
    public class ElectricEngineRepair : IServiceOperation
    {
        public string Name => "Electric Engine Repair";
        public void Perform(Vehicle vehicle)
        {
            Console.WriteLine("Repairing Electric Engine.");
        }
    }
}
