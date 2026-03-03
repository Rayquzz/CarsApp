using CarsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Services
{
    public interface IServiceOrderBuilder
    {
        void Reset();
        IServiceOrderBuilder ForVehicle(Vehicle vehicle);
        IServiceOrderBuilder WithPriority(string priority);
        IServiceOrderBuilder AddService(string serviceName);
        IServiceOrderBuilder WithTechnician(string name);
        IServiceOrderBuilder ScheduledOn(DateTime date);
        IServiceOrderBuilder WithLoanCar(bool include);
        IServiceOrderBuilder WithEstimatedCost(decimal cost);
        IServiceOrderBuilder WithNotes(string notes);
        
    }
}
