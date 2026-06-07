using System.Collections.Generic;
using UnityEngine;

public class MovementCommandProcessingSystem : IGameSystem, IFixedUpdatableSystem
{
    private World _world;
    public void Initialize(World world)
    {
        _world = world;
        _world.AddSystem(this);
        Debug.Log("MovementCommandProcessingSystem initialized");
    }


    public void Shutdown() => Debug.Log("MovementCommandProcessingSystem shutdown");

    public void FixedUpdate(float deltaTime)
    {
        if (_world.Commands.GetCommands<MovementCommand>(out List<MovementCommand> movementCommands) && movementCommands.Count > 0)
        {
            foreach (MovementCommand command in movementCommands)
            {
                foreach (EntityID entityId in command.TargetEntityIDs)
                {
                    if (_world.GetComponentFromEntity<MovementComponent>(entityId, out MovementComponent _))
                    {
                        // Add or update the MovementTargetComponent for the entity with the target position from the command
                        if (_world.GetComponentFromEntity<MovementTargetComponent>(entityId, out MovementTargetComponent movementTargetComponent))
                        {
                            _world.AddComponentToEntity<MovementTargetComponent>(entityId, new MovementTargetComponent(command.TargetPosition));
                        }
                        else
                        {
                            movementTargetComponent.TargetPosition = command.TargetPosition;
                        }
                    }
                }
            }
        }
    }
}
