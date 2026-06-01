using CarsApp.Domain.Builder;
using CarsApp.Domain.Entities;
using CarsApp.Domain.Lab4.Composite;
using CarsApp.Domain.Lab6.Command;
using CarsApp.Domain.Lab6.Iterator;
using CarsApp.Domain.Lab6.Memento;
using CarsApp.Domain.Lab6.Observer;
using CarsApp.Domain.Lab6.Strategy;
using CarsApp.Infrastructure.Lab4.Composite;
using CarsApp.Infrastructure.Lab6.Command;
using CarsApp.Infrastructure.Lab6.Iterator;
using CarsApp.Infrastructure.Lab6.Observer;
using CarsApp.Infrastructure.Lab6.Strategy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarsApp.WebUI.Pages
{
    public class Lab6Model : PageModel
    {
        [BindProperty]
        public string StrategySelectedAlgorithm { get; set; } = "Standard";

        [BindProperty]
        public List<string> StrategySelectedServices { get; set; } = new();

        [BindProperty]
        public bool StrategyIncludesLoanCar { get; set; }

        [BindProperty]
        public string StrategyPriority { get; set; } = "Standard";

        [BindProperty]
        public string IteratorSelectedPackageName { get; set; } = "VIP Package";

        [BindProperty]
        public string IteratorTraversalMode { get; set; } = nameof(ServiceComponentTraversalMode.DepthFirst);

        [BindProperty]
        public string ObserverTargetStatus { get; set; } = nameof(ServiceOrderStatus.InProgress);

        [BindProperty]
        public string CommandScenario { get; set; } = "execute";

        public List<string> StrategyAvailableAlgorithms { get; set; } = new();
        public List<ServiceOption> StrategyAvailableServices { get; set; } = new();
        public List<string> StrategyAvailablePriorities { get; set; } = new();
        public List<PackageOption> IteratorAvailablePackages { get; set; } = new();
        public List<TraversalModeOption> IteratorAvailableModes { get; set; } = new();
        public List<string> ObserverAvailableStatuses { get; set; } = new();
        public List<CommandScenarioOption> CommandScenarioOptions { get; set; } = new();

        public CostEstimate? StrategyEstimate { get; set; }
        public ServiceOrder? StrategyOrder { get; set; }
        public IteratorDemoResult? IteratorResult { get; set; }
        public ObserverDemoResult? ObserverResult { get; set; }
        public CommandDemoResult? CommandResult { get; set; }
        public MementoDemoResult? MementoResult { get; set; }

        public void OnGet()
        {
            InitializeDemoData();
        }

        public IActionResult OnPostStrategy()
        {
            InitializeDemoData(keepEmptyStrategyServices: true);

            if (!StrategySelectedServices.Any())
            {
                ModelState.AddModelError(nameof(StrategySelectedServices), "Selecteaza cel putin un serviciu.");
                return Page();
            }

            StrategyOrder = BuildStrategyOrder();

            var strategy = CreateStrategy(StrategySelectedAlgorithm);
            var calculator = new ServiceCostCalculator(strategy);

            StrategyEstimate = calculator.Calculate(StrategyOrder);

            return Page();
        }

        public IActionResult OnPostIterator()
        {
            InitializeDemoData();

            var packages = ServicePackageCatalog.GetPackages();
            var selectedPackage = packages.FirstOrDefault(p =>
                    string.Equals(p.Name, IteratorSelectedPackageName, StringComparison.OrdinalIgnoreCase))
                ?? packages.First();

            IteratorSelectedPackageName = selectedPackage.Name;
            var traversalMode = ParseTraversalMode(IteratorTraversalMode);
            IteratorTraversalMode = traversalMode.ToString();

            var iterable = new IterableServicePackage(selectedPackage);
            var iterator = iterable.CreateIterator(traversalMode);
            var rows = new List<IteratorRow>();

            while (iterator.MoveNext())
            {
                var current = iterator.Current;
                rows.Add(new IteratorRow(
                    current.Index,
                    current.Component.Name,
                    current.Component.Description,
                    current.Component.Price,
                    current.Component.IsComposite,
                    current.Depth,
                    current.Path));
            }

            IteratorResult = new IteratorDemoResult(
                selectedPackage.Name,
                selectedPackage.Price,
                traversalMode.ToString(),
                rows,
                rows.Count(r => r.IsComposite),
                rows.Count(r => !r.IsComposite));

            return Page();
        }

        public IActionResult OnPostObserver()
        {
            InitializeDemoData();

            var order = CreateDemoOrder(
                new[] { "Oil Change", "Brake Repair" },
                "Standard",
                includesLoanCar: false);

            var subject = new ServiceOrderStatusSubject(order);
            var dashboard = new ReceptionDashboardObserver();
            var customer = new CustomerNotificationObserver();
            var technician = new TechnicianNotificationObserver();

            subject.Attach(dashboard);
            subject.Attach(customer);
            subject.Attach(technician);

            subject.ChangeStatus(ServiceOrderStatus.Scheduled, "Comanda a fost programata la receptie.");

            var targetStatus = ParseOrderStatus(ObserverTargetStatus);
            ObserverTargetStatus = targetStatus.ToString();

            if (targetStatus != ServiceOrderStatus.Scheduled)
            {
                subject.ChangeStatus(targetStatus, BuildObserverMessage(targetStatus));
            }

            ObserverResult = new ObserverDemoResult(
                order.OrderId,
                subject.PreviousStatus.ToString(),
                subject.CurrentStatus.ToString(),
                subject.LastStatusChangedAt,
                dashboard.StatusHistory.ToList(),
                customer.Notifications.ToList(),
                technician.Tasks.ToList());

            return Page();
        }

        public IActionResult OnPostCommand()
        {
            InitializeDemoData();

            var order = CreateDemoOrder(
                new[] { "Oil Change", "Brake Repair" },
                "Standard",
                includesLoanCar: false);

            var receiver = new ServiceOrderCommandReceiver();
            var invoker = new ServiceCommandInvoker();

            var commands = new IServiceOrderCommand[]
            {
                new AddServiceCommand(receiver, order, "Diagnostics"),
                new ChangePriorityCommand(receiver, order, "High"),
                new AssignTechnicianCommand(receiver, order, "Ioana Stan"),
                new ScheduleOrderCommand(receiver, order, DateTime.Today.AddDays(3).AddHours(10))
            };

            foreach (var command in commands)
            {
                invoker.ScheduleCommand(command);
            }

            var title = "Comenzi programate";

            if (CommandScenario is "execute" or "undo" or "redo")
            {
                invoker.ExecuteScheduledCommands();
                title = "Comenzi executate";
            }

            if (CommandScenario is "undo" or "redo")
            {
                invoker.Undo();
                title = "Ultima comanda anulata";
            }

            if (CommandScenario == "redo")
            {
                invoker.Redo();
                title = "Ultima comanda refacuta";
            }

            CommandResult = new CommandDemoResult(
                title,
                CreateSnapshot("Stare comanda", order),
                invoker.History.ToList(),
                invoker.PendingCommandsCount,
                invoker.UndoCount,
                invoker.RedoCount);

            return Page();
        }

        public IActionResult OnPostMemento()
        {
            InitializeDemoData();

            var order = CreateDemoOrder(
                new[] { "Oil Change" },
                "Standard",
                includesLoanCar: false);

            var originator = new ServiceOrderOriginator(order);
            var history = new ServiceOrderHistory(originator);
            var snapshots = new List<OrderSnapshot>();

            history.Backup("Comanda initiala");
            snapshots.Add(CreateSnapshot("1. Initial", order));

            order.Priority = "High";
            order.Services.Add("Engine Repair");
            order.EstimatedCost = 1050m;

            history.Backup("Dupa schimbarea prioritatii si adaugarea motorului");
            snapshots.Add(CreateSnapshot("2. Modificat", order));

            order.TechnicianName = "Radu Ionescu";
            order.IncludesLoanCar = true;
            order.ScheduledDate = DateTime.Today.AddDays(4).AddHours(9);
            snapshots.Add(CreateSnapshot("3. Inainte de Undo", order));

            var undoSucceeded = history.Undo();
            snapshots.Add(CreateSnapshot("4. Dupa Undo", order));

            var redoSucceeded = history.Redo();
            snapshots.Add(CreateSnapshot("5. Dupa Redo", order));

            MementoResult = new MementoDemoResult(
                snapshots,
                history.History.ToList(),
                undoSucceeded,
                redoSucceeded,
                history.UndoCount,
                history.RedoCount);

            return Page();
        }

        private void InitializeDemoData(bool keepEmptyStrategyServices = false)
        {
            StrategyAvailableAlgorithms = new List<string>
            {
                "Standard",
                "Express",
                "Loyalty"
            };

            StrategyAvailablePriorities = new List<string>
            {
                "Standard",
                "High",
                "VIP"
            };

            StrategyAvailableServices = new List<ServiceOption>
            {
                new("Oil Change", 150m),
                new("Brake Repair", 450m),
                new("Engine Repair", 900m),
                new("Electric Engine Repair", 1000m),
                new("Electric Brake Repair", 500m),
                new("Combustion Engine Repair", 950m),
                new("Combustion Brake Repair", 480m)
            };

            if (!keepEmptyStrategyServices && !StrategySelectedServices.Any())
            {
                StrategySelectedServices = new List<string>
                {
                    "Oil Change",
                    "Brake Repair"
                };
            }

            if (string.IsNullOrWhiteSpace(StrategySelectedAlgorithm))
            {
                StrategySelectedAlgorithm = "Standard";
            }

            if (string.IsNullOrWhiteSpace(StrategyPriority))
            {
                StrategyPriority = "Standard";
            }

            var packages = ServicePackageCatalog.GetPackages();
            IteratorAvailablePackages = packages
                .Select(p => new PackageOption(p.Name, p.Description, p.Price))
                .ToList();

            IteratorAvailableModes = new List<TraversalModeOption>
            {
                new(nameof(ServiceComponentTraversalMode.DepthFirst), "Depth First"),
                new(nameof(ServiceComponentTraversalMode.LeafOnly), "Doar servicii finale"),
                new(nameof(ServiceComponentTraversalMode.CompositeOnly), "Doar pachete compuse")
            };

            if (string.IsNullOrWhiteSpace(IteratorSelectedPackageName))
            {
                IteratorSelectedPackageName = "VIP Package";
            }

            if (string.IsNullOrWhiteSpace(IteratorTraversalMode))
            {
                IteratorTraversalMode = nameof(ServiceComponentTraversalMode.DepthFirst);
            }

            ObserverAvailableStatuses = Enum.GetValues<ServiceOrderStatus>()
                .Where(status => status != ServiceOrderStatus.Created)
                .Select(status => status.ToString())
                .ToList();

            if (string.IsNullOrWhiteSpace(ObserverTargetStatus))
            {
                ObserverTargetStatus = nameof(ServiceOrderStatus.InProgress);
            }

            CommandScenarioOptions = new List<CommandScenarioOption>
            {
                new("scheduled", "Doar programeaza"),
                new("execute", "Executa coada"),
                new("undo", "Executa + Undo"),
                new("redo", "Executa + Undo + Redo")
            };

            if (string.IsNullOrWhiteSpace(CommandScenario))
            {
                CommandScenario = "execute";
            }
        }

        private ServiceOrder BuildStrategyOrder()
        {
            return CreateDemoOrder(
                StrategySelectedServices.Distinct(StringComparer.OrdinalIgnoreCase),
                StrategyPriority,
                StrategyIncludesLoanCar);
        }

        private static ServiceOrder CreateDemoOrder(
            IEnumerable<string> services,
            string priority,
            bool includesLoanCar)
        {
            var builder = new ServiceOrderBuilder();

            builder
                .ForVehicle(new Car("Toyota", "Camry", 2020))
                .WithPriority(priority)
                .WithTechnician("Ion Marinescu")
                .ScheduledOn(DateTime.Today.AddDays(2).AddHours(11))
                .WithLoanCar(includesLoanCar)
                .WithNotes("Comanda demo pentru Lab 6.");

            foreach (var service in services)
            {
                builder.AddService(service);
            }

            return builder.GetProduct();
        }

        private static IServiceCostStrategy CreateStrategy(string algorithm)
        {
            return algorithm switch
            {
                "Express" => new ExpressCostStrategy(),
                "Loyalty" => new LoyaltyCostStrategy(),
                _ => new StandardCostStrategy()
            };
        }

        private static ServiceComponentTraversalMode ParseTraversalMode(string value)
        {
            return Enum.TryParse<ServiceComponentTraversalMode>(value, ignoreCase: true, out var mode)
                ? mode
                : ServiceComponentTraversalMode.DepthFirst;
        }

        private static ServiceOrderStatus ParseOrderStatus(string value)
        {
            return Enum.TryParse<ServiceOrderStatus>(value, ignoreCase: true, out var status)
                ? status
                : ServiceOrderStatus.InProgress;
        }

        private static string BuildObserverMessage(ServiceOrderStatus status)
        {
            return status switch
            {
                ServiceOrderStatus.InProgress => "Mecanicul a inceput lucrul la comanda.",
                ServiceOrderStatus.WaitingForParts => "Comanda asteapta piese de schimb.",
                ServiceOrderStatus.Completed => "Comanda a fost finalizata si poate fi preluata.",
                ServiceOrderStatus.Cancelled => "Comanda a fost anulata la solicitarea clientului.",
                _ => "Statusul comenzii a fost actualizat."
            };
        }

        private static OrderSnapshot CreateSnapshot(string label, ServiceOrder order)
        {
            var vehicle = order.Vehicle == null
                ? "Vehicul necunoscut"
                : $"{order.Vehicle.Make} {order.Vehicle.Model} ({order.Vehicle.Year})";

            return new OrderSnapshot(
                label,
                order.OrderId,
                vehicle,
                order.Priority,
                order.TechnicianName,
                order.ScheduledDate,
                order.EstimatedCost,
                order.IncludesLoanCar,
                order.Services.ToList());
        }
    }

    public record ServiceOption(string Name, decimal Price);
    public record PackageOption(string Name, string Description, decimal Price);
    public record TraversalModeOption(string Value, string Label);
    public record CommandScenarioOption(string Value, string Label);

    public record IteratorRow(
        int Index,
        string Name,
        string Description,
        decimal Price,
        bool IsComposite,
        int Depth,
        string Path);

    public record IteratorDemoResult(
        string PackageName,
        decimal PackagePrice,
        string TraversalMode,
        List<IteratorRow> Rows,
        int CompositeCount,
        int LeafCount);

    public record ObserverDemoResult(
        string OrderId,
        string PreviousStatus,
        string CurrentStatus,
        DateTime ChangedAt,
        List<string> DashboardHistory,
        List<string> CustomerNotifications,
        List<string> TechnicianTasks);

    public record OrderSnapshot(
        string Label,
        string OrderId,
        string Vehicle,
        string Priority,
        string TechnicianName,
        DateTime ScheduledDate,
        decimal EstimatedCost,
        bool IncludesLoanCar,
        List<string> Services);

    public record CommandDemoResult(
        string ScenarioTitle,
        OrderSnapshot Order,
        List<string> History,
        int PendingCount,
        int UndoCount,
        int RedoCount);

    public record MementoDemoResult(
        List<OrderSnapshot> Snapshots,
        List<string> StoredSnapshots,
        bool UndoSucceeded,
        bool RedoSucceeded,
        int UndoCount,
        int RedoCount);
}
