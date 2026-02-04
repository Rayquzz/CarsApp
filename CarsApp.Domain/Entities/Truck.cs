using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Entities
{
    internal class Truck : Vehicle
    {
        public Truck(string make, string model, int year)
            : base(make, model, year) 
        {
        }
        public override string GetVehicleType()
        {
            return "Truck";
        }

    }
}
