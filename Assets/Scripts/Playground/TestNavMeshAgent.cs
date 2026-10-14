using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class TestNavMeshAgent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private NavMeshAgent agent;
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Debug.Log("TestNavMeshAgent: Start: agent: " + agent);
        // Need these, otherwise the agent will rotate in 3D space, making it disappear from the 2D view.
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    public void LeftClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3 targetPosition = new(worldPosition.x, worldPosition.y, 0);
            Debug.Log("TestNavMeshAgent: LeftClick: targetPosition: " + targetPosition);
            agent.SetDestination(targetPosition);
            NavMeshPath path = new();
            agent.CalculatePath(targetPosition, path);
            for (int i = 0; i < path.corners.Length; i++)
            {
                Debug.Log("TestNavMeshAgent: LeftClick: path corner " + i + ": " + path.corners[i]);
            }
        }
    }
}
