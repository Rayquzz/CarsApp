using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Entities;
using CarsApp.Domain.Lab6.Strategy;

namespace CarsApp.Infrastructure.Lab6.Strategy;

public class LoyaltyCostStrategy : IServiceCostStrategy
{
    public string Name => "Loyalty";

    public CostEstimate Calculate(ServiceOrder order)
    {
        var rules = new List<string>();

        var baseCost = order.Services.Sum(ServicePriceCatalog.GetPrice);
        rules.Add("Cost calculat pe baza serviciilor selectate.");

        var discount = -(baseCost * 0.15m);
        rules.Add("A fost aplicat discountul de fidelitate de 15%.");

        var adjustments = discount;

        if (order.IncludesLoanCar)
        {
            adjustments += 100m;
            rules.Add("A fost adaugat costul pentru masina de schimb.");
        }

        var total = baseCost + adjustments;

        return new CostEstimate(Name, baseCost, adjustments, total, rules);
    }
}