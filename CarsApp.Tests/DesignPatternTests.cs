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
using CarsApp.Domain.Lab4.Composite;
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
    }
}