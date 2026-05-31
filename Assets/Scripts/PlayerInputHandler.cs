using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Click(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            SelectCommand selectCommand = new();
            if (hit)
            {
                if (hit.collider.gameObject.TryGetComponent<EntityView>(out EntityView entityView))
                {
                    if (WorldBridge.World.GetComponentFromEntity<SelectableComponent>(entityView.EntityID, out SelectableComponent _))
                    {
                        selectCommand = new()
                        { targetEntityID = entityView.EntityID };
                    }
                }
            }
            WorldBridge.World.GetSystem<SelectSystem>()?.EnqueueSelectCommand(selectCommand);

        }
    }
}
