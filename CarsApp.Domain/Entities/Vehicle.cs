using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Entities
{
    public abstract class Vehicle
    {
        public string Make { get; protected set; }
        public string Model { get; protected set; }
        public int Year { get; protected set; }

        protected Vehicle(string make, string model, int year) 
        {
            Make = make;
            Model = model;
            Year = year;

        }

        public abstract string GetVehicleType();
    }
}
