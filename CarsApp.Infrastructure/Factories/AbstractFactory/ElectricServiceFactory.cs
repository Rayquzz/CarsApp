using CarsApp.Domain.Services;
using CarsApp.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.application.Interfaces;

namespace CarsApp.Infrastructure.Factories.AbstractFactory
{
    public class ElectricServiceFactory : IServicePackageFactory
    {
        public IServiceOperation CreateEngineRepair()
        {
            return new ElectricEngineRepair();
        }
        public IServiceOperation CreateBrakeRepair()
        {
            return new ElectricBrakeRepair();
        }
    }
}
