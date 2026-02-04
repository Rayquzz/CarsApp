using CarsApp.Domain.Entities;
using CarsApp.application.Services;
using CarsApp.Infrastructure.Services;


namespace CarsApp.ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Vehicle vehicle = new Car("Toyota", "Camry", 2020);

            var service = new OilChange();
            var manager = new ServiceManager();

            manager.ExecuteService(vehicle, service);

            Console.ReadLine();
        }
    }
}