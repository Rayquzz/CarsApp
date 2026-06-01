using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Entities;
using CarsApp.Domain.Lab6.Strategy;

namespace CarsApp.Infrastructure.Lab6.Strategy;

public class StandardCostStrategy : IServiceCostStrategy
{
    public string Name => "Standard";

    public CostEstimate Calculate(ServiceOrder order)
    {
        var rules = new List<string>();

        var baseCost = order.Services.Sum(ServicePriceCatalog.GetPrice);
        rules.Add("Cost calculat pe baza serviciilor selectate.");

        var adjustments = 0m;

        if (order.IncludesLoanCar)
        {
            adjustments += 100m;
            rules.Add("A fost adaugat costul pentru masina de schimb.");
        }

        var total = baseCost + adjustments;

        return new CostEstimate(Name, baseCost, adjustments, total, rules);
    }
}