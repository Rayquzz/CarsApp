using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Entities;

namespace CarsApp.Domain.Lab6.Strategy;

public class ServiceCostCalculator
{
    private IServiceCostStrategy _strategy;

    public ServiceCostCalculator(IServiceCostStrategy strategy)
    {
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public string CurrentStrategyName => _strategy.Name;

    public void SetStrategy(IServiceCostStrategy strategy)
    {
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public CostEstimate Calculate(ServiceOrder order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        var estimate = _strategy.Calculate(order);

        order.EstimatedCost = estimate.TotalCost;

        return estimate;
    }
}