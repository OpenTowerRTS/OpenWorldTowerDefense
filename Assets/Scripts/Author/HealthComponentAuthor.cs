using UnityEngine;

public class HealthComponentAuthor : MonoBehaviour, IComponentAuthor
{
    public float maxHealth = 100f;

    public void RegisterToWorld(World world, EntityID entityId)
    {
        // Creates the pure data instance and safely adds it to the ECS database
        HealthComponent health = new(maxHealth);
        world.AddComponentToEntity<HealthComponent>(entityId, health);
    }
}
