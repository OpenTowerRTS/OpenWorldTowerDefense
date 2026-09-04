using System.Collections.Generic;
using UnityEngine;

public class HealthUISystem : IGameSystem, IUpdatableSystem
{
    private World _world;

    public void Initialize(World world)
    {
        _world = world;
        _world.AddSystem(this);
        Debug.Log("HealthUISystem initialized");
    }

    public void Shutdown() => Debug.Log("HealthUISystem shutdown");

    public void Update(float deltaTime)
    {
        // 1. Find every entity in the game that has Health
        IEnumerable<EntityID> entities = _world.GetEntitiesWithComponent<HealthComponent>();

        foreach (EntityID entityId in entities)
        {
            // 2. Read the pure data
            HealthComponent health = _world.GetComponentFromEntity<HealthComponent>(entityId);

            // 3. Find the physical GameObject in the Unity scene
            if (_world.GetEntityObject(entityId, out GameObject entityObject))
            {
                // 4. Update the visual slider if it has one
                if (entityObject.TryGetComponent(out HealthUIDisplay display))
                {
                    display.UpdateHealth(health.CurrentHealth, health.MaxHealth);
                }
            }
        }
    }
}
