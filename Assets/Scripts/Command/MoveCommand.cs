using System.Collections.Generic;
using UnityEngine;

public readonly struct MoveCommand : ICommand
{
    public List<EntityID> TargetEntityIDs { get; }
    public Vector3 TargetPosition { get; }

    public MoveCommand(List<EntityID> targetEntityIDs, Vector3 targetPosition)
    {
        TargetEntityIDs = targetEntityIDs;
        TargetPosition = targetPosition;
    }
}
