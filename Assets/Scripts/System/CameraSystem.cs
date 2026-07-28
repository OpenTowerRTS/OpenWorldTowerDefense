using UnityEngine;

public class CameraSystem : ISystem
{
    private readonly float _moveSpeed = 15f;
    private readonly float _zoomSpeed = 50f;
    private readonly float _minZoom = 2f; 
    private readonly float _maxZoom = 15f;

    public void Update(float deltaTime)
    {
        // TanoVip123's Refactor: Early exit if the camera is missing
        if (Camera.main == null) 
        {
            return;
        }

        // 1. Handle Camera Panning
        foreach (var moveCommand in WorldBridge.World.Commands.GetCommands<CameraMoveCommand>())
        {
            Vector3 moveDirection = new Vector3(moveCommand.PanDirection.x, moveCommand.PanDirection.y, 0).normalized;
            Camera.main.transform.position += moveDirection * _moveSpeed * deltaTime;
        }

        // 2. Handle Camera Zooming (Strictly Orthographic now)
        foreach (var zoomCommand in WorldBridge.World.Commands.GetCommands<CameraZoomCommand>())
        {
            if (Camera.main.orthographic)
            {
                // Applied deltaTime for frame-rate independent zooming
                float zoomAmount = zoomCommand.ZoomDelta * _zoomSpeed * deltaTime;
                
                Camera.main.orthographicSize -= zoomAmount;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, _minZoom, _maxZoom);
            }
        }
    }
}
