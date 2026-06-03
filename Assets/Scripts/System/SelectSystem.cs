using System.Collections.Generic;
using UnityEngine;

public class SelectSystem : IUpdatableSystem, IBaseGameSystem
{
    // Public for now for easy debugging

    public List<EntityID> selectedEntities;
    private Queue<SelectCommand> _selectCommands;
    private World _world;
    public void Initialize(World world)
    {
        _world = world;
        _world.AddSystem(this);
        selectedEntities = new List<EntityID>();
        _selectCommands = new Queue<SelectCommand>();
        // Initialization logic for the SelectSystem, if needed
    }

    public void EnqueueSelectCommand(SelectCommand command)
    {
        _selectCommands.Enqueue(command);
        Debug.Log($"Enqueued SelectCommand for EntityID: {command.TargetEntityID}");
    }

    public void Shutdown()
    {
        selectedEntities.Clear();
        _selectCommands.Clear();
        // Cleanup logic for the SelectSystem, if needed
    }

    // For now, only allowed to select one entity and selection cleared when selecting another entity.
    public void Update(float deltaTime)
    {
        while (_selectCommands.Count > 0)
        {
            SelectCommand command = _selectCommands.Dequeue();

            // For simplicity, only one entity can be selected at a time.
            selectedEntities.Clear();
            if (command.TargetEntityID is not EntityID targetEntityID)
            {
                Debug.Log("Remove all selected target");
                _world.EventBus.Publish(new HighlightEntitiesEvent(selectedEntities));
                continue;
            }

            selectedEntities.Add(targetEntityID);

            Debug.Log($"Processed Event: Selected EntityID: {targetEntityID}");

            _world.EventBus.Publish(new HighlightEntitiesEvent(selectedEntities));
        }
    }
}
