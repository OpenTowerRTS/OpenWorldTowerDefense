using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementResolutionSystem : IGameSystem, IFixedUpdatableSystem
{
    private World _world;
    public void Initialize(World world)
    {
        _world = world;
        _world.AddSystem(this);
        Debug.Log("MovementResolutionSystem initialized");
    }

    public void Shutdown() => Debug.Log("MovementResolutionSystem shutdown");
    public void FixedUpdate(float fixedDeltaTime)
    {
        // We make sure in CommandPhase that only object with MovementComponent  can have a MovementTargetComponent
        IEnumerable<EntityID> entities = _world.GetEntitiesWithComponent<MovementTargetComponent>();
        Debug.Log($"MovementResolutionSystem: processing {entities.Count()} entities");
        foreach (EntityID entityId in entities)
        {
            MovementTargetComponent movementTarget = _world.GetComponentFromEntity<MovementTargetComponent>(entityId);
            if (_world.GetEntityObject(entityId, out GameObject entityObject))
            {
                // Check if we reach the target and remove all the Movement related component.
                // Or if we reach the end of the path
                float dist = Vector3.Distance(entityObject.transform.position, movementTarget.TargetPosition);
                ref MovementComponent movementComponent = ref _world.GetComponentFromEntity<MovementComponent>(entityId);
                Debug.Log($"MovementResolutionSystem: distance of entity {entityId}: {dist} with stop={movementComponent.ArrivalRadius}");
                if (dist < movementComponent.ArrivalRadius || movementComponent.ShouldStopNextFrame ||
                    (_world.TryGetComponentFromEntity(entityId, out PathComponent pathComponent) && pathComponent.CurrentPathIndex == pathComponent.PathPoints.Length))
                {
                    Debug.Log($"Reset Movement for Entity: {entityId}");
                    _world.RemoveComponentFromEntity<MovementTargetComponent>(entityId);
                    _world.RemoveComponentFromEntity<PathComponent>(entityId);
                    movementComponent.CurrSpeed = Vector3.zero;
                    movementComponent.ShouldStopNextFrame = false;
                }
                movementComponent.ShouldStopNextFrame = movementComponent.CurrSpeed.magnitude * fixedDeltaTime > dist;
            }
        }

        // At the moment, we have handling for updating the MovementTargetComponent. Meaning that an entity receive a new MovementTarget
        // Even though the old MovementTarget is not yet reached.
        // TODO: we don't have handling cancelling Movement all together.
        // 2 things can be done: either we can add a new CancellingMovemenentEvent + Command that will remove the MovementTarget right away
        // Or We can add a flag to MovementTargetComponent to indicate that the movement is cancelled. And then we can remove the MovementTargetComponent in this system.
    }
}
