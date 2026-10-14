using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementCommandProcessingSystem : IGameSystem, IUpdatableSystem
{
    private World _world;
    private readonly IPathFinder _pathFinder;
    public MovementCommandProcessingSystem(IPathFinder pathFinder) => _pathFinder = pathFinder;
    public void Initialize(World world)
    {
        _world = world;
        _world.AddSystem(this);
        Debug.Log("MovementCommandProcessingSystem initialized");
    }

    public void Shutdown() => Debug.Log("MovementCommandProcessingSystem shutdown");

    public void Update(float deltaTime)
    {
        if (_world.Commands.GetCommands(out List<MoveCommand> movementCommands) && movementCommands.Count > 0)
        {
            Debug.Log($"MovementCommandProcessingSystem received {movementCommands.Count} MoveCommand(s)");
            foreach (MoveCommand command in movementCommands)
            {
                foreach (EntityID entityId in command.TargetEntityIDs)
                {
                    // Add or update the MovementTargetComponent for the entity with the target position from the command
                    if (!_world.TryGetComponentFromEntity(entityId, out MovementTargetComponent _))
                    {
                        _world.AddComponentToEntity(entityId, new MovementTargetComponent(command.TargetPosition));
                    }
                    else
                    {
                        ref MovementTargetComponent movementTargetComponentRef = ref _world.GetComponentFromEntity<MovementTargetComponent>(entityId);
                        movementTargetComponentRef.TargetPosition = command.TargetPosition;
                    }

                    // delete existingPathfindingRequest
                    if (_world.TryGetComponentFromEntity(entityId, out PathFindingRequest pathFindingRequest))
                    {
                        GameBootstrap.NavigationSystemManager.getPathFinder().cancelPathQuery(pathFindingRequest.requestId);
                        _world.RemoveComponentFromEntity<PathFindingRequest>(entityId);
                    }

                    // Submit a new pathfinding request for the entity
                    _world.GetEntityObject(entityId, out GameObject entityObject);
                    PathFindingRequest newPathfindingRequest = new((Vector2)entityObject.transform.position, (Vector2)command.TargetPosition, RequestIdGenerator.GenerateID());
                    _world.AddComponentToEntity(entityId, newPathfindingRequest);
                    PathQuery newPathQuery = new(newPathfindingRequest.StartPosition, newPathfindingRequest.EndPosition, newPathfindingRequest.requestId);
                    _pathFinder.submitPathQuery(newPathQuery);
                }
            }
        }
    }
}
