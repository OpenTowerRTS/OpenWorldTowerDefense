using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// This is driving the waypoint to waypoint movement in Path Component
public class PathFollowingSystem : IGameSystem, IFixedUpdatableSystem
{
    private World _world;
    public void Initialize(World world)
    {
        _world = world;
        _world.AddSystem(this);
        Debug.Log("PathFollowingSystem initialized");
    }

    public void Shutdown() => Debug.Log("PathFollowingSystem shutdown");

    public void FixedUpdate(float deltaTime)
    {
        IEnumerable<EntityID> entities = _world.GetEntitiesWithComponent<PathComponent>();
        Debug.Log($"PathFollowingSystem processing: {entities.Count()} entities");
        foreach (EntityID entityId in entities)
        {
            ref PathComponent pathComponent = ref _world.GetComponentFromEntity<PathComponent>(entityId);
            int currIndex = pathComponent.CurrentPathIndex;
            int pathLength = pathComponent.PathPoints.Length;
            if (pathComponent.CurrentPathIndex == pathLength)
            {
                Debug.Log($"NavigationSystemTest Destination reached currIndex:{currIndex}; pathLength: {pathComponent.PathPoints.Length}");
                continue;
            }
            Debug.Log($"NavigationSystemTest CurrentPath {string.Join(", ", pathComponent.PathPoints)}");
            // If we reach the end of the path, skip
            Debug.Log($"NavigationSystemTest currIndex:{currIndex}; pathLength: {pathComponent.PathPoints.Length}");
            Debug.Log($"NavigationSystemTest CrossCheck CurrentPathIndex: {currIndex} CurrentPathPoint: {pathComponent.PathPoints[currIndex]}");

            if (_world.GetEntityObject(entityId, out GameObject entityObject))
            {
                // Move forward if are at the way point
                ref MovementComponent movementComponent = ref _world.GetComponentFromEntity<MovementComponent>(entityId);
                // Debug.Log($"NavigationSystemTest PathFollowingSystem: distance of entity {entityId}: {dist}");
                Debug.Log($"NavigationSystemTest PathShoudAdvance: {pathComponent.ShouldAdvanceNextFrame}");
                // Debug.Log($"NavigationSystemTest ArrivalRadius: {dist < movementComponent.ArrivalRadius}");
                if (pathComponent.ShouldAdvanceNextFrame)
                {
                    pathComponent.ShouldAdvanceNextFrame = false;
                    currIndex += 1;
                    pathComponent.CurrentPathIndex += 1;
                    Debug.Log($"NavigationSystemTest UpdateIndex CurrIndex {currIndex} CurrentPathIndex {pathComponent.CurrentPathIndex}, PathShoudAdvanceRest: {pathComponent.ShouldAdvanceNextFrame}");
                }
                if (currIndex < pathLength)
                {
                    Debug.Log($"NavigationSystemTest CurrentPathIndex: {currIndex} CurrentPathPoint: {pathComponent.PathPoints[currIndex]}");
                }
                else
                {
                    Debug.Log($"NavigationSystemTest Destination reached currIndex:{currIndex}; pathLength: {pathComponent.PathPoints.Length}");
                }

                //in C#, a method that returns ref T can be used in two different ways:
                // As a reference (alias) to storage
                // As a value obtained from that reference

                if (currIndex < pathLength)
                {
                    float dist = Vector3.Distance(entityObject.transform.position, pathComponent.PathPoints[currIndex]);
                    Vector3 dir = Vector3.Normalize(pathComponent.PathPoints[currIndex] - entityObject.transform.position);
                    movementComponent.CurrSpeed = dir * movementComponent.MaxSpeed;
                    Debug.Log($"NavigationSystemTest Moving entity {entityId} to {pathComponent.PathPoints[currIndex]} at speed {movementComponent.CurrSpeed}");
                    Debug.Log($"NavigationSystemTest Speed from database = {_world.GetComponentFromEntity<MovementComponent>(entityId).CurrSpeed}");
                    pathComponent.ShouldAdvanceNextFrame = movementComponent.CurrSpeed.magnitude * deltaTime > dist;
                    Debug.Log($"NavigationSystemTest NextFrameDistance:{movementComponent.CurrSpeed.magnitude * deltaTime}, dist: {dist}, shouldAdvance: {pathComponent.ShouldAdvanceNextFrame}");

                }
                Debug.Log("NavigationSystemTest loopdone");

            }
        }
    }
}
