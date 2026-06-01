using CarsApp.Domain.Lab7.ChainOfResponsibility;

namespace CarsApp.Infrastructure.Lab7.ChainOfResponsibility;

public static class ServiceRequestChainFactory
{
    public static IServiceRequestHandler CreateDefaultChain()
    {
        var reception = new ReceptionRequestHandler();
        var diagnostics = new DiagnosticsRequestHandler();
        var seniorTechnician = new SeniorTechnicianRequestHandler();
        var manager = new ManagerApprovalRequestHandler();

        reception
            .SetNext(diagnostics)
            .SetNext(seniorTechnician)
            .SetNext(manager);

        return reception;
    }
}
