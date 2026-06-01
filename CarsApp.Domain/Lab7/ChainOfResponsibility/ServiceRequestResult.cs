namespace CarsApp.Domain.Lab7.ChainOfResponsibility;

public record ServiceRequestResult(
    string HandlerName,
    string Department,
    string Message,
    bool RequiresFollowUp);
