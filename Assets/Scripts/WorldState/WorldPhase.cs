using System.Collections.Generic;

public class WorldPhase
{
    private readonly List<IGameSystem> _systems;

    public WorldPhase() => _systems = new List<IGameSystem>();

    public void AddSystem(IGameSystem system) => _systems.Add(system);

    public void RemoveSystem(IGameSystem system) => _systems.Remove(system);

    public void Update(float deltaTime)
    {
        foreach (IGameSystem system in _systems)
        {
            if (system is IUpdatableSystem updatable)
            {
                updatable.Update(deltaTime);
            }
        }
    }

    public void FixedUpdate(float fixedDeltaTime)
    {
        foreach (IGameSystem system in _systems)
        {
            if (system is IFixedUpdatableSystem fixedUpdatable)
            {
                fixedUpdatable.FixedUpdate(fixedDeltaTime);
            }
        }
    }
}
