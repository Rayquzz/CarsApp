using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Singleton
{
    public interface IServiceLogger
    {
        void Log(string vehicleInfo, string serviceName, string technicianName, decimal cost);
        IReadOnlyList<string> GetHistory();
        void PrintHistory();
    }
}
