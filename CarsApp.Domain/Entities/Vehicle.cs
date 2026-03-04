using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Prototype;

namespace CarsApp.Domain.Entities
{
    public abstract class Vehicle : IPrototype<Vehicle>
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
        protected Vehicle(Vehicle source) 
        {
            Make = source.Make;
            Model = source.Model;
            Year = source.Year;
        }

        public abstract string GetVehicleType();

        public abstract Vehicle Clone();
    }
}
