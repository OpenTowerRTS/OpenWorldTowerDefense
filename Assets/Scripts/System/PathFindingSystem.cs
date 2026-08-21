using System.Collections.Generic;
using UnityEngine;

public class PathFindingSystem : IGameSystem, IFixedUpdatableSystem
{
    private World _world;
    public void Initialize(World world)
    {
        _world = world;
        _world.AddSystem(this);
        Debug.Log("PathFindingSystem initialized");
    }

    public void Shutdown() => Debug.Log("PathFindingSystem shutdown");

    public void FixedUpdate(float deltaTime)
    {
        IEnumerable<EntityID> entities = _world.GetEntitiesWithComponent<MovementTargetComponent>();

        // PathFindingSystem is supposed to careate a path. The specific pat also differ base on unit
        // But keep it simple for now. We will only create a Path with one point, which is the target position.
        // Stuff to add: PathFinding algorithm, different path finder for different unit
        foreach (EntityID entityId in entities)
        {
            MovementTargetComponent movementTarget = _world.GetComponentFromEntity<MovementTargetComponent>(entityId);
            if (!_world.TryGetComponentFromEntity(entityId, out PathComponent pathComponent) || pathComponent.Version != movementTarget.Version)
            {
                PathComponent newPathComponent = new(new Vector3[] { movementTarget.TargetPosition }, movementTarget.Version)
                {
                    CurrentPathIndex = 0
                };
                _world.AddComponentToEntity(entityId, newPathComponent);
            }
        }
    }
}
