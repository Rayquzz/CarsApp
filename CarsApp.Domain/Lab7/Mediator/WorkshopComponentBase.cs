namespace CarsApp.Domain.Lab7.Mediator;

public abstract class WorkshopComponentBase
{
    private IWorkshopMediator? _mediator;

    protected IWorkshopMediator Mediator =>
        _mediator ?? throw new InvalidOperationException("Mediator was not assigned.");

    protected WorkshopComponentBase(IWorkshopMediator? mediator = null)
    {
        _mediator = mediator;
    }

    public void SetMediator(IWorkshopMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
}
