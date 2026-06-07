using UnityEngine;

public struct MovementTargetComponent : IComponent
{
    public Vector3 TargetPosition { get; set; } // Position the entity is moving towards

    public MovementTargetComponent(Vector3 targetPosition) => TargetPosition = targetPosition;
}
