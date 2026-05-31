using System.Collections.Generic;
using UnityEngine;

public class HighlightSystem : IGameSystem
{
    private List<EntityID> _highlightEntities;
    public void Initialize()
    {
        _highlightEntities = new List<EntityID>();
        Debug.Log("HighlightSystem initialized");
    }

    public void Shutdown() => _highlightEntities.Clear();

    public void OnHighlightEntity(List<EntityID> entityIDs)
    {
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
