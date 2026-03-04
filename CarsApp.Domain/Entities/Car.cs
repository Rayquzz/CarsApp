using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Entities
{
    public class Car : Vehicle
    {
        public Car(string make, string model, int year)
            : base(make, model, year) 
        {
        }


        // Constructor de copiere — apelează base(source) pentru câmpurile din Vehicle
        // Identic cu: constructor Rectangle(source: Rectangle) -> super(source)
        protected Car(Car source) : base(source) 
        {
        }

        public override string GetVehicleType() => "Car";


        // Clone returnează new Car(this) — exact ca în exemplul abstract
        public override Vehicle Clone() => new Car(this);
    }
}
