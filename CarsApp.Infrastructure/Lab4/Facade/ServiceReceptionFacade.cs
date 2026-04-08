using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarsApp.Domain.Lab4.Facade;
using CarsApp.Infrastructure.Lab4.Facade.Subsystems;

namespace CarsApp.Infrastructure.Lab4.Facade
{
    // Façade — ascunde complexitatea celor 4 subsisteme
    // Clientul apeleaza doar CheckInVehicle() si primeste tot
    public class ServiceReceptionFacade : IReceptionFacade
    {
        private readonly VehicleHistoryService _history = new();
        private readonly TechnicianAllocator _allocator = new();
        private readonly ServiceOrderService _orderService = new();
        private readonly NotificationService _notification = new();

        public ReceptionResult CheckInVehicle(
            string customerName, string vehicleMake,
            string vehicleModel, int vehicleYear,
            string packageName)
        {
            var vehicleInfo = $"{vehicleMake} {vehicleModel} ({vehicleYear})";

            // Pasul 1 — verifica istoricul
            var historyNote = _history.GetHistory(vehicleMake, vehicleModel, vehicleYear);

            // Pasul 2 — aloca tehnician
            var technician = _allocator.AllocateTechnician(packageName);

            // Pasul 3 — creeaza comanda
            var (orderId, scheduledDate) = _orderService.CreateOrder(
                customerName, vehicleInfo, packageName);

            // Pasul 4 — trimite confirmare
            var notification = _notification.SendConfirmation(
                customerName, orderId, scheduledDate, technician);

            return new ReceptionResult(
                Success: true,
                OrderId: orderId,
                CustomerName: customerName,
                VehicleInfo: vehicleInfo,
                PackageName: packageName,
                AssignedTechnician: technician,
                ScheduledDate: scheduledDate,
                HistoryNote: historyNote,
                NotificationMessage: notification
            );
        }
    }
}