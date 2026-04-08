using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Lab4.Composite
{
    public class ServicePackage : IServiceComponent
    {
        private readonly List<IServiceComponent> _children = new();

        public string Name { get; }
        public string Description { get; }
        public bool IsComposite => true;
        public IReadOnlyList<IServiceComponent> Children => _children.AsReadOnly();

        // Pretul se calculeaza recursiv din copii
        public decimal Price => _children.Sum(c => c.Price);

        public ServicePackage(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public ServicePackage Add(IServiceComponent component)
        {
            _children.Add(component);
            return this;
        }

        public ServicePackage Remove(IServiceComponent component)
        {
            _children.Remove(component);
            return this;
        }
    }
}