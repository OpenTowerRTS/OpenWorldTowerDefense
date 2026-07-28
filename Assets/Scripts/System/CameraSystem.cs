using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : IGameSystem, IUpdatableSystem
{
    private World _world;
    private readonly float _panSpeed = 15f;
    private readonly float _zoomSpeed = 50f;
    private readonly float _minZoom = 2f;
    private readonly float _maxZoom = 15f;

    public void Initialize(World world)
    {
        _world = world;
        _world.AddSystem(this);
        Debug.Log("CameraSystem initialized");
    }

    public void Shutdown() => Debug.Log("CameraSystem shutdown");

    public void Update(float deltaTime)
    {
        // TanoVip123's PR feedback: Early exit to optimize and remove duplicate checks
        if (Camera.main == null)
        {
            return;
        }

        // 1. Handle Camera Panning
        if (_world.Commands.GetCommands(out List<CameraMoveCommand> moveCommands))
        {
            Vector2 totalMove = Vector2.zero;

            foreach (CameraMoveCommand cmd in moveCommands)
            {
                totalMove += cmd.Direction;
            }

            if (totalMove != Vector2.zero)
            {
                totalMove.Normalize();
                Vector3 moveDirection = totalMove; // Implicitly converts Vector2 to Vector3
                Camera.main.transform.position += moveDirection * (_panSpeed * deltaTime);
            }
        }

        // 2. Handle Camera Zooming (Strictly Orthographic)
        if (_world.Commands.GetCommands(out List<CameraZoomCommand> zoomCommands))
        {
            foreach (CameraZoomCommand cmd in zoomCommands)
            {
                if (Camera.main.orthographic)
                {
                    // Applied deltaTime for frame-rate independence
                    float zoomAmount = cmd.ZoomDelta * _zoomSpeed * deltaTime;
                    Camera.main.orthographicSize -= zoomAmount;
                    Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, _minZoom, _maxZoom);
                }
            }
        }
    }
}
