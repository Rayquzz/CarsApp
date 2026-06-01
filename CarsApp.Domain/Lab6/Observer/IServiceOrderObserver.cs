using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Lab6.Observer;

public interface IServiceOrderObserver
{
    void Update(IServiceOrderSubject subject);
}