using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Infrastructure.Lab4.Facade.Subsystems
{
    // Subsistem 4 — trimite confirmare clientului
    public class NotificationService
    {
        public string SendConfirmation(string customerName, string orderId,
                                       string scheduledDate, string technicianName)
        {
            return $"Confirmare trimisa catre {customerName}: " +
                   $"Programarea {orderId} este confirmata pentru {scheduledDate}. " +
                   $"Tehnicianul dumneavoastra: {technicianName}.";
        }
    }
}