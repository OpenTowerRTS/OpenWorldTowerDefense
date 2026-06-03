using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : IEventBus
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();

    public void Subscribe<T>(Action<T> listener)
    {

        if (!_subscribers.TryGetValue(typeof(T), out List<Delegate> listeners))
        {
            listeners = new List<Delegate>();
            _subscribers[typeof(T)] = listeners;
        }
        listeners.Add(listener);
        Debug.Log($"Subscribing to event of type {typeof(T)} to {listeners[0].Target.GetType().Name}.{((Action<T>)listeners[0]).Method.Name}");
    }

    public void Unsubscribe<T>(Action<T> listener)
    {
        if (_subscribers.TryGetValue(typeof(T), out List<Delegate> listeners))
        {
            listeners.Remove(listener);
            Debug.Log($"Unsubscribing from event of type {typeof(T)} from {listeners[0].Target.GetType().Name}.{((Action<T>)listeners[0]).Method.Name}");
            if (listeners.Count == 0)
            {
                _subscribers.Remove(typeof(T));
            }
        }
    }

    public void Publish<T>(T eventData)
    {
        if (_subscribers.TryGetValue(typeof(T), out List<Delegate> listeners))
        {
            foreach (Delegate listener in listeners)
            {
                ((Action<T>)listener).Invoke(eventData);
                Debug.Log($"Invoke event of type {listeners[0].Target.GetType().Name}.{((Action<T>)listener).Method.Name} with data: {eventData}");
            }
        }
    }
}
