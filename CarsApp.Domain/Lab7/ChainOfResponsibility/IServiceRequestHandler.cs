namespace CarsApp.Domain.Lab7.ChainOfResponsibility;

public interface IServiceRequestHandler
{
    IServiceRequestHandler SetNext(IServiceRequestHandler handler);

    ServiceRequestResult? Handle(ServiceRequest request);
}
