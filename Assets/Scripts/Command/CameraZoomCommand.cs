public readonly struct CameraZoomCommand : ICommand
{
    public readonly float ZoomDelta;

    public CameraZoomCommand(float zoomDelta) => ZoomDelta = zoomDelta;
}
