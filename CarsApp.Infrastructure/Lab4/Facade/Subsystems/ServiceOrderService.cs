using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Infrastructure.Lab4.Facade.Subsystems
{
    // Subsistem 3 — creeaza comanda de service
    public class ServiceOrderService
    {
        public (string OrderId, string ScheduledDate) CreateOrder(
            string customerName, string vehicleInfo, string packageName)
        {
            var orderId = "ORD-" + Guid.NewGuid().ToString("N")[..6].ToUpper();
            var scheduledDate = DateTime.Today.AddDays(2).ToString("dd.MM.yyyy");
            return (orderId, scheduledDate);
        }
    }
}