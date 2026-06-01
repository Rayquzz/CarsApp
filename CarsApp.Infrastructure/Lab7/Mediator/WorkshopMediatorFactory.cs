namespace CarsApp.Infrastructure.Lab7.Mediator;

public static class WorkshopMediatorFactory
{
    public static WorkshopMediator CreateDefault(
        out ReceptionDesk receptionDesk,
        out TechnicianTeam technicianTeam,
        out PartsDepartment partsDepartment,
        out CustomerNotificationCenter notificationCenter)
    {
        receptionDesk = new ReceptionDesk();
        technicianTeam = new TechnicianTeam();
        partsDepartment = new PartsDepartment();
        notificationCenter = new CustomerNotificationCenter();

        return new WorkshopMediator(
            receptionDesk,
            technicianTeam,
            partsDepartment,
            notificationCenter);
    }
}
