using CarsApp.Domain.Entities;
using CarsApp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Builder
{
    public class ServiceOrderBuilder : IServiceOrderBuilder
    {
        private ServiceOrder _order;

        public ServiceOrderBuilder() => Reset();

        public void Reset()
        {
            _order = new ServiceOrder
            {
                OrderId = Guid.NewGuid().ToString("N")[..8].ToUpper(),
                Priority = "Standard",
                ScheduledDate = DateTime.Today.AddDays(1)
            };
        }

        public IServiceOrderBuilder ForVehicle(Vehicle v)
        { _order.Vehicle = v; return this; }

        public IServiceOrderBuilder AddService(string s)
        { _order.Services.Add(s); return this; }

        public IServiceOrderBuilder WithPriority(string priority)
        {
            _order.Priority = priority; return this;
        }

        public IServiceOrderBuilder WithTechnician(string name)
        {
            _order.TechnicianName = name; return this;
        }

        public IServiceOrderBuilder ScheduledOn(DateTime date)
        {
            _order.ScheduledDate = date; return this;
        }

        public IServiceOrderBuilder WithLoanCar(bool include)
        {
            _order.IncludesLoanCar = include; return this;
        }

        public IServiceOrderBuilder WithEstimatedCost(decimal cost)
        {
            _order.EstimatedCost = cost; return this;
        }

        public IServiceOrderBuilder WithNotes(string notes)
        {
            _order.Notes = notes; return this;
        }

        public ServiceOrder GetProduct()
        {
            var result = _order;
            Reset(); // pregătit pentru o nouă construire
            return result;
        }

       
    }
}
