using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Lab6.Command;

public class ServiceCommandInvoker
{
    private readonly Queue<IServiceOrderCommand> _scheduledCommands = new();
    private readonly Stack<IServiceOrderCommand> _undoStack = new();
    private readonly Stack<IServiceOrderCommand> _redoStack = new();
    private readonly List<string> _history = new();

    public IReadOnlyList<string> History => _history.AsReadOnly();
    public int PendingCommandsCount => _scheduledCommands.Count;
    public int UndoCount => _undoStack.Count;
    public int RedoCount => _redoStack.Count;

    public void ScheduleCommand(IServiceOrderCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        _scheduledCommands.Enqueue(command);
        _history.Add($"Scheduled: {command.Name} - {command.Description}");
    }

    public void ExecuteCommand(IServiceOrderCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        command.Execute();

        _undoStack.Push(command);
        _redoStack.Clear();

        _history.Add($"Executed: {command.Name} - {command.Description}");
    }

    public void ExecuteScheduledCommands()
    {
        while (_scheduledCommands.Count > 0)
        {
            var command = _scheduledCommands.Dequeue();
            ExecuteCommand(command);
        }
    }

    public bool Undo()
    {
        if (_undoStack.Count == 0)
        {
            return false;
        }

        var command = _undoStack.Pop();
        command.Undo();

        _redoStack.Push(command);

        _history.Add($"Undo: {command.Name} - {command.Description}");
        return true;
    }

    public bool Redo()
    {
        if (_redoStack.Count == 0)
        {
            return false;
        }

        var command = _redoStack.Pop();
        command.Execute();

        _undoStack.Push(command);

        _history.Add($"Redo: {command.Name} - {command.Description}");
        return true;
    }

    public void ClearHistory()
    {
        _scheduledCommands.Clear();
        _undoStack.Clear();
        _redoStack.Clear();
        _history.Clear();
    }
}