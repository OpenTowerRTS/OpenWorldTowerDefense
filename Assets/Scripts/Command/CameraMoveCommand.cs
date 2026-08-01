using UnityEngine;

public readonly struct CameraMoveCommand : ICommand
{
    public readonly Vector2 Direction;

    public CameraMoveCommand(Vector2 direction) => Direction = direction;
}
