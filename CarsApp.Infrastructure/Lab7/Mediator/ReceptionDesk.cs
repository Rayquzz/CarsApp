using CarsApp.Domain.Lab7.Mediator;

namespace CarsApp.Infrastructure.Lab7.Mediator;

public class ReceptionDesk : WorkshopComponentBase
{
    private readonly List<string> _serviceRequests = new();
    private readonly List<string> _checkoutPreparations = new();

    public IReadOnlyList<string> ServiceRequests => _serviceRequests.AsReadOnly();

    public IReadOnlyList<string> CheckoutPreparations => _checkoutPreparations.AsReadOnly();

    public string LastServiceRequest { get; private set; } = string.Empty;

    public void CreateServiceRequest(string customerName, string vehicleInfo, string requestedService)
    {
        if (string.IsNullOrWhiteSpace(customerName))
        {
            throw new ArgumentException("Customer name is required.", nameof(customerName));
        }

        if (string.IsNullOrWhiteSpace(vehicleInfo))
        {
            throw new ArgumentException("Vehicle info is required.", nameof(vehicleInfo));
        }

        if (string.IsNullOrWhiteSpace(requestedService))
        {
            throw new ArgumentException("Requested service is required.", nameof(requestedService));
        }

        LastServiceRequest = $"{customerName} - {vehicleInfo} - {requestedService}";
        _serviceRequests.Add(LastServiceRequest);

        Mediator.Notify(this, WorkshopEvent.ServiceRequestCreated);
    }

    public void PrepareCheckout(string repairSummary)
    {
        var entry = $"Checkout prepared for completed repair: {repairSummary}";
        _checkoutPreparations.Add(entry);
    }
}
