using CarsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Services
{
    public interface IServiceOperation
    {
        string Name { get; }
        void Perform(Vehicle vehicle);
    }
}
