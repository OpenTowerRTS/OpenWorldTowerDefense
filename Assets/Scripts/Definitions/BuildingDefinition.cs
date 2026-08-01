using System;
using UnityEngine;

[Serializable]
public struct BuildingId
{
    public string Value;

    public BuildingId(string value) => Value = value;

    public readonly bool Equals(BuildingId other)
        => Value == other.Value;

    public override readonly bool Equals(object obj)
        => obj is BuildingId other && Equals(other);

    public override readonly int GetHashCode()
        => Value.GetHashCode();

    public override readonly string ToString()
        => Value;
}

[CreateAssetMenu(fileName = "BuildingDefinition", menuName = "Scriptable Objects/BuildingDefinition")]
public class BuildingDefinition : ScriptableObject
{
    public BuildingId Id;
    public BuildingType BuildingType;
    public GameObject BuildingPrefab;
    public GameObject BuildingShadow;
    public Sprite BuildingIcon;
    public Vector2Int[] GridOccupancy;
}
