using CarsApp.application.Interfaces;
using CarsApp.Domain.Entities;
using CarsApp.Infrastructure.Factories.AbstractFactory;
using CarsApp.Infrastructure.Factories.FactoryMethod;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class Lab2Model : PageModel
{
    private readonly IServiceManager _manager;
    private static readonly Vehicle _vehicle = new Car("Toyota", "Camry", 2020);

    public FactoryMethodResult? FactoryResult { get; set; }
    public AbstractFactoryResult? AbstractResult { get; set; }

    public Lab2Model(IServiceManager manager) => _manager = manager;
    public void OnGet() { }

    public IActionResult OnPostFactoryMethod(string factoryType)
    {
        ServiceFactory factory = factoryType switch
        {
            "brake" => new BrakeRepairFactory(),
            "oil" => new OilChangeFactory(),
            _ => new EngineRepairFactory()
        };
        var service = factory.CreateService();
        FactoryResult = new FactoryMethodResult(
            factory.GetType().Name,
            service.Name,
            _vehicle.Make,
            _vehicle.Model,
            _vehicle.Year
        );
        return Page();
    }

    public IActionResult OnPostAbstractFactory(string familyType)
    {
        IServicePackageFactory factory = familyType == "electric"
            ? new ElectricServiceFactory()
            : new CombustionServiceFactory();

        AbstractResult = new AbstractFactoryResult(
            familyType == "electric" ? "Electric" : "Combustion",
            factory.GetType().Name,
            factory.CreateEngineRepair().Name,
            factory.CreateBrakeRepair().Name
        );
        return Page();
    }
}

public record FactoryMethodResult(
    string FactoryName, string ServiceName,
    string VehicleMake, string VehicleModel, int VehicleYear);

public record AbstractFactoryResult(
    string Family, string FactoryName,
    string EngineService, string BrakeService);