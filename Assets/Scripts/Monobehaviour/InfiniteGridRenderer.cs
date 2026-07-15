using UnityEngine;

public class InfiniteGridRenderer : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private void LateUpdate() => transform.position = new Vector3(
            mainCamera.transform.position.x,
            mainCamera.transform.position.y,
            0);
}
