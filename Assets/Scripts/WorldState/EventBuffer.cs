using System;
using System.Collections.Generic;

public class EventBuffer
{
    private Dictionary<Type, List<IEvent>> _write = new();
    private Dictionary<Type, List<IEvent>> _read = new();
    public long Version { get; private set; }
    public EventBuffer()
    {
        _write = new Dictionary<Type, List<IEvent>>();
        _read = new Dictionary<Type, List<IEvent>>();
        Version = 0;
    }

    public void AddEvent<T>(T newEvent) where T : IEvent
    {
        if (!_write.TryGetValue(typeof(T), out List<IEvent> events))

        {
            events = new List<IEvent>();
            _write[typeof(T)] = events;
        }
        events.Add(newEvent);
    }

    public bool GetEvents<T>(out List<T> events) where T : IEvent
    {
        if (_read.TryGetValue(typeof(T), out List<IEvent> eventList))
        {
            events = eventList.ConvertAll(evt => (T)evt);
            return true;
        }
        else
        {
            events = new List<T>();
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
