using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Services;

namespace CarsApp.application.Interfaces
{
    public interface IServicePackageFactory
    {
        IServiceOperation CreateEngineRepair();
        IServiceOperation CreateBrakeRepair();
    }
}
