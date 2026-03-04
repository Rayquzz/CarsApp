using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Entities
{
    public class Truck : Vehicle
    {

        public int PayloadCapacity { get; private set; } // Capacitatea de încărcare în kilograme
        public Truck(string make, string model, int year, int payloadCapacity = 5000)
            : base(make, model, year) 
        {
            PayloadCapacity = payloadCapacity;
        }

        protected Truck(Truck source) : base(source) 
        {
            PayloadCapacity = source.PayloadCapacity;
        }

        public override string GetVehicleType() => "Truck";

        public override Vehicle Clone() => new Truck(this);

    }
}
