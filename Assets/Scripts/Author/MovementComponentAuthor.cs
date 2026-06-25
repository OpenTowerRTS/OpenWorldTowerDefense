using UnityEngine;

public class MovementComponentAuthor : MonoBehaviour, IComponentAuthor
{
    public MovementComponent MovementComponent { get; private set; }
    public float maxSpeed = 5f; // Default max speed for movement
    public float arrivalRadius = 0.05f; // Default stoping distance, tweaked for default max speed

    public void Start() =>
        // Create a new MovementComponent instance
        MovementComponent = new MovementComponent(maxSpeed, arrivalRadius);

    // Register the MovementComponent with the world. This allows entities to have movement capabilities.
    public void RegisterToWorld(World world, EntityID entityId) => world.AddComponentToEntity<MovementComponent>(entityId, MovementComponent);
}
