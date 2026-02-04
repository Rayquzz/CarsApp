using CarsApp.Domain.Entities;
using CarsApp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.application.Interfaces
{
    public interface IServiceManager
    {
        void ExecuteService(Vehicle vehicle, IServiceOperation service);
    }
}
