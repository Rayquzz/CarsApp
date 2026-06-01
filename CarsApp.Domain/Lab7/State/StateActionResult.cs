namespace CarsApp.Domain.Lab7.State;

public record StateActionResult(
    bool Success,
    string FromState,
    string ToState,
    string Message);
