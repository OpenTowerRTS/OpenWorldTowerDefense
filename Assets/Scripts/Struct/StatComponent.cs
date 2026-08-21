// Removed 'using Unity.Entities;' since we are using your custom ECS

public struct StatComponent : IComponent
{
    public float BaseAttack;
    public float BaseSpeed;
    public float AttackRange;
    public float PhysicalDefense;
    public float MagicDefense;
}
