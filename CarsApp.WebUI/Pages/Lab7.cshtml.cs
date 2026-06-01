using CarsApp.Domain.Builder;
using CarsApp.Domain.Entities;
using CarsApp.Domain.Lab7.ChainOfResponsibility;
using CarsApp.Domain.Lab7.Mediator;
using CarsApp.Domain.Lab7.State;
using CarsApp.Domain.Lab7.TemplateMethod;
using CarsApp.Domain.Lab7.Visitor;
using CarsApp.Infrastructure.Lab7.ChainOfResponsibility;
using CarsApp.Infrastructure.Lab7.Mediator;
using CarsApp.Infrastructure.Lab7.State;
using CarsApp.Infrastructure.Lab7.TemplateMethod;
using CarsApp.Infrastructure.Lab7.Visitor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarsApp.WebUI.Pages
{
    public class Lab7Model : PageModel
    {
        [BindProperty]
        public string? ChainRequestedService { get; set; } = "Brake Repair";

        [BindProperty]
        public string? ChainComplexity { get; set; } = nameof(ServiceRequestComplexity.High);

        [BindProperty]
        public decimal ChainEstimatedCost { get; set; } = 650m;

        [BindProperty]
        public bool ChainIsVipCustomer { get; set; }

        [BindProperty]
        public bool ChainRequiresManagerApproval { get; set; }

        [BindProperty]
        public string? StateScenario { get; set; } = "complete";

        [BindProperty]
        public string? MediatorScenario { get; set; } = "parts-available";

        [BindProperty]
        public string? TemplateReportType { get; set; } = "Customer Service Report";

        [BindProperty]
        public string? VisitorExportFormat { get; set; } = "CSV";

        public List<string> ChainComplexities { get; set; } = new();
        public List<ScenarioOption> StateScenarios { get; set; } = new();
        public List<ScenarioOption> MediatorScenarios { get; set; } = new();
        public List<string> TemplateReportTypes { get; set; } = new();
        public List<string> VisitorExportFormats { get; set; } = new();

        public ServiceRequestResult? ChainResult { get; set; }
        public StateDemoResult? StateResult { get; set; }
        public MediatorDemoResult? MediatorResult { get; set; }
        public TemplateDemoResult? TemplateResult { get; set; }
        public VisitorDemoResult? VisitorResult { get; set; }

        public void OnGet()
        {
            InitializeDemoData();
        }

        public IActionResult OnPostChain()
        {
            InitializeDemoData();

            if (string.IsNullOrWhiteSpace(ChainRequestedService))
            {
                ModelState.AddModelError(nameof(ChainRequestedService), "Introdu un serviciu cerut.");
                return Page();
            }

            if (ChainEstimatedCost < 0)
            {
                ModelState.AddModelError(nameof(ChainEstimatedCost), "Costul estimat nu poate fi negativ.");
                return Page();
            }

            var request = new ServiceRequest(
                "Ion Popescu",
                "Toyota Camry 2020",
                ChainRequestedService.Trim(),
                ParseComplexity(ChainComplexity),
                ChainEstimatedCost,
                ChainIsVipCustomer,
                ChainRequiresManagerApproval);

            var chain = ServiceRequestChainFactory.CreateDefaultChain();
            ChainResult = chain.Handle(request) ??
                new ServiceRequestResult(
                    "NoHandler",
                    "N/A",
                    "Cererea nu a fost procesata de niciun handler din lant.",
                    RequiresFollowUp: true);

            return Page();
        }

        public IActionResult OnPostState()
        {
            InitializeDemoData();

            var order = CreateDemoOrder();
            var workflow = ServiceOrderWorkflowFactory.Create(order);
            var actions = new List<StateActionResult>
            {
                workflow.Schedule(DateTime.Today.AddDays(2).AddHours(9)),
                workflow.StartWork("Ioana Stan")
            };

            switch (StateScenario)
            {
                case "wait":
                    actions.Add(workflow.WaitForParts("Discuri de frana indisponibile."));
                    break;
                case "resume-complete":
                    actions.Add(workflow.WaitForParts("Filtru special indisponibil."));
                    actions.Add(workflow.StartWork("Ioana Stan"));
                    actions.Add(workflow.Complete("Lucrare finalizata dupa sosirea pieselor."));
                    break;
                case "cancel":
                    actions.Add(workflow.Cancel("Clientul a amanat reparatia."));
                    break;
                default:
                    StateScenario = "complete";
                    actions.Add(workflow.Complete("Test drive finalizat fara erori."));
                    break;
            }

            StateResult = new StateDemoResult(
                workflow.CurrentStateName,
                workflow.CurrentStatus.ToString(),
                actions,
                workflow.History.ToList());

            return Page();
        }

        public IActionResult OnPostMediator()
        {
            InitializeDemoData();

            var mediator = WorkshopMediatorFactory.CreateDefault(
                out var receptionDesk,
                out var technicianTeam,
                out var partsDepartment,
                out var notificationCenter);

            receptionDesk.CreateServiceRequest(
                "Ion Popescu",
                "Toyota Camry 2020",
                "Brake Repair");

            switch (MediatorScenario)
            {
                case "request-only":
                    break;
                case "parts-missing":
                    technicianTeam.RequestParts("Brake pads");
                    break;
                case "complete":
                    partsDepartment.AddStock("Brake pads", 1);
                    technicianTeam.RequestParts("Brake pads");
                    technicianTeam.CompleteRepair("Front brake pads replaced and road test passed.");
                    break;
                default:
                    MediatorScenario = "parts-available";
                    partsDepartment.AddStock("Brake pads", 1);
                    technicianTeam.RequestParts("Brake pads");
                    break;
            }

            MediatorResult = new MediatorDemoResult(
                mediator.CoordinationLog.ToList(),
                notificationCenter.Notifications.ToList(),
                technicianTeam.WorkNotes.ToList(),
                technicianTeam.AssignedJobs.ToList(),
                receptionDesk.CheckoutPreparations.ToList());

            return Page();
        }

        public IActionResult OnPostTemplate()
        {
            InitializeDemoData();

            var order = CreateDemoOrder();
            var template = CreateReportTemplate(TemplateReportType);
            TemplateReportType = template.ReportType;

            var report = template.Generate(order);
            TemplateResult = new TemplateDemoResult(
                report.ReportType,
                report.Sections.ToList(),
                order.EstimatedCost);

            return Page();
        }

        public IActionResult OnPostVisitor()
        {
            InitializeDemoData();

            var document = ServiceExportDocumentFactory.CreateSampleInvoice();
            VisitorExportFormat = NormalizeExportFormat(VisitorExportFormat);
            var visitor = CreateExportVisitor(VisitorExportFormat);
            var output = document.Export(visitor);

            VisitorResult = new VisitorDemoResult(
                VisitorExportFormat,
                visitor.GetType().Name,
                document.Elements.Count,
                output);

            return Page();
        }

        private void InitializeDemoData()
        {
            ChainComplexities = Enum.GetValues<ServiceRequestComplexity>()
                .Select(value => value.ToString())
                .ToList();

            if (string.IsNullOrWhiteSpace(ChainComplexity))
            {
                ChainComplexity = nameof(ServiceRequestComplexity.High);
            }

            if (string.IsNullOrWhiteSpace(ChainRequestedService))
            {
                ChainRequestedService = "Brake Repair";
            }

            StateScenarios = new List<ScenarioOption>
            {
                new("complete", "Created -> Completed"),
                new("wait", "Pauza pentru piese"),
                new("resume-complete", "Piese sosite + finalizare"),
                new("cancel", "Anulare din lucru")
            };

            if (string.IsNullOrWhiteSpace(StateScenario))
            {
                StateScenario = "complete";
            }

            MediatorScenarios = new List<ScenarioOption>
            {
                new("request-only", "Cerere noua"),
                new("parts-available", "Piese disponibile"),
                new("parts-missing", "Piese indisponibile"),
                new("complete", "Reparatie finalizata")
            };

            if (string.IsNullOrWhiteSpace(MediatorScenario))
            {
                MediatorScenario = "parts-available";
            }

            TemplateReportTypes = ServiceReportTemplateFactory
                .CreateDefaultReports()
                .Select(report => report.ReportType)
                .ToList();

            if (string.IsNullOrWhiteSpace(TemplateReportType))
            {
                TemplateReportType = "Customer Service Report";
            }

            VisitorExportFormats = new List<string>
            {
                "CSV",
                "XML",
                "PDF"
            };

            if (string.IsNullOrWhiteSpace(VisitorExportFormat))
            {
                VisitorExportFormat = "CSV";
            }
        }

        private static ServiceRequestComplexity ParseComplexity(string? value)
        {
            return Enum.TryParse<ServiceRequestComplexity>(value, ignoreCase: true, out var complexity)
                ? complexity
                : ServiceRequestComplexity.High;
        }

        private static ServiceOrder CreateDemoOrder()
        {
            var builder = new ServiceOrderBuilder();

            builder
                .ForVehicle(new Car("Toyota", "Camry", 2020))
                .AddService("Brake Repair")
                .AddService("Diagnostics")
                .WithPriority("High")
                .WithTechnician("Ioana Stan")
                .ScheduledOn(DateTime.Today.AddDays(2).AddHours(9))
                .WithLoanCar(true)
                .WithEstimatedCost(780m)
                .WithNotes("Comanda demo pentru Lab 7.");

            return builder.GetProduct();
        }

        private static ServiceReportTemplate CreateReportTemplate(string? reportType)
        {
            var reports = ServiceReportTemplateFactory.CreateDefaultReports();

            return reports.FirstOrDefault(report =>
                    string.Equals(report.ReportType, reportType, StringComparison.OrdinalIgnoreCase))
                ?? reports.First();
        }

        private static IDocumentExportVisitor CreateExportVisitor(string? format)
        {
            return format switch
            {
                "XML" => new XmlDocumentExportVisitor(),
                "PDF" => new PdfDocumentExportVisitor(),
                _ => new CsvDocumentExportVisitor()
            };
        }

        private static string NormalizeExportFormat(string? format)
        {
            return format switch
            {
                "XML" => "XML",
                "PDF" => "PDF",
                _ => "CSV"
            };
        }
    }

    public record ScenarioOption(string Value, string Label);

    public record StateDemoResult(
        string CurrentState,
        string CurrentStatus,
        List<StateActionResult> Actions,
        List<string> History);

    public record MediatorDemoResult(
        List<WorkshopCoordinationEntry> CoordinationLog,
        List<string> Notifications,
        List<string> WorkNotes,
        List<string> AssignedJobs,
        List<string> CheckoutPreparations);

    public record TemplateDemoResult(
        string ReportType,
        List<string> Sections,
        decimal OrderCost);

    public record VisitorDemoResult(
        string Format,
        string VisitorName,
        int ElementCount,
        string Output);
}
