using System.Collections.Generic;
using UnityEngine;

public class HighlightSystem : IGameSystem
{
    private List<EntityID> _highlightEntities;
    private World _world;
    public void Initialize(World world)
    {
        _world = world;
        _world.AddSystem(this);
        _highlightEntities = new List<EntityID>();
        Debug.Log("HighlightSystem initialized");
        world.EventBus.Subscribe<HighlightEntitiesEvent>(OnHighlightEntity);
    }

    public void Shutdown() => _highlightEntities.Clear();

    public void OnHighlightEntity(HighlightEntitiesEvent highlightEvent)
    {
        IReadOnlyList<EntityID> entityIDs = highlightEvent.EntityIDs;

        // unhighlight previously highlighted entities
        foreach (EntityID entityID in _highlightEntities)
        {
            // Logic to unhighlight the entity, e.g., remove highlight component or change material
            if (WorldBridge.World.GetEntityObject(entityID, out GameObject entityObject))
            {
                if (entityObject.TryGetComponent<HighlightDisplay>(out HighlightDisplay display))
                {
                    display.SetHighlight(false);
                }
            }
        }

        _highlightEntities = new List<EntityID>(entityIDs);
        Debug.Log($"HighlightSystem received HighlightEntitiesEvent for EntityIDs: {string.Join(", ", entityIDs)}");
        foreach (EntityID entityID in _highlightEntities)
        {
            // Logic to highlight the entity, e.g., add highlight component or change material
            if (WorldBridge.World.GetEntityObject(entityID, out GameObject entityObject))
            {
                if (entityObject.TryGetComponent<HighlightDisplay>(out HighlightDisplay display))
                {
                    display.SetHighlight(true);
                }
            }
        }
    }
}
