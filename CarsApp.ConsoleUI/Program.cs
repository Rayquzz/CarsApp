using CarsApp.application.Interfaces;
using CarsApp.application.Services;
using CarsApp.Domain.Builder;
using CarsApp.Domain.Entities;
using CarsApp.Infrastructure.Builder;
using CarsApp.Infrastructure.Factories.AbstractFactory;
using CarsApp.Infrastructure.Factories.FactoryMethod;
using CarsApp.Infrastructure.Services;
using System.Diagnostics;

namespace CarsApp.ConsoleUI
{
    internal class Program
    {
        // vehicul comun folosit în toate demo-urile
        static Vehicle vehicle = new Car("Toyota", "Camry", 2020);

        static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("==============================");
                Console.WriteLine("   CARS APP — Design Patterns ");
                Console.WriteLine("==============================");
                Console.WriteLine("1. Lab 1 — OOP & SOLID");
                Console.WriteLine("2. Lab 2 — Factory Method & Abstract Factory");
                Console.WriteLine("3. Lab 3 — Builder, Prototype, Singleton");
                Console.WriteLine("0. Exit");
                Console.WriteLine("------------------------------");
                Console.Write("Alege optiunea: ");

                switch (Console.ReadLine())
                {
                    case "1": MenuLab1(); break;
                    case "2": MenuLab2(); break;
                    case "3": MenuLab3(); break;
                    case "0": running = false; break;
                    default:
                        Console.WriteLine("Optiune invalida.");
                        Pause();
                        break;
                }
            }
        }

        // ─────────────────────────────────────────
        // LAB 1
        // ─────────────────────────────────────────
        static void MenuLab1()
        {
            Console.Clear();
            Console.WriteLine("==============================");
            Console.WriteLine("   LAB 1 — OOP & SOLID        ");
            Console.WriteLine("==============================");
            Console.WriteLine("Vehicule disponibile:");
            Console.WriteLine();

            Vehicle car = new Car("Toyota", "Camry", 2020);
            Vehicle truck = new Truck("Ford", "F-150", 2021);
            Vehicle moto = new Motorcycle("Honda", "CBR", 2019);

            Console.WriteLine($"  Tip: {car.GetVehicleType()} | {car.Make} {car.Model} ({car.Year})");
            Console.WriteLine($"  Tip: {truck.GetVehicleType()} | {truck.Make} {truck.Model} ({truck.Year})");
            Console.WriteLine($"  Tip: {moto.GetVehicleType()} | {moto.Make} {moto.Model} ({moto.Year})");

            Console.WriteLine();
            Console.WriteLine("Principii SOLID aplicate:");
            Console.WriteLine("  SRP — fiecare clasa are o singura responsabilitate");
            Console.WriteLine("  OCP — Vehicle e extensibila fara modificari");
            Console.WriteLine("  LSP — Car/Truck/Motorcycle inlocuiesc Vehicle");
            Console.WriteLine("  ISP — IServiceOperation are doar metodele necesare");
            Console.WriteLine("  DIP — ServiceManager depinde de interfata, nu de implementare");

            Pause();
        }

        // ─────────────────────────────────────────
        // LAB 2
        // ─────────────────────────────────────────
        static void MenuLab2()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("==============================");
                Console.WriteLine("   LAB 2 — Factory Patterns   ");
                Console.WriteLine("==============================");
                Console.WriteLine("1. Factory Method");
                Console.WriteLine("2. Abstract Factory");
                Console.WriteLine("0. Inapoi");
                Console.WriteLine("------------------------------");
                Console.Write("Alege optiunea: ");

                switch (Console.ReadLine())
                {
                    case "1": DemoFactoryMethod(); break;
                    case "2": DemoAbstractFactory(); break;
                    case "0": back = true; break;
                    default:
                        Console.WriteLine("Optiune invalida.");
                        Pause();
                        break;
                }
            }
        }

        static void DemoFactoryMethod()
        {
            Console.Clear();
            Console.WriteLine("── Factory Method ──────────────");
            Console.WriteLine("Crearea serviciilor prin fabrica concreta,");
            Console.WriteLine("fara a specifica direct clasa serviciului.");
            Console.WriteLine();

            var manager = new ServiceManager();

            ServiceFactory factory = new EngineRepairFactory();
            var service = factory.CreateService();
            Console.Write("  EngineRepairFactory → ");
            manager.ExecuteService(vehicle, service);

            factory = new BrakeRepairFactory();
            service = factory.CreateService();
            Console.Write("  BrakeRepairFactory  → ");
            manager.ExecuteService(vehicle, service);

            factory = new OilChangeFactory();
            service = factory.CreateService();
            Console.Write("  OilChangeFactory    → ");
            manager.ExecuteService(vehicle, service);

            Pause();
        }

        static void DemoAbstractFactory()
        {
            Console.Clear();
            Console.WriteLine("── Abstract Factory ────────────");
            Console.WriteLine("Comutare intre familii de servicii");
            Console.WriteLine("(Electric vs Combustion) fara a schimba clientul.");
            Console.WriteLine();

            var manager = new ServiceManager();

            Console.WriteLine("  >> ElectricServiceFactory:");
            IServicePackageFactory factory = new ElectricServiceFactory();
            manager.ExecuteService(vehicle, factory.CreateEngineRepair());
            manager.ExecuteService(vehicle, factory.CreateBrakeRepair());

            Console.WriteLine();
            Console.WriteLine("  >> CombustionServiceFactory:");
            factory = new CombustionServiceFactory();
            manager.ExecuteService(vehicle, factory.CreateEngineRepair());
            manager.ExecuteService(vehicle, factory.CreateBrakeRepair());

            Pause();
        }

        // ─────────────────────────────────────────
        // LAB 3
        // ─────────────────────────────────────────
        static void MenuLab3()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("==============================");
                Console.WriteLine("   LAB 3 — Creational Patterns");
                Console.WriteLine("==============================");
                Console.WriteLine("1. Builder");
                Console.WriteLine("2. Prototype  (in curand)");
                Console.WriteLine("3. Singleton  (in curand)");
                Console.WriteLine("0. Inapoi");
                Console.WriteLine("------------------------------");
                Console.Write("Alege optiunea: ");

                switch (Console.ReadLine())
                {
                    case "1": DemoBuilder(); break;
                    case "2":
                        Console.WriteLine("  Prototype — nu este inca implementat.");
                        Pause();
                        break;
                    case "3":
                        Console.WriteLine("  Singleton — nu este inca implementat.");
                        Pause();
                        break;
                    case "0": back = true; break;
                    default:
                        Console.WriteLine("Optiune invalida.");
                        Pause();
                        break;
                }
            }
        }

        static void DemoBuilder()
        {
            Console.Clear();
            Console.WriteLine("── Builder ─────────────────────");
            Console.WriteLine("Constructie pas cu pas a unei comenzi complexe.");
            Console.WriteLine();

            var director = new ServiceOrderDirector();
            var builder = new ServiceOrderBuilder();

            // Full Service prin Director
            Console.WriteLine("  >> BuildFullService (Director):");
            director.BuildFullService(builder, vehicle);
            PrintOrder(builder.GetProduct());

            // Urgent Repair prin Director
            Console.WriteLine("  >> BuildUrgentRepair (Director):");
            director.BuildUrgentRepair(builder, vehicle, "Brake Repair");
            PrintOrder(builder.GetProduct());

            // VIP prin Director
            Console.WriteLine("  >> BuildVipService (Director):");
            director.BuildVipService(builder, vehicle);
            PrintOrder(builder.GetProduct());

            // Builder manual — fara Director
            Console.WriteLine("  >> Builder manual (fara Director):");
            builder.ForVehicle(vehicle)
                   .WithPriority("Standard")
                   .AddService("Oil Change")
                   .WithTechnician("Andrei Marin")
                   .WithEstimatedCost(200m);
            PrintOrder(builder.GetProduct());

            Pause();
        }

        // ─────────────────────────────────────────
        // HELPERS
        // ─────────────────────────────────────────
        static void PrintOrder(ServiceOrder order)
        {
            Console.WriteLine($"    Order ID  : {order.OrderId}");
            Console.WriteLine($"    Vehicle   : {order.Vehicle?.Make} {order.Vehicle?.Model} ({order.Vehicle?.Year})");
            Console.WriteLine($"    Services  : {string.Join(", ", order.Services)}");
            Console.WriteLine($"    Priority  : {order.Priority}");
            Console.WriteLine($"    Technician: {order.TechnicianName ?? "Neasignat"}");
            Console.WriteLine($"    Loan Car  : {order.IncludesLoanCar}");
            Console.WriteLine($"    Cost      : {order.EstimatedCost:C}");
            Console.WriteLine($"    Date      : {order.ScheduledDate:dd.MM.yyyy}");
            Console.WriteLine();
        }

        static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Apasa Enter pentru a continua...");
            Console.ReadLine();
        }
    }
}