using System.Collections.Generic;

public readonly struct HighlightEntitiesEvent : IEvent
{
    public IReadOnlyList<EntityID> EntityIDs { get; }

    public HighlightEntitiesEvent(IReadOnlyList<EntityID> entityIDs) => EntityIDs = entityIDs;
}
