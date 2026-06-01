using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Lab6.Iterator;

public interface IServiceComponentIterator
{
    int Key { get; }
    ServiceComponentIteratorItem Current { get; }

    bool MoveNext();
    void Reset();
}