using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab4.Composite;
using CarsApp.Domain.Lab6.Iterator;

namespace CarsApp.Infrastructure.Lab6.Iterator;

public class IterableServicePackage : IServiceComponentIterable
{
    private readonly IServiceComponent _root;

    public IterableServicePackage(IServiceComponent root)
    {
        _root = root ?? throw new ArgumentNullException(nameof(root));
    }

    public IServiceComponentIterator CreateIterator(
        ServiceComponentTraversalMode mode = ServiceComponentTraversalMode.DepthFirst)
    {
        return new ServicePackageIterator(_root, mode);
    }
}