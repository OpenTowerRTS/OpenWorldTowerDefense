using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : IGameSystem, IUpdatableSystem
{
    private World _world;
    private readonly float _panSpeed = 15f; // Made readonly to fix IDE0044

    public void Initialize(World world)
    {
        _world = world;
        _world.AddSystem(this);
        Debug.Log("CameraSystem initialized");
    }

    public void Shutdown() => Debug.Log("CameraSystem shutdown");

    public void Update(float deltaTime)
    {
        // Check if there are any camera movement commands waiting in the buffer
        if (_world.Commands.GetCommands(out List<CameraMoveCommand> commands))
        {
            Vector2 totalMove = Vector2.zero;

            // Accumulate all movement inputs for this frame
            foreach (CameraMoveCommand cmd in commands)
            {
                totalMove += cmd.Direction;
            }

            if (totalMove != Vector2.zero && Camera.main != null)
            {
                totalMove.Normalize();

                // Implicitly converts Vector2 to Vector3 (fixes UNT0035 and IDE0090)
                Vector3 moveDirection = totalMove;
                Camera.main.transform.position += moveDirection * (_panSpeed * deltaTime);
            }
        }
    }
}
