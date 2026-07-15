using UnityEngine;

public static class GridUtils
{
    public static float tileWidth = 1.0f; // Width of a single tile in world units
    public static float tileHeight = 0.5f; // Height of a single tile in world units. 1:2 to width ratio for isometric tiles
    public static Vector2 WorldToGrid(Vector2 worldPosition, bool roundToNearest = true)
    {
        float x = (worldPosition.x / tileWidth) + (worldPosition.y / tileHeight);
        float y = (worldPosition.y / tileHeight) - (worldPosition.x / tileWidth);
        if (roundToNearest)
        {
            x = Mathf.Floor(x);
            y = Mathf.Floor(y);
        }
        return new Vector2(x, y);
    }

    public static Vector2 GridToWorld(Vector2 gridPosition)
    {
        float x = (gridPosition.x - gridPosition.y) * tileWidth / 2;
        float y = (gridPosition.x + gridPosition.y) * tileHeight / 2;
        return new Vector2(x, y);
    }
}
