using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab4.Composite;

namespace CarsApp.Domain.Lab6.Iterator;

public class ServiceComponentIteratorItem
{
    public IServiceComponent Component { get; }
    public int Index { get; }
    public int Depth { get; }
    public string Path { get; }

    public ServiceComponentIteratorItem(
        IServiceComponent component,
        int index,
        int depth,
        string path)
    {
        Component = component ?? throw new ArgumentNullException(nameof(component));
        Index = index;
        Depth = depth;
        Path = path;
    }
}