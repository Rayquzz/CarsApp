using CarsApp.application.Interfaces;
using CarsApp.application.Services;
using CarsApp.Domain.Builder;
using CarsApp.Domain.Entities;
using CarsApp.Infrastructure.Builder;
//using CarsApp.Infrastructure.Factories.FactoryMethod;
using CarsApp.Infrastructure.Factories.AbstractFactory;
using CarsApp.Infrastructure.Services;



namespace CarsApp.ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Vehicle vehicle = new Car("Toyota", "Camry", 2020);


            //Factory Method pattern usage - se poate adauga usor un nou tip de serviciu (de exemplu, Brake Repair) fara a
            //modifica codul clientului (ServiceManager), doar prin adaugarea unei noi clase de fabrica si a unei noi clase de
            //serviciu care implementeaza interfata IServiceOperation.
            //ServiceFactory factory = new EngineRepairFactory(); 
            //var service = factory.CreateService();
            //var manager = new ServiceManager();
            //manager.ExecuteService(vehicle, service);



            //Abstract Factory pattern usage - se poate comuta intre diferite fabrici pentru a crea servicii specifice
            //pentru masini electrice sau masini cu combustie interna, 
            //fara a schimba codul clientului (ServiceManager)
            IServicePackageFactory factory = new ElectricServiceFactory();
            // sau
            // IServicePackageFactory factory = new ElectricVehicleServiceFactory();
            
            
            //var engineService = factory.CreateEngineRepair();
            //var brakeService = factory.CreateBrakeRepair();


            //var manager = new ServiceManager();
            //manager.ExecuteService(vehicle, engineService);
            //manager.ExecuteService(vehicle, brakeService);


            var director = new ServiceOrderDirector();
            var builder = new ServiceOrderBuilder();

            director.BuildFullService(builder, vehicle);
            ServiceOrder order = builder.GetProduct();

            //Console.WriteLine(order);
            Console.WriteLine($"Order ID: {order.OrderId}");
            Console.WriteLine($"Vehicle: {order.Vehicle.Make} {order.Vehicle.Model}");
            Console.WriteLine($"Services: {string.Join(", ", order.Services)}");
            Console.WriteLine($"Priority: {order.Priority}");
            Console.WriteLine($"Technician: {order.TechnicianName}");
            Console.WriteLine($"Loan Car: {order.IncludesLoanCar}");
            Console.WriteLine($"Cost: {order.EstimatedCost}");
            Console.ReadLine();
        }
    }
}