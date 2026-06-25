using System;
using System.Collections.Generic;

public class CommandBuffer
{
    private Dictionary<Type, List<ICommand>> _read;
    private Dictionary<Type, List<ICommand>> _write;
    public long Version { get; private set; }
    public CommandBuffer()
    {
        _read = new Dictionary<Type, List<ICommand>>();
        _write = new Dictionary<Type, List<ICommand>>();
        version = 0;
    }

    public long version; // Incremented each time commands are processed, can be used for synchronization and debugging

    public void AddCommand<T>(T command) where T : ICommand
    {
        if (!_write.TryGetValue(typeof(T), out List<ICommand> commands))
        {
            commands = new List<ICommand>();
            _write[typeof(T)] = commands;
        }
        commands.Add(command);
    }

    public bool GetCommands<T>(out List<T> commands) where T : ICommand
    {
        if (_read.TryGetValue(typeof(T), out List<ICommand> commandList))
        {
            commands = commandList.ConvertAll(cmd => (T)cmd);
            return true;
        }
        else
        {
            commands = new List<T>();
            return false;
        }
    }

    public void SwapBuffers()
    {
        (_read, _write) = (_write, _read);
        _write.Clear();
        Version++;
    }
}
