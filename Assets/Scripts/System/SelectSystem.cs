using System.Collections.Generic;
using UnityEngine;

public class SelectSystem : IUpdatableSystem, IBaseGameSystem
{
    // Public for now for easy debugging

    public EventBus eventBus;
    public List<EntityID> selectedEntities;
    private Queue<SelectCommand> _selectCommands;
    public void Initialize()
    {
        selectedEntities = new List<EntityID>();
        _selectCommands = new Queue<SelectCommand>();
        eventBus = new EventBus();
        // Initialization logic for the SelectSystem, if needed
    }

    public void EnqueueSelectCommand(SelectCommand command)
    {
        _selectCommands.Enqueue(command);
        Debug.Log($"Enqueued SelectCommand for EntityID: {command.targetEntityID}");
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
            if (command.targetEntityID is not EntityID targetEntityID)
            {
                Debug.Log("Remove all selected target");
                eventBus.Publish(selectedEntities);
                continue;
            }

            selectedEntities.Add(targetEntityID);

            Debug.Log($"Processed Event: Selected EntityID: {targetEntityID}");

            eventBus.Publish(selectedEntities);
        }
    }
}
