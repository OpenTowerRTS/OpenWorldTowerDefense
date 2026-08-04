using Unity.Entities;

public struct StatComponent : IComponentData
{
    public float BaseAttack;
    public float BaseSpeed;
    public float AttackRange;
    public float PhysicalDefense;
    public float MagicDefense;
}
