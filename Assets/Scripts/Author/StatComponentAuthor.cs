using Unity.Entities;
using UnityEngine;

public class StatComponentAuthor : MonoBehaviour
{
    [Header("Offensive Stats")]
    public float baseAttack = 10f;
    public float baseSpeed = 5f;
    public float attackRange = 1.5f;

    [Header("Defensive Stats")]
    public float physicalDefense = 2f;
    public float magicDefense = 0f;

    public class StatComponentBaker : Baker<StatComponentAuthor>
    {
        public override void Bake(StatComponentAuthor authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new StatComponent
            {
                BaseAttack = authoring.baseAttack,
                BaseSpeed = authoring.baseSpeed,
                AttackRange = authoring.attackRange,
                PhysicalDefense = authoring.physicalDefense,
                MagicDefense = authoring.magicDefense
            });
        }
    }
}
