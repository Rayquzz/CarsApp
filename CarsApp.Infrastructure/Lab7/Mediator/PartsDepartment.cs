using CarsApp.Domain.Lab7.Mediator;

namespace CarsApp.Infrastructure.Lab7.Mediator;

public class PartsDepartment : WorkshopComponentBase
{
    private readonly Dictionary<string, int> _stock = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<string> _reservedParts = new();
    private readonly List<string> _missingParts = new();

    public IReadOnlyDictionary<string, int> Stock => _stock;

    public IReadOnlyList<string> ReservedParts => _reservedParts.AsReadOnly();

    public IReadOnlyList<string> MissingParts => _missingParts.AsReadOnly();

    public string LastProcessedPart { get; private set; } = string.Empty;

    public void AddStock(string partName, int quantity)
    {
        if (string.IsNullOrWhiteSpace(partName))
        {
            throw new ArgumentException("Part name is required.", nameof(partName));
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        _stock[partName] = _stock.TryGetValue(partName, out var currentQuantity)
            ? currentQuantity + quantity
            : quantity;
    }

    public void CheckAvailability(string partName)
    {
        if (string.IsNullOrWhiteSpace(partName))
        {
            throw new ArgumentException("Part name is required.", nameof(partName));
        }

        LastProcessedPart = partName;

        if (_stock.TryGetValue(partName, out var quantity) && quantity > 0)
        {
            _stock[partName] = quantity - 1;
            _reservedParts.Add(partName);
            Mediator.Notify(this, WorkshopEvent.PartsAvailable);
            return;
        }

        _missingParts.Add(partName);
        Mediator.Notify(this, WorkshopEvent.PartsUnavailable);
    }
}
