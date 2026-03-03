using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Entities
{
    public class ServiceOrder
    {
        public string OrderId { get; set; }
        public Vehicle Vehicle { get; set; }
        public List<string> Services { get; set; } = new();
        public string Priority { get; set; }
        public bool IncludesLoanCar { get; set; }
        public string TechnicianName { get; set; }
        public DateTime ScheduledDate { get; set; }
        public decimal EstimatedCost { get; set; }
        public string Notes { get; set; }
    }
}
