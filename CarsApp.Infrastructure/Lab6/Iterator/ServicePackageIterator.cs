using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab4.Composite;
using CarsApp.Domain.Lab6.Iterator;

namespace CarsApp.Infrastructure.Lab6.Iterator;

public class ServicePackageIterator : IServiceComponentIterator
{
    private readonly IServiceComponent _root;
    private readonly ServiceComponentTraversalMode _mode;
    private readonly List<ServiceComponentIteratorItem> _items = new();
    private int _position = -1;

    public int Key => _position;

    public ServiceComponentIteratorItem Current
    {
        get
        {
            if (_position < 0 || _position >= _items.Count)
            {
                throw new InvalidOperationException("Iteratorul nu este pozitionat pe un element valid.");
            }

            return _items[_position];
        }
    }

    public ServicePackageIterator(
        IServiceComponent root,
        ServiceComponentTraversalMode mode = ServiceComponentTraversalMode.DepthFirst)
    {
        _root = root ?? throw new ArgumentNullException(nameof(root));
        _mode = mode;

        BuildTraversalList();
    }

    public bool MoveNext()
    {
        var nextPosition = _position + 1;

        if (nextPosition >= _items.Count)
        {
            return false;
        }

        _position = nextPosition;
        return true;
    }

    public void Reset()
    {
        _position = -1;
    }

    private void BuildTraversalList()
    {
        _items.Clear();

        var index = 0;
        Traverse(_root, depth: 0, path: _root.Name, index: ref index);
    }

    private void Traverse(
        IServiceComponent component,
        int depth,
        string path,
        ref int index)
    {
        if (ShouldInclude(component))
        {
            _items.Add(new ServiceComponentIteratorItem(
                component,
                index,
                depth,
                path));

            index++;
        }

        if (!component.IsComposite)
        {
            return;
        }

        foreach (var child in component.Children)
        {
            var childPath = $"{path} > {child.Name}";
            Traverse(child, depth + 1, childPath, ref index);
        }
    }

    private bool ShouldInclude(IServiceComponent component)
    {
        return _mode switch
        {
            ServiceComponentTraversalMode.DepthFirst => true,
            ServiceComponentTraversalMode.LeafOnly => !component.IsComposite,
            ServiceComponentTraversalMode.CompositeOnly => component.IsComposite,
            _ => true
        };
    }
}