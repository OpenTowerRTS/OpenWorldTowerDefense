using System.Collections.Generic;
using UnityEngine;

public class PathFindingSystem : IGameSystem, IFixedUpdatableSystem
{
    private World _world;

    private readonly IPathFinder _pathFinder;
    public PathFindingSystem(IPathFinder pathFinder) => _pathFinder = pathFinder;
    public void Initialize(World world)
    {
        _world = world;
        _world.AddSystem(this);
        Debug.Log("PathFindingSystem initialized");
    }

    public void Shutdown() => Debug.Log("PathFindingSystem shutdown");

    public void FixedUpdate(float deltaTime)
    {
        // For any system with a PathFindingRequest. Having a PathFindingRequest indicate that it has
        // A MovementTarget. The reason why these 2 are separated is because The Request is transient. It will be removed once the path is found.
        // But the MovementTarget will stay until the entity reach the target position.
        // However, there are cases where the MovementTarget is simply not reachable so we need a way to stop the movement after we did our best.
        IEnumerable<EntityID> entities = _world.GetEntitiesWithComponent<PathFindingRequest>();

        // PathFindingSystem now call NavigationSystemManager to request a path.
        // TODO: The specific path also need differ based on unit
        // But keep it simple for now. We will only create a Path with one point, which is the target position.
        // Stuff to add: different path finder for different unit
        foreach (EntityID entityId in entities)
        {
            PathFindingRequest pathFindingRequest = _world.GetComponentFromEntity<PathFindingRequest>(entityId);
            PathRequestStatus requestStatus = _pathFinder.TryGetPathResult(pathFindingRequest.requestId, out Vector3[] pathResult);
            if (requestStatus == PathRequestStatus.Completed)
            {
                // Ignore null path. This means that the destination is unreachable
                // Force remove the movement target. This movement target is now meaningless since you can't reach it.
                PathComponent newPathComponent = new(pathResult, 0);
                _world.AddComponentToEntity<PathComponent>(entityId, newPathComponent);
                _world.RemoveComponentFromEntity<PathFindingRequest>(entityId);
            }
            else if (requestStatus == PathRequestStatus.Unreachable)
            {
                Debug.Log("Path result Remove PathRequest");
                _world.RemoveComponentFromEntity<PathFindingRequest>(entityId);
                _world.RemoveComponentFromEntity<MovementTargetComponent>(entityId);
            }
        }
    }
}
