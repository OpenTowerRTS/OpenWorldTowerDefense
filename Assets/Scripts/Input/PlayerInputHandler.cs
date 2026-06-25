using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LeftClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit
            && hit.collider.gameObject.TryGetComponent<EntityView>(out EntityView entityView)
            && WorldBridge.World.TryGetComponentFromEntity<SelectableComponent>(entityView.EntityID, out SelectableComponent _))
            {
                WorldBridge.World.Commands.AddCommand(new SelectCommand(entityView.EntityID));
            }
            else
            {
                // If the raycast doesn't hit a selectable entity, we can consider it as clicking on empty space, which should trigger MoveCommand.
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                Vector3 targetPosition = new(worldPosition.x, worldPosition.y, 0); // Assuming a 2D game on the XY plane
                WorldBridge.World.Commands.AddCommand(new MoveCommand(WorldBridge.World.selectedEntities, targetPosition));
                Debug.Log($"Added MoveCommand for selected entities to move to position: {targetPosition}");
            }
        }
    }

    // For now, Rightclick mean unselected.
    public void RightClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            WorldBridge.World.Commands.AddCommand(new SelectCommand(null));
        }
    }
}
