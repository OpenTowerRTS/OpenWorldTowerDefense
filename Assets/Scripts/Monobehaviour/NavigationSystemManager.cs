using NavMeshPlus.Components;
using UnityEngine;
public class NavigationSystemManager : MonoBehaviour
{
    private readonly GameObject navMeshObjectPrefab;
    private readonly GameObject defaultPathFinderPrefab;
    private NavMeshSurface _navMeshInstance;
    private bool _isNavMeshDirty = false;
    private IPathFinder _pathFinder;
    public void Awake()
    {
        _navMeshInstance = FindFirstObjectByType<NavMeshSurface>();
        // Create NavMeshObject if it doesn't exist
        if (_navMeshInstance == null)
        {
            _navMeshInstance = Instantiate(navMeshObjectPrefab, transform).GetComponent<NavMeshSurface>();
        }
        _navMeshInstance.BuildNavMesh();
        _pathFinder = Instantiate(defaultPathFinderPrefab, transform).GetComponent<DefaultPathFinder>();
        Debug.Log("NavigationSystemTest: Initialized");
    }

    public void Update()
    {
        if (_isNavMeshDirty)
        {
            Debug.Log("NavigationSystemManager: Rebuilding NavMesh because it is marked as dirty.");
            _navMeshInstance.BuildNavMesh();
            _isNavMeshDirty = false;
        }
    }

    public void MarkNavMeshDirty() => _isNavMeshDirty = true;

    public IPathFinder getPathFinder() => _pathFinder;
}
