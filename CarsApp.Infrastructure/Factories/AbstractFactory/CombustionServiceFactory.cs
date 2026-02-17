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
    public class CombustionServiceFactory : IServicePackageFactory
    {
        public IServiceOperation CreateEngineRepair()
        {
            return new CombustionEngineRepair();
        }

        public IServiceOperation CreateBrakeRepair()
        {
            return new CombustionBrakeRepair();
        }
    }
}
