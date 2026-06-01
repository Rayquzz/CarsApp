using CarsApp.Infrastructure.Lab5.Flyweight;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarsApp.Domain.Lab5.Decorator;
using CarsApp.Infrastructure.Lab5.Decorator;
using CarsApp.Domain.Lab5.Bridge;
using CarsApp.Infrastructure.Lab5.Bridge.Renderers;
using CarsApp.Infrastructure.Lab5.Bridge.Reports;
using CarsApp.Domain.Lab5.Proxy;
using CarsApp.Infrastructure.Lab5.Proxy;

public class Lab5Model : PageModel
{
    public FlyweightResult? FwResult { get; set; }
    public DecoratorResult? DecResult { get; set; }
    public BridgeResult? BrResult { get; set; }
    private static CachedHistoryProxy? _cachedProxy;
    private static LoggingHistoryProxy? _loggingProxy;
    public ProxyResult? PxResult { get; set; }

    public void OnGet() { }

    public IActionResult OnPostFlyweight()
    {
        var factory = new SparePartFactory();
        var entries = new List<StockEntry>();

        // Date de test — 5 tipuri de piese, 2000 intrări în stoc
        var catalog = new[]
        {
            ("FLT-001", "Filtru Ulei",    "Bosch",     "Filtre"),
            ("FLT-002", "Filtru Aer",     "Mann",      "Filtre"),
            ("BRK-001", "Plăcuțe Frână", "Brembo",    "Frâne"),
            ("SPK-001", "Bujii",          "NGK",       "Aprindere"),
            ("BLT-001", "Curea Distribuție","Gates",   "Transmisie"),
        };

        var rng = new Random(42);
        var locations = new[] { "Raft A1", "Raft A2", "Raft B1", "Raft B2", "Raft C1" };

        // Simulăm 2000 de intrări în stoc
        for (int i = 0; i < 2000; i++)
        {
            var (code, name, manuf, cat) = catalog[rng.Next(catalog.Length)];
            var flyweight = factory.GetOrCreate(code, name, manuf, cat);
            entries.Add(new StockEntry(
                flyweight,
                quantity: rng.Next(1, 50),
                location: locations[rng.Next(locations.Length)],
                price: rng.Next(20, 500)
            ));
        }

        // Verificare că aceleași coduri → aceeași referință
        var oil1 = factory.GetOrCreate("FLT-001", "Filtru Ulei", "Bosch", "Filtre");
        var oil2 = factory.GetOrCreate("FLT-001", "Filtru Ulei", "Bosch", "Filtre");

        FwResult = new FlyweightResult(
            TotalEntries: entries.Count,
            UniqueObjects: factory.PoolSize,
            SameReference: ReferenceEquals(oil1, oil2),
            MemorySaved: entries.Count - factory.PoolSize,
            SampleEntries: entries.Take(8).Select(e => new StockRow(
                                e.PartCode, e.GetLabel(),
                                e.Price, e.FlyweightHashCode)).ToList(),
            PoolContents: factory.Pool.Values
                                .Select(p => new PoolRow(p.PartCode, p.Name,
                                            p.Manufacturer, p.GetHashCode()))
                                .ToList()
        );

        return Page();
    }
    public IActionResult OnPostDecorator(
    string baseService,
    bool withWarranty, bool withDiscount,
    bool withReport, bool withSms)
    {
        // 1. Alege serviciul de bază
        IDecoratedService service = baseService switch
        {
            "brake" => new BaseServiceOperation("Reparație Frâne", "Înlocuire plăcuțe și discuri", 350m),
            "engine" => new BaseServiceOperation("Reparație Motor", "Diagnosticare și reparare", 800m),
            "oil" => new BaseServiceOperation("Schimb Ulei", "Ulei + filtru nou", 150m),
            _ => new BaseServiceOperation("Inspecție Generală", "Verificare completă", 100m)
        };

        var steps = new List<string> { $"Serviciu de bază: {service.Name} — {service.Cost:C}" };

        // 2. Înlănțuire decoratori — ordinea contează!
        if (withDiscount) { service = new LoyaltyDiscountDecorator(service); steps.Add($"+ Discount 10% → {service.Cost:C}"); }
        if (withWarranty) { service = new WarrantyDecorator(service); steps.Add($"+ Garanție 12L → {service.Cost:C}"); }
        if (withReport) { service = new DetailedReportDecorator(service); steps.Add($"+ Raport PDF  → {service.Cost:C}"); }
        if (withSms) { service = new SmsNotificationDecorator(service); steps.Add($"+ SMS        → {service.Cost:C}"); }

        // 3. Execută lanțul
        var output = service.Execute("Toyota Camry (2020)");

        DecResult = new DecoratorResult(
            FinalName: service.Name,
            Description: service.Description,
            FinalCost: service.Cost,
            Steps: steps,
            Output: output.Split('\n').ToList()
        );

        return Page();
    }
    public IActionResult OnPostBridge(string reportType, string rendererType)
    {
        // 1. Alege implementorul (renderer)
        IReportRenderer renderer = rendererType switch
        {
            "html" => new HtmlReportRenderer(),
            "plain" => new PlainTextReportRenderer(),
            _ => new ConsoleReportRenderer()
        };

        // 2. Alege abstracția (tipul de raport)
        ServiceReport report = reportType switch
        {
            "detailed" => new DetailedServiceReport(renderer),
            "financial" => new FinancialServiceReport(renderer),
            _ => new SummaryServiceReport(renderer)
        };

        // 3. Date comune — aceleași indiferent de combinație
        var data = new ReportData(
            OrderId: "ORD-" + Guid.NewGuid().ToString("N")[..6].ToUpper(),
            CustomerName: "Alexandru Popa",
            VehicleInfo: "Toyota Camry (2020)",
            TechnicianName: "Ion Marinescu",
            Services: new List<string> { "Schimb Ulei", "Reparație Frâne", "Inspecție Generală" },
            TotalCost: 650m,
            ScheduledDate: DateTime.Today.AddDays(2),
            Priority: "Standard"
        );

        // 4. Generează — bridge-ul face conexiunea automat
        var output = report.Generate(data);

        BrResult = new BridgeResult(
            ReportType: report.ReportType,
            RendererName: renderer.RendererName,
            Output: output,
            IsHtml: rendererType == "html"
        );

        return Page();
    }
    public IActionResult OnPostProxy(string vehicleMake, string userRole, bool invalidateCache)
    {
        // Construim lanțul O SINGURĂ DATĂ (sau la invalidare)
        // Lanț: AuthProxy → LoggingProxy → CachedProxy → RealService
        if (_cachedProxy == null || invalidateCache)
        {
            var real = new RealVehicleHistoryService();
            _cachedProxy = new CachedHistoryProxy(real, TimeSpan.FromMinutes(2));
            _loggingProxy = new LoggingHistoryProxy(_cachedProxy);
        }

        var authProxy = new AuthHistoryProxy(_loggingProxy!);

        // Apelul trece prin toți cei 3 proxy înainte să ajungă la serviciul real
        var result = authProxy.GetHistory(vehicleMake, "Model", 2020, userRole);

        PxResult = new ProxyResult(
            Success: result.Success,
            Data: result.Data,
            AccessLog: result.AccessLog,
            CacheStatus: result.CacheStatus,
            FromCache: result.FromCache,
            DeniedReason: result.DeniedReason,
            CacheHits: _cachedProxy.CacheHits,
            CacheMisses: _cachedProxy.CacheMisses,
            AuditTrail: _loggingProxy!.GetAuditTrail()
        );

        return Page();
    }
}

public record FlyweightResult(
    int TotalEntries, int UniqueObjects, bool SameReference,
    int MemorySaved,
    List<StockRow> SampleEntries,
    List<PoolRow> PoolContents);

// Adaugă recordul la final:
public record DecoratorResult(
    string FinalName, string Description, decimal FinalCost,
    List<string> Steps, List<string> Output);

public record BridgeResult(
    string ReportType, string RendererName,
    string Output, bool IsHtml);

public record ProxyResult(
    bool Success,
    string Data,
    string AccessLog,
    string CacheStatus,
    bool FromCache,
    string DeniedReason,
    int CacheHits,
    int CacheMisses,
    List<string> AuditTrail);

public record StockRow(string Code, string Label, decimal Price, int FwHash);
public record PoolRow(string Code, string Name, string Manufacturer, int Hash);