using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Vector2 _currentMoveInput;

    // Dispatches the command every frame the player holds a movement key or touches the screen edge
    private void Update()
    {
        Vector2 totalPan = _currentMoveInput;

        // Edge Panning: Only run if the Alt key is NOT being held down
        if (Keyboard.current != null && !Keyboard.current.altKey.isPressed)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            float edgeSize = 25f; // How many pixels from the edge triggers the movement

            // Make sure the mouse is actually inside the game window
            if (mousePos.x >= 0 && mousePos.x <= Screen.width &&
                mousePos.y >= 0 && mousePos.y <= Screen.height)
            {
                if (mousePos.x < edgeSize)
                {
                    totalPan.x -= 1f;
                }
                else if (mousePos.x > Screen.width - edgeSize)
                {
                    totalPan.x += 1f;
                }

                if (mousePos.y < edgeSize)
                {
                    totalPan.y -= 1f;
                }
                else if (mousePos.y > Screen.height - edgeSize)
                {
                    totalPan.y += 1f;
                }
            }
        }

        // Send the final movement command to the ECS world if there is any input
        if (totalPan != Vector2.zero && WorldBridge.World != null)
        {
            WorldBridge.World.Commands.AddCommand(new CameraMoveCommand(totalPan));
        }
    }

    public void Move(InputAction.CallbackContext context) => _currentMoveInput = context.ReadValue<Vector2>();

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
