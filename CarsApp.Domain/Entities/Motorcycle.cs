using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Entities
{
    public class Motorcycle : Vehicle
    {
        public Motorcycle(string make, string model, int year)
            : base(make, model, year) 
        {
        }
        public override string GetVehicleType()
        {
            return "Motorcycle";
        }
    }
}
