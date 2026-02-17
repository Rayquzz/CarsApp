using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Services;
using CarsApp.Infrastructure.Services;

namespace CarsApp.Infrastructure.Factories.FactoryMethod
{
    public class OilChangeFactory : ServiceFactory
    {
        public override IServiceOperation CreateService()
        {
            return new OilChange();
        }
    }
}
