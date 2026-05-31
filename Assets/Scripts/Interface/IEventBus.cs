using System;

public interface IEventBus
{
    public void Subscribe<T>(Action<T> listener);
    public void Unsubscribe<T>(Action<T> listener);
    public void Publish<T>(T eventData);
}
