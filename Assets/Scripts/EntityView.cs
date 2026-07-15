using UnityEngine;
public class EntityView : MonoBehaviour
{
    public EntityID EntityID { get; private set; }

    public void Start()
    {
        EntityID = WorldBridge.World.RegisterEntity(gameObject);
        foreach (IComponentAuthor author in GetComponents<IComponentAuthor>())
        {
            author.RegisterToWorld(WorldBridge.World, EntityID);
        }

        // Need to traverse through children to find all PhysicalBridge components and register them to the world as well.
    }
}
