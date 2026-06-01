using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Lab6.Command;

public interface IServiceOrderCommand
{
    string Name { get; }
    string Description { get; }
    DateTime? ExecutedAt { get; }

    void Execute();
    void Undo();
}