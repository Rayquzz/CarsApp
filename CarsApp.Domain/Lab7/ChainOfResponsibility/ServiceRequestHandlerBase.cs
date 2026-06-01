namespace CarsApp.Domain.Lab7.ChainOfResponsibility;

public abstract class ServiceRequestHandlerBase : IServiceRequestHandler
{
    private IServiceRequestHandler? _nextHandler;

    public IServiceRequestHandler SetNext(IServiceRequestHandler handler)
    {
        _nextHandler = handler ?? throw new ArgumentNullException(nameof(handler));
        return handler;
    }

    public virtual ServiceRequestResult? Handle(ServiceRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return _nextHandler?.Handle(request);
    }
}
