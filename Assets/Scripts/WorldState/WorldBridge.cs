using System.Collections.Generic;
using UnityEngine;

public class WorldBridge : MonoBehaviour
{
    public static World World { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {
        World = new World();
        World.Initialize();

        // Temporary: Add SelectSystem to the world for testing. In the future, systems should be added and initialized by a Central Manager.
        SelectSystem selectSystem = new();
        HighlightSystem highlightSystem = new();
        MovementCommandProcessingSystem movementCommandProcessingSystem = new();
        MovementResolutionSystem movementResolutionSystem = new();
        PathFindingSystem pathFindingSystem = new();
        PathFollowingSystem pathFollowingSystem = new();
        PhysicSyncSystem physicSyncSystem = new();
        GridSnapSystem gridSnapSystem = new();

        selectSystem.Initialize(World);
        highlightSystem.Initialize(World);
        movementCommandProcessingSystem.Initialize(World);
        movementResolutionSystem.Initialize(World);
        pathFindingSystem.Initialize(World);
        pathFollowingSystem.Initialize(World);
        physicSyncSystem.Initialize(World);
        gridSnapSystem.Initialize(World);

        World.Phases[World.EWorldPhase.Command].AddSystem(selectSystem);
        World.Phases[World.EWorldPhase.Presentation].AddSystem(gridSnapSystem);
        World.Phases[World.EWorldPhase.Presentation].AddSystem(highlightSystem);
        World.Phases[World.EWorldPhase.Command].AddSystem(movementCommandProcessingSystem);
        World.Phases[World.EWorldPhase.Simulation].AddSystem(movementResolutionSystem);
        World.Phases[World.EWorldPhase.Simulation].AddSystem(pathFindingSystem);
        World.Phases[World.EWorldPhase.Simulation].AddSystem(pathFollowingSystem);
        World.Phases[World.EWorldPhase.Simulation].AddSystem(physicSyncSystem);
    }
    // Initialize the World and Central Manager here

    // public void Start() {}

    // Update is called once per frame
    public void Update() => World.Update(Time.deltaTime);
    public void FixedUpdate() => World.FixedUpdate(Time.fixedDeltaTime);
}
