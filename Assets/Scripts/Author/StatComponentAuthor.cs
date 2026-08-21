using UnityEngine;

public class StatComponentAuthor : MonoBehaviour, IComponentAuthor
{
    // Fix: Used PascalCase for public fields to match project conventions
    [Header("Offensive Stats")]
    public float BaseAttack = 10f;
    public float BaseSpeed = 5f;
    public float AttackRange = 1.5f;

    [Header("Defensive Stats")]
    public float PhysicalDefense = 2f;
    public float MagicDefense = 0f;

    // Fix: Replaced Unity Baker with your custom ECS registration method
    public void RegisterToWorld(World world, EntityID entityId)
    {
        // Package the inspector values into the pure data struct
        StatComponent statComponent = new()
        {
            BaseAttack = BaseAttack,
            BaseSpeed = BaseSpeed,
            AttackRange = AttackRange,
            PhysicalDefense = PhysicalDefense,
            MagicDefense = MagicDefense
        };

        // Add to your custom World registry
        world.AddComponentToEntity(entityId, statComponent);
    }
}
