using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : IGameSystem, IUpdatableSystem
{
    private World _world;
    private readonly float _panSpeed = 15f; // Made readonly to fix IDE0044
    private readonly float _zoomSpeed = 2f;
    private readonly float _minZoom = 5f;
    private readonly float _maxZoom = 20f;
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
        // Check for zoom commands
        if (_world.Commands.GetCommands(out List<CameraZoomCommand> zoomCommands))
        {
            float totalZoom = 0;
            foreach (CameraZoomCommand cmd in zoomCommands)
            {
                totalZoom += cmd.ZoomDelta;
            }

            if (totalZoom != 0 && Camera.main != null)
            {
                if (Camera.main.orthographic)
                {
                    // For Orthographic 2.5D cameras
                    Camera.main.orthographicSize -= totalZoom * _zoomSpeed;
                    Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, _minZoom, _maxZoom);
                }
                else
                {
                    // For Perspective 2.5D cameras (adjusting the Z axis or Field of View)
                    Camera.main.fieldOfView -= totalZoom * _zoomSpeed * 5f; // FOV scales faster
                    Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 15f, 90f);
                }
            }
        }
    }
}
