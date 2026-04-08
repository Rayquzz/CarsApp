using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Lab4.Composite
{
    public interface IServiceComponent
    {
        string Name { get; }
        string Description { get; }
        decimal Price { get; }
        bool IsComposite { get; }
        IReadOnlyList<IServiceComponent> Children { get; }
    }
}