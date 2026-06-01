using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Entities;

namespace CarsApp.Domain.Lab6.Strategy;

public interface IServiceCostStrategy
{
    string Name { get; }

    CostEstimate Calculate(ServiceOrder order);
}
