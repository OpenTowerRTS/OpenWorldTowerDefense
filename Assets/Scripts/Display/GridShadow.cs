using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PolygonCollider2D))]
public class GridShadow : MonoBehaviour
{
    public GameObject shadowPrefab;

    public int GridSize = 6;

    public List<Vector2Int> _activePoints;

    private SpriteRenderer[,] _sprites;

    private PolygonCollider2D _collider;

    public Color _tileFreeColor = new(0.6f, 1f, 0.6f, 0.5f); // Light green
    public Color _tileBlockedColor = new(1f, 0.6f, 0.6f, 0.5f); // White with 10% opacity

    public void Start()
    {
        _collider = GetComponent<PolygonCollider2D>();
        _sprites = new SpriteRenderer[GridSize, GridSize];
        CreateGridShadows();
        UpdateActivePoints();
        UpdateCollider();
    }

    //public void update Check with the overal grid and the collider to see if there is anything blocking the grid and update the color of the shadows accordingly
    private void CreateGridShadows()
    {
        for (int x = 0; x < GridSize; x++)
        {
            for (int y = 0; y < GridSize; y++)
            {
                Vector2 position = GridUtils.GridToWorld(new Vector2(x, y));
                GameObject shadow = Instantiate(shadowPrefab, position, Quaternion.identity, transform);
                shadow.name = $"Shadow_{x}_{y}";
                _sprites[x, y] = shadow.GetComponent<SpriteRenderer>();
                shadow.SetActive(false);
            }
        }
    }

    private void UpdateCollider()
    {
        Debug.Log("GridShadow: Creating PolygonCollider2D with active points: " + string.Join(", ", _activePoints));
        List<Vector2> expanded = new();
        foreach (Vector2 p in _activePoints)
        {
            expanded.Add(new Vector2(p.x, p.y));
            expanded.Add(new Vector2(p.x + 1f, p.y));
            expanded.Add(new Vector2(p.x + 1f, p.y + 1f));
            expanded.Add(new Vector2(p.x, p.y + 1f));
        }
        Debug.Log("GridShadow: Expanded points for collider: " + string.Join(", ", expanded));
        List<Vector2> points = MeshUtils.ConvexHull(expanded.ConvertAll(p => GridUtils.GridToWorld(new Vector2(p.x, p.y))));
        Debug.Log("GridShadow: Creating PolygonCollider2D with points: " + string.Join(", ", points));
        _collider.SetPath(0, points);
        if (TryGetComponent<GridSnapper>(out GridSnapper snapper))
        {
            snapper.UpdateOffset();
        }
    }

    private void UpdateActivePoints()
    {
        // Load the active points from the serialized field
        foreach (Vector2Int point in _activePoints)
        {
            if (point.x < 0 || point.x >= GridSize || point.y < 0 || point.y >= GridSize)
            {
                Debug.LogWarning($"Active point {point} is out of bounds for grid size {GridSize}. Skipping.");
                continue;
            }
            _sprites[point.x, point.y].gameObject.SetActive(true); // Activate the shadow for the active point
            _sprites[point.x, point.y].color = _tileFreeColor;
        }
    }
}
