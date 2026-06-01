using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CarsApp.Domain.Lab6.Memento;

public interface IServiceOrderMemento
{
    string GetName();
    DateTime GetDate();
    string GetDescription();
}