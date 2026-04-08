using CarsApp.Domain.Lab4.Composite;
using CarsApp.Domain.Lab4.Facade;
using CarsApp.Infrastructure.Lab4.Composite;
using CarsApp.Infrastructure.Lab4.Facade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class ProgramareModel : PageModel
{
    public List<PackageSummary> AvailablePackages { get; set; } = new();
    public string SelectedPackage { get; set; } = "";
    public ReceptionResult? Result { get; set; }

    public decimal ResultPrice { get; set; } = 0;

    public void OnGet(string? package)
    {
        LoadPackages();
        SelectedPackage = package ?? AvailablePackages.FirstOrDefault()?.Name ?? "";
    }

    public IActionResult OnPostConfirma(
        string customerName, string vehicleMake,
        string vehicleModel, int vehicleYear,
        string packageName)
    {
        LoadPackages();
        SelectedPackage = packageName;

        var facade = new ServiceReceptionFacade();
        Result = facade.CheckInVehicle(
            customerName, vehicleMake,
            vehicleModel, vehicleYear,
            packageName);
        ResultPrice = ServicePackageCatalog
            .GetPackages()
            .FirstOrDefault(p => p.Name == packageName)?.Price ?? 0;


        return Page();

    }

    private void LoadPackages()
    {
        AvailablePackages = ServicePackageCatalog
            .GetPackages()
            .Select(p => new PackageSummary(p.Name, p.Price))
            .ToList();
    }
}

public record PackageSummary(string Name, decimal Price);
