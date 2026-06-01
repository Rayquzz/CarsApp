using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab5.Decorator;

namespace CarsApp.Infrastructure.Lab5.Decorator;

// Decoratorul abstract — învelește orice IDecoratedService
// Subclasele suprascriu doar ce adaugă ele
public abstract class ServiceDecorator : IDecoratedService
{
    protected readonly IDecoratedService _inner;

    protected ServiceDecorator(IDecoratedService inner)
    {
        _inner = inner;
    }

    // Implicit deleghează tot spre inner
    public virtual string Name => _inner.Name;
    public virtual string Description => _inner.Description;
    public virtual decimal Cost => _inner.Cost;
    public virtual IReadOnlyList<string> Log => _inner.Log;

    public virtual string Execute(string vehicleInfo)
        => _inner.Execute(vehicleInfo);
}