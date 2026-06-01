namespace CarsApp.Infrastructure.Lab7.Mediator;

public class CustomerNotificationCenter
{
    private readonly List<string> _notifications = new();

    public IReadOnlyList<string> Notifications => _notifications.AsReadOnly();

    public void Send(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Message is required.", nameof(message));
        }

        _notifications.Add(message);
    }
}
