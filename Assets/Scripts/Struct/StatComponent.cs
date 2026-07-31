using UnityEngine;

// This struct strictly holds data for unit stats, following our ECS pure-data rule.
// It implements the IComponent interface so it can be registered with the World.
public struct StatComponent : IComponent
{
    // A multiplier applied to the base weapon damage. 
    // Example: 1.2 means the unit deals 20% extra damage.
    public float AttackMultiplier { get; set; }

    // A multiplier applied to the base movement speed. 
    // Example: 0.9 means the unit moves 10% slower.
    public float SpeedMultiplier { get; set; }

    // A flat bonus added to defense calculations to reduce incoming damage.
    // Example: 5.0 means it reduces damage by 5 points.
    public float DefenseBonus { get; set; }

    // This constructor allows us to easily initialize the component with specific values
    public StatComponent(float attackMultiplier = 1f, float speedMultiplier = 1f, float defenseBonus = 0f)
    {
        AttackMultiplier = attackMultiplier;
        SpeedMultiplier = speedMultiplier;
        DefenseBonus = defenseBonus;
    }
}
