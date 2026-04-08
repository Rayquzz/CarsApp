using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Lab4.Facade
{
    public interface IReceptionFacade
    {
        ReceptionResult CheckInVehicle(string customerName, string vehicleMake,
                                       string vehicleModel, int vehicleYear,
                                       string packageName);
    }

    public record ReceptionResult(
        bool Success,
        string OrderId,
        string CustomerName,
        string VehicleInfo,
        string PackageName,
        string AssignedTechnician,
        string ScheduledDate,
        string HistoryNote,
        string NotificationMessage
    );
}