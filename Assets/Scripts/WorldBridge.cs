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
        selectSystem.Initialize();
        highlightSystem.Initialize();
        World.AddSystem(selectSystem);
        World.AddSystem(highlightSystem);
        selectSystem.eventBus.Subscribe<List<EntityID>>(highlightSystem.OnHighlightEntity);
    }
    // Initialize the World and Central Manager here

    // public void Start() {}

    // Update is called once per frame
    public void Update() => World.Update(Time.deltaTime);
}
