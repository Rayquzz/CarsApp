using CarsApp.Domain.Lab4.Composite;
using CarsApp.Infrastructure.Lab4.Composite;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class PacheteModel : PageModel
{
    public List<PackageViewModel> Packages { get; set; } = new();

    public void OnGet()
    {
        var catalog = ServicePackageCatalog.GetPackages();
        Packages = catalog.Select(p => new PackageViewModel(p)).ToList();
    }
}

public class PackageViewModel
{
    public string Name { get; }
    public string Description { get; }
    public decimal Price { get; }
    public List<ServiceItemViewModel> FlatItems { get; }

    private static readonly string[] Colors =
    {
        "#4ade80", "#60a5fa", "#c4b5fd", "#fb923c", "#f472b6"
    };

    public PackageViewModel(IServiceComponent component)
    {
        Name = component.Name;
        Description = component.Description;
        Price = component.Price;
        FlatItems = new List<ServiceItemViewModel>();
        Flatten(component, 0);
    }

    // Traverseaza recursiv ierarhia Composite
    private void Flatten(IServiceComponent component, int depth)
    {
        if (!component.IsComposite)
        {
            FlatItems.Add(new ServiceItemViewModel(
                component.Name,
                component.Price,
                Colors[FlatItems.Count % Colors.Length]
            ));
            return;
        }
        foreach (var child in component.Children)
            Flatten(child, depth + 1);
    }
}

public record ServiceItemViewModel(string Name, decimal Price, string Color);