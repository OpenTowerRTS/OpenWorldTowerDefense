using System.Collections.Generic;
using UnityEngine;

public readonly struct MovementCommand : ICommand
{
    public List<EntityID> TargetEntityIDs { get; }
    public Vector3 TargetPosition { get; }

    public MovementCommand(List<EntityID> targetEntityIDs, Vector3 targetPosition)
    {
        TargetEntityIDs = targetEntityIDs;
        TargetPosition = targetPosition;
    }
}
