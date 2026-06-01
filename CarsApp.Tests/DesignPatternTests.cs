using CarsApp.Domain.Builder;
using CarsApp.Domain.Entities;
using CarsApp.Domain.Lab4.Adapter;
using CarsApp.Domain.Lab4.Composite;
using CarsApp.Infrastructure.Builder;
using CarsApp.Infrastructure.Lab4.Adapter;
using CarsApp.Infrastructure.Lab4.Composite;
using CarsApp.Infrastructure.Lab4.Facade;
using CarsApp.Infrastructure.Singleton;
using CarsApp.Domain.Lab4.Facade;
using CarsApp.Domain.Lab6.Command;
using CarsApp.Domain.Lab6.Iterator;
using CarsApp.Domain.Lab6.Memento;
using CarsApp.Domain.Lab6.Observer;
using CarsApp.Domain.Lab6.Strategy;
using CarsApp.Infrastructure.Lab6.Command;
using CarsApp.Infrastructure.Lab6.Iterator;
using CarsApp.Infrastructure.Lab6.Observer;
using CarsApp.Infrastructure.Lab6.Strategy;
using Xunit;

namespace CarsApp.Tests
{
    public class DesignPatternTests
    {
        // ─────────────────────────────────────────
        // BUILDER TESTS
        // ─────────────────────────────────────────

        [Fact]
        public void Builder_SetsPriority_Correctly()
        {
            // Arrange — pregătești obiectele
            var builder = new ServiceOrderBuilder();
            var vehicle = new Car("Toyota", "Camry", 2020);

            // Act — execuți
            builder.ForVehicle(vehicle)
                   .WithPriority("Urgent");
            var order = builder.GetProduct();

            // Assert — verifici
            Assert.Equal("Urgent", order.Priority);
        }

        [Fact]
        public void Builder_AddsServices_Correctly()
        {
            // Arrange
            var builder = new ServiceOrderBuilder();
            var vehicle = new Car("Toyota", "Camry", 2020);

            // Act
            builder.ForVehicle(vehicle)
                   .AddService("Oil Change")
                   .AddService("Brake Repair");
            var order = builder.GetProduct();

            // Assert
            Assert.Contains("Oil Change", order.Services);
            Assert.Contains("Brake Repair", order.Services);
            Assert.Equal(2, order.Services.Count);
        }

        [Fact]
        public void Builder_SetsEstimatedCost_Correctly()
        {
            // Arrange
            var builder = new ServiceOrderBuilder();
            var vehicle = new Car("Toyota", "Camry", 2020);

            // Act
            builder.ForVehicle(vehicle)
                   .WithEstimatedCost(500m);
            var order = builder.GetProduct();

            // Assert
            Assert.Equal(500m, order.EstimatedCost);
        }

        // ─────────────────────────────────────────
        // PROTOTYPE TESTS
        // ─────────────────────────────────────────

        [Fact]
        public void Prototype_Clone_IsDifferentObject()
        {
            // Arrange
            var original = new Car("Toyota", "Camry", 2020);

            // Act
            var clone = original.Clone();

            // Assert — obiecte diferite în memorie
            Assert.False(ReferenceEquals(original, clone));
        }

        [Fact]
        public void Prototype_Clone_HasSameValues()
        {
            // Arrange
            var original = new Car("Toyota", "Camry", 2020);

            // Act
            var clone = original.Clone();

            // Assert — dar cu aceleași valori
            Assert.Equal(original.Make, clone.Make);
            Assert.Equal(original.Model, clone.Model);
            Assert.Equal(original.Year, clone.Year);
        }

        [Fact]
        public void Prototype_PolymorphicClone_PreservesType()
        {
            // Arrange — lucrezi cu tipul abstract Vehicle
            Vehicle vehicle = new Truck("Ford", "F-150", 2022, 3000);

            // Act
            var clone = vehicle.Clone();

            // Assert — clona e tot Truck, nu Vehicle generic
            Assert.IsType<Truck>(clone);
        }

        // ─────────────────────────────────────────
        // SINGLETON TESTS
        // ─────────────────────────────────────────

        [Fact]
        public void Singleton_Instance_IsSameObject()
        {
            // Act
            var foo = ServiceLogger.Instance;
            var bar = ServiceLogger.Instance;

            // Assert — același obiect în memorie
            Assert.True(ReferenceEquals(foo, bar));
        }

        [Fact]
        public void Singleton_LogsAccumulate_InSameInstance()
        {
            // Arrange
            var foo = ServiceLogger.Instance;
            var bar = ServiceLogger.Instance;
            foo.ClearHistory(); // resetezi pentru test curat

            // Act
            foo.Log("Toyota Camry", "Oil Change", "Ion", 150m);
            bar.Log("Ford F-150", "Engine Repair", "Maria", 500m);

            // Assert — bar vede și log-urile lui foo
            Assert.Equal(2, bar.GetHistory().Count);
        }

        // ============================================================
        // LAB 4 — Structural Patterns
        // ============================================================

        // ── Composite ───────────────────────────────────────────────

        [Fact]
        public void Composite_SingleService_ReturnsCorrectPrice()
        {
            var service = new SingleService("Oil Change", "Schimb ulei", 150m);
            Assert.Equal(150m, service.Price);
            Assert.False(service.IsComposite);
            Assert.Empty(service.Children);
        }

        [Fact]
        public void Composite_ServicePackage_CalculatesTotalPriceRecursively()
        {
            var oil = new SingleService("Oil Change", "Schimb ulei", 150m);
            var brake = new SingleService("Brake Repair", "Frâne", 250m);
            var tire = new SingleService("Tire Check", "Anvelope", 80m);

            var basic = new ServicePackage("Basic", "Pachet basic")
                .Add(oil)
                .Add(tire);

            var standard = new ServicePackage("Standard", "Pachet standard")
                .Add(basic)
                .Add(brake);

            // Basic = 150 + 80 = 230
            Assert.Equal(230m, basic.Price);
            // Standard = Basic(230) + Brake(250) = 480
            Assert.Equal(480m, standard.Price);
        }

        [Fact]
        public void Composite_ServicePackage_IsComposite_ReturnsTrue()
        {
            var package = new ServicePackage("Test", "Test package");
            Assert.True(package.IsComposite);
        }

        [Fact]
        public void Composite_Catalog_ReturnsPackages()
        {
            var packages = ServicePackageCatalog.GetPackages();
            Assert.NotEmpty(packages);
            Assert.All(packages, p => Assert.True(p.Price > 0));
        }

        [Fact]
        public void Composite_VipPackage_PriceEqualsSubPackagesSum()
        {
            var packages = ServicePackageCatalog.GetPackages();
            var vip = packages.FirstOrDefault(p => p.Name == "VIP Package");
            Assert.NotNull(vip);
            Assert.True(vip.IsComposite);
            // Pretul VIP trebuie sa fie suma copiilor
            var expectedPrice = vip.Children.Sum(c => c.Price);
            Assert.Equal(expectedPrice, vip.Price);
        }

        // ── Adapter ─────────────────────────────────────────────────

        [Fact]
        public void Adapter_StripeAdapter_ProcessesPaymentSuccessfully()
        {
            IPaymentProvider provider = new StripeAdapter();
            var result = provider.ProcessPayment("Ion Popescu", 500m);

            Assert.Equal("Stripe", provider.ProviderName);
            Assert.True(result.Success);
            Assert.NotNull(result.TransactionId);
            Assert.StartsWith("stripe_", result.TransactionId, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Adapter_PayPalAdapter_ProcessesPaymentSuccessfully()
        {
            IPaymentProvider provider = new PayPalAdapter();
            var result = provider.ProcessPayment("Maria Ionescu", 300m);

            Assert.Equal("PayPal", provider.ProviderName);
            Assert.True(result.Success);
            Assert.NotNull(result.TransactionId);
        }

        [Fact]
        public void Adapter_BankCardAdapter_ProcessesPaymentSuccessfully()
        {
            IPaymentProvider provider = new BankCardAdapter();
            var result = provider.ProcessPayment("Andrei Marin", 850m);

            Assert.Equal("Bank Card", provider.ProviderName);
            Assert.True(result.Success);
            Assert.NotNull(result.TransactionId);
        }

        [Fact]
        public void Adapter_AllProviders_ReturnSameInterface()
        {
            var providers = new List<IPaymentProvider>
    {
        new StripeAdapter(),
        new PayPalAdapter(),
        new BankCardAdapter()
    };

            // Clientul trateaza toti providerii uniform prin IPaymentProvider
            foreach (var provider in providers)
            {
                var result = provider.ProcessPayment("Test Client", 100m);
                Assert.True(result.Success);
                Assert.NotEmpty(provider.ProviderName);
                Assert.NotEmpty(result.TransactionId);
            }
        }

        // ── Façade ───────────────────────────────────────────────────

        [Fact]
        public void Facade_CheckInVehicle_ReturnsSuccessResult()
        {
            var facade = new ServiceReceptionFacade();
            var result = facade.CheckInVehicle(
                "Ion Popescu", "Toyota", "Camry", 2020, "Basic Package");

            Assert.True(result.Success);
            Assert.NotEmpty(result.OrderId);
            Assert.StartsWith("ORD-", result.OrderId);
        }

        [Fact]
        public void Facade_CheckInVehicle_AssignsTechnician()
        {
            var facade = new ServiceReceptionFacade();
            var result = facade.CheckInVehicle(
                "Maria Ionescu", "Ford", "F-150", 2021, "Standard Package");

            Assert.NotEmpty(result.AssignedTechnician);
        }

        [Fact]
        public void Facade_CheckInVehicle_VipPackage_AssignsSeniorTechnician()
        {
            var facade = new ServiceReceptionFacade();
            var result = facade.CheckInVehicle(
                "Alexandru Dumitrescu", "BMW", "X5", 2022, "VIP Package");

            Assert.Equal("Alexandru Constantin", result.AssignedTechnician);
        }

        [Fact]
        public void Facade_CheckInVehicle_SetsScheduledDate()
        {
            var facade = new ServiceReceptionFacade();
            var result = facade.CheckInVehicle(
                "Elena Pop", "Audi", "A4", 2019, "Basic Package");

            Assert.NotEmpty(result.ScheduledDate);
            // Data trebuie sa fie in viitor
            var date = DateTime.ParseExact(result.ScheduledDate, "dd.MM.yyyy", null);
            Assert.True(date > DateTime.Today);
        }

        [Fact]
        public void Facade_CheckInVehicle_SendsNotification()
        {
            var facade = new ServiceReceptionFacade();
            var result = facade.CheckInVehicle(
                "Ion Popescu", "Toyota", "Camry", 2020, "VIP Package");

            Assert.NotEmpty(result.NotificationMessage);
            Assert.Contains("Ion Popescu", result.NotificationMessage);
            Assert.Contains(result.OrderId, result.NotificationMessage);
        }

        [Fact]
        public void Facade_HidesSubsystemComplexity()
        {
            // Clientul apeleaza o singura metoda — Facade orchestreaza restul
            var facade = new ServiceReceptionFacade();
            var result = facade.CheckInVehicle(
                "Test", "Honda", "Civic", 2021, "Engine Package");

            // Toate subsistemele au rulat — verificam outputul complet
            Assert.True(result.Success);
            Assert.NotEmpty(result.OrderId);
            Assert.NotEmpty(result.AssignedTechnician);
            Assert.NotEmpty(result.ScheduledDate);
            Assert.NotEmpty(result.HistoryNote);
            Assert.NotEmpty(result.NotificationMessage);
        }

        // ============================================================
        // LAB 6 - Behavioral Patterns
        // ============================================================

        // -- Strategy -------------------------------------------------

        [Fact]
        public void Strategy_Standard_AddsLoanCarCost()
        {
            var order = CreateLab6Order(
                new[] { "Oil Change", "Brake Repair" },
                includesLoanCar: true);

            var calculator = new ServiceCostCalculator(new StandardCostStrategy());
            var estimate = calculator.Calculate(order);

            Assert.Equal("Standard", estimate.StrategyName);
            Assert.Equal(600m, estimate.BaseCost);
            Assert.Equal(100m, estimate.Adjustments);
            Assert.Equal(700m, estimate.TotalCost);
            Assert.Equal(700m, order.EstimatedCost);
            Assert.Contains(estimate.AppliedRules, rule => rule.Contains("masina de schimb"));
        }

        [Fact]
        public void Strategy_ExpressAndLoyalty_ApplyDifferentAdjustments()
        {
            var expressOrder = CreateLab6Order(new[] { "Oil Change", "Brake Repair" });
            var loyaltyOrder = CreateLab6Order(new[] { "Oil Change", "Brake Repair" });

            var express = new ServiceCostCalculator(new ExpressCostStrategy()).Calculate(expressOrder);
            var loyalty = new ServiceCostCalculator(new LoyaltyCostStrategy()).Calculate(loyaltyOrder);

            Assert.Equal(150m, express.Adjustments);
            Assert.Equal(750m, express.TotalCost);
            Assert.Equal(-90m, loyalty.Adjustments);
            Assert.Equal(510m, loyalty.TotalCost);
            Assert.True(express.TotalCost > loyalty.TotalCost);
        }

        // -- Iterator -------------------------------------------------

        [Fact]
        public void Iterator_DepthFirst_ReturnsComponentsInTreeOrder()
        {
            var root = CreateIteratorPackage();
            var iterator = new IterableServicePackage(root)
                .CreateIterator(ServiceComponentTraversalMode.DepthFirst);

            var items = Drain(iterator);

            Assert.Equal(
                new[] { "Root Package", "Basic Package", "Oil Change", "Tire Check", "Brake Repair" },
                items.Select(item => item.Component.Name).ToArray());
            Assert.Equal(new[] { 0, 1, 2, 2, 1 }, items.Select(item => item.Depth).ToArray());
        }

        [Fact]
        public void Iterator_LeafOnly_ReturnsOnlySingleServices()
        {
            var root = CreateIteratorPackage();
            var iterator = new IterableServicePackage(root)
                .CreateIterator(ServiceComponentTraversalMode.LeafOnly);

            var items = Drain(iterator);

            Assert.All(items, item => Assert.False(item.Component.IsComposite));
            Assert.Equal(
                new[] { "Oil Change", "Tire Check", "Brake Repair" },
                items.Select(item => item.Component.Name).ToArray());
        }

        [Fact]
        public void Iterator_CompositeOnly_ReturnsOnlyPackages()
        {
            var root = CreateIteratorPackage();
            var iterator = new IterableServicePackage(root)
                .CreateIterator(ServiceComponentTraversalMode.CompositeOnly);

            var items = Drain(iterator);

            Assert.All(items, item => Assert.True(item.Component.IsComposite));
            Assert.Equal(
                new[] { "Root Package", "Basic Package" },
                items.Select(item => item.Component.Name).ToArray());
        }

        // -- Observer -------------------------------------------------

        [Fact]
        public void Observer_StatusChange_NotifiesDashboardCustomerAndTechnician()
        {
            var order = CreateLab6Order(new[] { "Oil Change" });
            var subject = new ServiceOrderStatusSubject(order);
            var dashboard = new ReceptionDashboardObserver();
            var customer = new CustomerNotificationObserver();
            var technician = new TechnicianNotificationObserver();

            subject.Attach(dashboard);
            subject.Attach(customer);
            subject.Attach(technician);

            subject.ChangeStatus(ServiceOrderStatus.Scheduled, "Programare confirmata.");

            Assert.Single(dashboard.StatusHistory);
            Assert.Contains("Created -> Scheduled", dashboard.StatusHistory[0]);
            Assert.Single(customer.Notifications);
            Assert.Contains("Toyota Camry", customer.Notifications[0]);
            Assert.Single(technician.Tasks);
            Assert.Contains(order.TechnicianName, technician.Tasks[0]);
        }

        [Fact]
        public void Observer_CompletedStatus_DoesNotCreateTechnicianTask()
        {
            var order = CreateLab6Order(new[] { "Oil Change" });
            var subject = new ServiceOrderStatusSubject(order);
            var technician = new TechnicianNotificationObserver();

            subject.Attach(technician);
            subject.ChangeStatus(ServiceOrderStatus.Completed, "Comanda finalizata.");

            Assert.Empty(technician.Tasks);
        }

        // -- Command --------------------------------------------------

        [Fact]
        public void Command_Invoker_ExecutesUndoAndRedo()
        {
            var order = CreateLab6Order(new[] { "Oil Change" });
            var receiver = new ServiceOrderCommandReceiver();
            var invoker = new ServiceCommandInvoker();

            invoker.ExecuteCommand(new AddServiceCommand(receiver, order, "Diagnostics"));
            invoker.ExecuteCommand(new ChangePriorityCommand(receiver, order, "High"));

            Assert.Contains("Diagnostics", order.Services);
            Assert.Equal("High", order.Priority);
            Assert.Equal(2, invoker.UndoCount);

            Assert.True(invoker.Undo());
            Assert.Equal("Standard", order.Priority);
            Assert.Contains("Diagnostics", order.Services);

            Assert.True(invoker.Undo());
            Assert.DoesNotContain("Diagnostics", order.Services);

            Assert.True(invoker.Redo());
            Assert.Contains("Diagnostics", order.Services);
            Assert.Equal(1, invoker.RedoCount);
        }

        [Fact]
        public void Command_ScheduledCommands_ExecuteInOrder()
        {
            var order = CreateLab6Order(new[] { "Oil Change" });
            var receiver = new ServiceOrderCommandReceiver();
            var invoker = new ServiceCommandInvoker();

            invoker.ScheduleCommand(new AddServiceCommand(receiver, order, "Brake Repair"));
            invoker.ScheduleCommand(new AssignTechnicianCommand(receiver, order, "Ioana Stan"));

            Assert.Equal(2, invoker.PendingCommandsCount);

            invoker.ExecuteScheduledCommands();

            Assert.Equal(0, invoker.PendingCommandsCount);
            Assert.Contains("Brake Repair", order.Services);
            Assert.Equal("Ioana Stan", order.TechnicianName);
            Assert.Equal(2, invoker.UndoCount);
        }

        // -- Memento --------------------------------------------------

        [Fact]
        public void Memento_UndoRestoresSavedState()
        {
            var order = CreateLab6Order(new[] { "Oil Change" });
            var originator = new ServiceOrderOriginator(order);
            var history = new ServiceOrderHistory(originator);

            history.Backup("Initial state");

            order.Priority = "High";
            order.Services.Add("Engine Repair");
            order.IncludesLoanCar = true;

            Assert.True(history.Undo());

            Assert.Equal("Standard", order.Priority);
            Assert.Equal(new[] { "Oil Change" }, order.Services);
            Assert.False(order.IncludesLoanCar);
            Assert.Equal(0, history.UndoCount);
            Assert.Equal(1, history.RedoCount);
        }

        [Fact]
        public void Memento_RedoRestoresStateBeforeUndo()
        {
            var order = CreateLab6Order(new[] { "Oil Change" });
            var originator = new ServiceOrderOriginator(order);
            var history = new ServiceOrderHistory(originator);

            history.Backup("Initial state");
            order.Priority = "High";
            order.Services.Add("Engine Repair");

            history.Undo();

            Assert.True(history.Redo());

            Assert.Equal("High", order.Priority);
            Assert.Contains("Engine Repair", order.Services);
            Assert.Equal(1, history.UndoCount);
            Assert.Equal(0, history.RedoCount);
        }

        private static ServiceOrder CreateLab6Order(
            IEnumerable<string> services,
            string priority = "Standard",
            bool includesLoanCar = false)
        {
            var builder = new ServiceOrderBuilder();

            builder
                .ForVehicle(new Car("Toyota", "Camry", 2020))
                .WithPriority(priority)
                .WithTechnician("Maria Ionescu")
                .ScheduledOn(DateTime.Today.AddDays(2))
                .WithLoanCar(includesLoanCar)
                .WithNotes("Test order");

            foreach (var service in services)
            {
                builder.AddService(service);
            }

            return builder.GetProduct();
        }

        private static IServiceComponent CreateIteratorPackage()
        {
            var basicPackage = new ServicePackage("Basic Package", "Basic")
                .Add(new SingleService("Oil Change", "Oil", 150m))
                .Add(new SingleService("Tire Check", "Tires", 80m));

            return new ServicePackage("Root Package", "Root")
                .Add(basicPackage)
                .Add(new SingleService("Brake Repair", "Brakes", 250m));
        }

        private static List<ServiceComponentIteratorItem> Drain(IServiceComponentIterator iterator)
        {
            var items = new List<ServiceComponentIteratorItem>();

            while (iterator.MoveNext())
            {
                items.Add(iterator.Current);
            }

            return items;
        }
    }
}
