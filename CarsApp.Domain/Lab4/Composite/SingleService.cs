using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Lab4.Composite
{
    public class SingleService : IServiceComponent
    {
        public string Name { get; }
        public string Description { get; }
        public decimal Price { get; }
        public bool IsComposite => false;
        public IReadOnlyList<IServiceComponent> Children => Array.Empty<IServiceComponent>();

        public SingleService(string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
        }
    }
}