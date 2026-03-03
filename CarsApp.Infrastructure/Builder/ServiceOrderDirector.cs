using CarsApp.Domain.Entities;
using CarsApp.Domain.Services;

namespace CarsApp.Infrastructure.Builder
{
    public class ServiceOrderDirector
    {
        public void BuildFullService(IServiceOrderBuilder builder, Vehicle v)
        {
            builder.Reset();
            builder.ForVehicle(v)
                   .WithPriority("Standard")
                   .AddService("Oil Change")
                   .AddService("Brake Repair")
                   .AddService("Engine Repair")
                   .WithTechnician("Ion Popescu")
                   .WithLoanCar(true)
                   .WithEstimatedCost(850m);
        }

        public void BuildUrgentRepair(IServiceOrderBuilder builder, Vehicle v, string tip)
        {
            builder.Reset();
            builder.ForVehicle(v)
                   .WithPriority("Urgent")
                   .AddService(tip)
                   .WithTechnician("Maria Ionescu")
                   .WithLoanCar(false)
                   .WithEstimatedCost(300m);
        }

        public void BuildVipService(IServiceOrderBuilder builder, Vehicle v)
        {
            builder.Reset();
            builder.ForVehicle(v)
                   .WithPriority("VIP")
                   .AddService("Engine Repair")
                   .AddService("Brake Repair")
                   .AddService("Oil Change")
                   .AddService("Full Detailing")
                   .WithTechnician("Alexandru Dumitrescu")
                   .WithLoanCar(true)
                   .WithEstimatedCost(1500m);
        }
    }
}