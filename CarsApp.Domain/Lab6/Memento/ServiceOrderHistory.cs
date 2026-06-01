using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Lab6.Memento;

public class ServiceOrderHistory
{
    private readonly ServiceOrderOriginator _originator;
    private readonly Stack<IServiceOrderMemento> _undoStack = new();
    private readonly Stack<IServiceOrderMemento> _redoStack = new();

    public ServiceOrderHistory(ServiceOrderOriginator originator)
    {
        _originator = originator ?? throw new ArgumentNullException(nameof(originator));
    }

    public int UndoCount => _undoStack.Count;
    public int RedoCount => _redoStack.Count;

    public IReadOnlyList<string> History =>
        _undoStack
            .Reverse()
            .Select(memento => $"{memento.GetName()} - {memento.GetDescription()}")
            .ToList()
            .AsReadOnly();

    public void Backup(string description)
    {
        var memento = _originator.Save(description);

        _undoStack.Push(memento);
        _redoStack.Clear();
    }

    public bool Undo()
    {
        if (_undoStack.Count == 0)
        {
            return false;
        }

        var currentState = _originator.Save("Redo snapshot before undo");
        _redoStack.Push(currentState);

        var previousState = _undoStack.Pop();
        _originator.Restore(previousState);

        return true;
    }

    public bool Redo()
    {
        if (_redoStack.Count == 0)
        {
            return false;
        }

        var currentState = _originator.Save("Undo snapshot before redo");
        _undoStack.Push(currentState);

        var nextState = _redoStack.Pop();
        _originator.Restore(nextState);

        return true;
    }

    public void Clear()
    {
        _undoStack.Clear();
        _redoStack.Clear();
    }
}