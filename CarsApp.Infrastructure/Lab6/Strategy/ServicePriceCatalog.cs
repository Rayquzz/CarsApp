using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Infrastructure.Lab6.Strategy;

internal static class ServicePriceCatalog
{
    public static decimal GetPrice(string serviceName)
    {
        return serviceName switch
        {
            "Oil Change" => 150m,
            "Brake Repair" => 450m,
            "Engine Repair" => 900m,
            "Electric Engine Repair" => 1000m,
            "Electric Brake Repair" => 500m,
            "Combustion Engine Repair" => 950m,
            "Combustion Brake Repair" => 480m,
            _ => 200m
        };
    }
}