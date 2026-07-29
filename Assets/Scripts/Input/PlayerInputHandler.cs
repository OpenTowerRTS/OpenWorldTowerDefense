using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UnityEngine.InputSystem.PlayerInput))] // Fixes UNT0039
public class PlayerInputHandler : MonoBehaviour
{
    // Explicitly use Unity's namespace to avoid the naming collision (Fixes CS1061 and UNT0014)
    private UnityEngine.InputSystem.PlayerInput _playerInput;
    private InputAction _moveAction;

    private void Awake()
    {
        _playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
    }

    private void Update()
    {
        Vector2 totalPan = _moveAction.ReadValue<Vector2>();

        if (Keyboard.current != null && !Keyboard.current.altKey.isPressed && Camera.main != null)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            float edgeSize = 25f; // Fixed physical pixel size

            // TanoVip123 PR Fix: Removed the unnecessary screen bounds wrapper. 
            // A simple threshold check is sufficient.

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

        if (totalPan != Vector2.zero && WorldBridge.World != null)
        {
            WorldBridge.World.Commands.AddCommand(new CameraMoveCommand(totalPan));
        }
    }

    // Epic #5 Zoom Method
    public void Zoom(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<Vector2>().y;

        if (scrollValue != 0 && WorldBridge.World != null)
        {
            float normalizedScroll = Mathf.Clamp(scrollValue, -1f, 1f);
            WorldBridge.World.Commands.AddCommand(new CameraZoomCommand(normalizedScroll));
        }
    }

    public void LeftClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit &&
                hit.collider.gameObject.TryGetComponent<EntityView>(out EntityView entityView) &&
                WorldBridge.World.TryGetComponentFromEntity<SelectableComponent>(entityView.EntityID, out SelectableComponent _))
            {
                WorldBridge.World.Commands.AddCommand(new SelectCommand(entityView.EntityID));
            }
            else
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                Vector3 targetPosition = new(worldPosition.x, worldPosition.y, 0);
                WorldBridge.World.Commands.AddCommand(new MoveCommand(WorldBridge.World.selectedEntities, targetPosition));
                Debug.Log($"Added MoveCommand for selected entities to move to position: {targetPosition}");
            }
        }
    }

    public void RightClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            WorldBridge.World.Commands.AddCommand(new SelectCommand(null));
        }
    }
}
