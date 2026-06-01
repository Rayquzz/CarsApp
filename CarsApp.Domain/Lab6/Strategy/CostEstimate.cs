using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Lab6.Strategy;

public class CostEstimate
{
    public string StrategyName { get; }
    public decimal BaseCost { get; }
    public decimal Adjustments { get; }
    public decimal TotalCost { get; }
    public IReadOnlyList<string> AppliedRules { get; }

    public CostEstimate(
        string strategyName,
        decimal baseCost,
        decimal adjustments,
        decimal totalCost,
        IReadOnlyList<string> appliedRules)
    {
        StrategyName = strategyName;
        BaseCost = baseCost;
        Adjustments = adjustments;
        TotalCost = totalCost;
        AppliedRules = appliedRules;
    }
}