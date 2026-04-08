using CarsApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class Lab1Model : PageModel
{
    public List<VehicleInfo> Vehicles { get; set; } = new();

    public void OnGet() { }

    public IActionResult OnPostShowVehicles()
    {
        var list = new List<Vehicle>
        {
            new Car("Toyota", "Camry", 2020),
            new Truck("Ford", "F-150", 2021, 3000),
            new Motorcycle("Honda", "CBR", 2019, true)
        };

        Vehicles = list.Select(v => new VehicleInfo(
            v.GetVehicleType(),
            v.Make,
            v.Model,
            v.Year,
            v.GetVehicleType() switch
            {
                "Car" => "tag-green",
                "Truck" => "tag-orange",
                "Motorcycle" => "tag-purple",
                _ => "tag-blue"
            }
        )).ToList();

        return Page();
    }
}

public record VehicleInfo(
    string Type, string Make, string Model,
    int Year, string TagClass);