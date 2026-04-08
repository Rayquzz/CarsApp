using CarsApp.Domain.Builder;
using CarsApp.Domain.Entities;
using CarsApp.Infrastructure.Builder;
using CarsApp.Infrastructure.Prototype;
using CarsApp.Infrastructure.Singleton;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class Lab3Model : PageModel
{
    private static readonly Vehicle _vehicle = new Car("Toyota", "Camry", 2020);

    public ServiceOrder? Order { get; set; }
    public List<CloneInfo> Clones { get; set; } = new();
    public bool SingletonVerified { get; set; }
    public int FooHash { get; set; }
    public int BarHash { get; set; }
    public List<string> LogEntries { get; set; } = new();

    public void OnGet() { }

    public IActionResult OnPostBuilder(string buildType)
    {
        var director = new ServiceOrderDirector();
        var builder = new ServiceOrderBuilder();
        switch (buildType)
        {
            case "full": director.BuildFullService(builder, _vehicle); break;
            case "urgent": director.BuildUrgentRepair(builder, _vehicle, "Brake Repair"); break;
            case "vip": director.BuildVipService(builder, _vehicle); break;
            default:
                builder.ForVehicle(_vehicle).WithPriority("Standard")
                       .AddService("Oil Change").WithTechnician("Andrei Marin")
                       .WithEstimatedCost(200m);
                break;
        }
        Order = builder.GetProduct();
        return Page();
    }

    public IActionResult OnPostPrototype()
    {
        var original = new Car("Toyota", "Camry", 2020);
        var clone = original.Clone();
        Clones.Add(new CloneInfo("Car", original.Make, original.Model, original.Year, original.GetHashCode(), "Original"));
        Clones.Add(new CloneInfo("Car", clone.Make, clone.Model, clone.Year, clone.GetHashCode(), "Clone direct"));

        var list = new List<Vehicle>
        {
            new Car("Honda", "Civic", 2021),
            new Truck("Ford", "F-150", 2022, 3000),
            new Motorcycle("Yamaha", "MT-07", 2023, false)
        };
        foreach (var c in list.Select(v => v.Clone()))
            Clones.Add(new CloneInfo(c.GetVehicleType(), c.Make, c.Model, c.Year, c.GetHashCode(), "Clone polimorfic"));

        var registry = new VehiclePrototypeRegistry();
        registry.Register("standard-car", new Car("Toyota", "Corolla", 2024));
        registry.Register("heavy-truck", new Truck("Volvo", "FH16", 2023, 20000));
        var rc = registry.GetClone("standard-car");
        var rt = registry.GetClone("heavy-truck");
        Clones.Add(new CloneInfo(rc.GetVehicleType(), rc.Make, rc.Model, rc.Year, rc.GetHashCode(), "Registry"));
        Clones.Add(new CloneInfo(rt.GetVehicleType(), rt.Make, rt.Model, rt.Year, rt.GetHashCode(), "Registry"));
        return Page();
    }

    public IActionResult OnPostSingleton()
    {
        var foo = ServiceLogger.Instance;
        var bar = ServiceLogger.Instance;
        FooHash = foo.GetHashCode();
        BarHash = bar.GetHashCode();
        SingletonVerified = true;
        foo.ClearHistory();
        foo.Log("Toyota Camry (2020)", "Oil Change", "Ion Popescu", 150);
        foo.Log("Toyota Camry (2020)", "Brake Repair", "Ion Popescu", 300);
        bar.Log("Ford F-150 (2021)", "Engine Repair", "Maria Ionescu", 500);
        LogEntries = bar.GetHistory().ToList();
        return Page();
    }
}

public record CloneInfo(string Type, string Make, string Model, int Year, int Hash, string Source);