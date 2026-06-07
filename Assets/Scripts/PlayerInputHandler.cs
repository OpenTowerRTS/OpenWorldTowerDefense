using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LeftClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit
            && hit.collider.gameObject.TryGetComponent<EntityView>(out EntityView entityView)
            && WorldBridge.World.GetComponentFromEntity<SelectableComponent>(entityView.EntityID, out SelectableComponent _))
            {
                WorldBridge.World.Commands.AddCommand(new SelectCommand(entityView.EntityID));
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
