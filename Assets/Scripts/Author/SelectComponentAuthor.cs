using System.Collections.Generic;
using UnityEngine;

public class SelectComponentAuthor : MonoBehaviour, IComponentAuthor
{
    public SelectableComponent SelectableComponent { get; private set; }

    public void Start() =>
        // Create a new SelectableComponent instance
        SelectableComponent = new SelectableComponent();

    // Register the SelectableComponent with the world. This allows entities to be marked as selectable.
    public void RegisterToWorld(World world, EntityID entityId) => world.AddComponentToEntity<SelectableComponent>(entityId, SelectableComponent);
}
