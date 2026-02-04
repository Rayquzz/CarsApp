using CarsApp.application.Interfaces;
using CarsApp.Domain.Entities;
using CarsApp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.application.Services
{
    public class ServiceManager : IServiceManager
    {
        public void ExecuteService(Vehicle vehicle, IServiceOperation service)
        {
            service.Perform(vehicle);
        }
    }
}
