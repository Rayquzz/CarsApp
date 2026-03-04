using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Entities
{
    public class Motorcycle : Vehicle
    {
        public bool HasSidecar { get; private set; } // Indică dacă motocicleta are ataș sau nu
        public Motorcycle(string make, string model, int year, bool hasSidecar = false)
            : base(make, model, year) 
        {
            HasSidecar = hasSidecar;
        }

        protected Motorcycle(Motorcycle source) : base(source) 
        {
            HasSidecar = source.HasSidecar;
        }

        public override string GetVehicleType() => "Motorcycle";

        public override Vehicle Clone() => new Motorcycle(this);
    }
}
