using System;

public static class ComponentRegistry
{
    public static readonly Type[] Types =
    {
        typeof(MovementComponent),
        typeof(SelectableComponent),
        typeof(MovementTargetComponent),
        typeof(PathComponent),
        typeof(GridSnappableComponent),
    };
}
