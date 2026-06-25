using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicSyncSystem : IGameSystem, IFixedUpdatableSystem
{
    private World _world;
    public void Initialize(World world)
    {
        _world = world;
        _world.AddSystem(this);
        Debug.Log("PhysicSyncSystem initialized");
    }

    public void Shutdown() => Debug.Log("PhysicSyncSystem shutdown");
    public void FixedUpdate(float fixedDeltaTime)
    {
        // Only object that is movable (aka have MovementComponent) need to sync with Unity RigidBody
        IEnumerable<EntityID> entities = _world.GetEntitiesWithComponent<MovementComponent>();
        Debug.Log($"PhysicSyncSystem: {entities.Count()} entities");
        foreach (EntityID entityId in entities)
        {
            if (_world.GetEntityObject(entityId, out GameObject entityObject) && entityObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb) && _world.TryGetComponentFromEntity<MovementComponent>(entityId, out MovementComponent movementComponent))
            {
                rb.linearVelocity = new Vector2(movementComponent.CurrSpeed.x, movementComponent.CurrSpeed.y);
                Debug.Log($"PhysicSyncSystem: EntityID {entityId} set Rigidbody speed to {rb.linearVelocity} currSpeed is {movementComponent.CurrSpeed}");
            }
        }
    }
}
