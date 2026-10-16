using System.Collections.Generic;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    public static World World { get; private set; }

    // Temporary leave this BuildingSystemManager variable in GameBootstrap for testing. In the future, we want a GameManager that handle GameBootStrap and other game management tasks.
    private readonly BuildingSystemManager buildingSystemManager;
    public static BuildingSystemManager BuildingSystemManager { get; private set; }

    // Similarly, Temporary leave this BuildingSystemManager variable in GameBootstrap for testing.
    private readonly NavigationSystemManager navigationSystemManager;
    public static NavigationSystemManager NavigationSystemManager { get; private set; }

    // Temporary leave this definitionDatabase variable in GameBootstrap for testing. In the future, we want a GameManager that handle GameBootStrap and other game management tasks.
    // In fact, The World should live in GameManager, not in GameBootstrap. GameBootstrap should only be responsible for initializing the World and other systems, and then pass the World to GameManager.
    private readonly DefinitionDatabase definitionDatabase;
    public static DefinitionDatabase DefinitionDatabase { get; private set; }

    private readonly PlayerInputHandler playerInputHandler;
    public static PlayerInputHandler PlayerInputHandler { get; set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private static GameBootstrap _staticGameBootstrap;
    public void Awake()
    {
        if (_staticGameBootstrap != null && _staticGameBootstrap != this)
        {
            Destroy(gameObject);
            return;
        }
        _staticGameBootstrap = this;

        World = new World();
        World.Initialize();
        if (definitionDatabase != null)
        {
            DefinitionDatabase = definitionDatabase;
            DefinitionDatabase.Initialize();
        }

        if (buildingSystemManager != null)
        {
            BuildingSystemManager = buildingSystemManager;
        }

        if (navigationSystemManager != null)
        {
            NavigationSystemManager = navigationSystemManager;
        }

        if (playerInputHandler != null)
        {
            PlayerInputHandler = playerInputHandler;
        }
    }
    // Initialize the World and Central Manager here

    public void Start()
    {
        // Temporary: Add SelectSystem to the world for testing. In the future, systems should be added and initialized by a Central Manager.
        SelectSystem selectSystem = new();
        HighlightSystem highlightSystem = new();
        MovementCommandProcessingSystem movementCommandProcessingSystem = new(NavigationSystemManager.getPathFinder());
        MovementResolutionSystem movementResolutionSystem = new();
        PathFindingSystem pathFindingSystem = new(NavigationSystemManager.getPathFinder());
        PathFollowingSystem pathFollowingSystem = new();
        PhysicSyncSystem physicSyncSystem = new();
        GridSnapSystem gridSnapSystem = new();
        GridMapSystem gridMapSystem = new();
        BuildingSystem buildingSystem = new();

        selectSystem.Initialize(World);
        highlightSystem.Initialize(World);
        movementCommandProcessingSystem.Initialize(World);
        movementResolutionSystem.Initialize(World);
        pathFindingSystem.Initialize(World);
        pathFollowingSystem.Initialize(World);
        physicSyncSystem.Initialize(World);
        gridSnapSystem.Initialize(World);
        gridMapSystem.Initialize(World);
        buildingSystem.Initialize(World);

        World.Phases[World.EWorldPhase.Command].AddSystem(selectSystem);
        World.Phases[World.EWorldPhase.Command].AddSystem(buildingSystem);
        World.Phases[World.EWorldPhase.Presentation].AddSystem(gridSnapSystem);
        World.Phases[World.EWorldPhase.Presentation].AddSystem(highlightSystem);
        World.Phases[World.EWorldPhase.Command].AddSystem(movementCommandProcessingSystem);
        World.Phases[World.EWorldPhase.DataProcessing].AddSystem(gridMapSystem);
        World.Phases[World.EWorldPhase.EventProcessing].AddSystem(movementResolutionSystem);
        World.Phases[World.EWorldPhase.EventProcessing].AddSystem(pathFindingSystem);
        World.Phases[World.EWorldPhase.EventProcessing].AddSystem(pathFollowingSystem);
        World.Phases[World.EWorldPhase.Simulation].AddSystem(physicSyncSystem);

        // Health System
        HealthUISystem healthUISystem = new();
        healthUISystem.Initialize(World);
        World.Phases[World.EWorldPhase.Presentation].AddSystem(healthUISystem);

        // Camera System
        CameraSystem cameraSystem = new();
        cameraSystem.Initialize(World);
        World.Phases[World.EWorldPhase.Presentation].AddSystem(cameraSystem);
    }

    // Update is called once per frame
    public void Update() => World.Update(Time.deltaTime);
    public void FixedUpdate() => World.FixedUpdate(Time.fixedDeltaTime);
}
