using UnityEngine;

// This class acts as a bridge between the Unity Editor and our ECS World.
// By inheriting from MonoBehaviour, we can attach it to prefabs.
// By implementing IComponentAuthor, the EntityView knows it needs to register this to the World.
public class StatComponentAuthor : MonoBehaviour, IComponentAuthor
{
    // These fields will appear in the Unity Inspector so you can easily tweak stats per unit.
    [Header("Unit Stats")]
    [Tooltip("Multiplier for base weapon damage. (e.g., 1.2 = +20% damage)")]
    public float attackMultiplier = 1f;

    [Tooltip("Multiplier for base movement speed. (e.g., 0.8 = -20% speed)")]
    public float speedMultiplier = 1f;

    [Tooltip("Flat reduction to incoming damage.")]
    public float defenseBonus = 0f;

    // This property holds the actual ECS component data.
    public StatComponent StatComponent { get; private set; }

    // Start is called by Unity before the first frame update.
    public void Start() =>
        // We take the values set in the Unity Inspector and package them into our pure ECS data struct.
        StatComponent = new StatComponent(attackMultiplier, speedMultiplier, defenseBonus);

    // This method is required by the IComponentAuthor interface. 
    // It is called by the EntityView script to inject this component into the ECS World.
    public void RegisterToWorld(World world, EntityID entityId) =>
        // Adds the StatComponent to the specific entity in the World's component registry.
        world.AddComponentToEntity<StatComponent>(entityId, StatComponent);
}
