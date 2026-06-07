using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class World
{

    private interface IComponentPool
    {
        public void Remove(EntityID entity);
        public bool Has(EntityID entity);
    }

    private class ComponentPool<T> : IComponentPool
    {
        private readonly Dictionary<EntityID, T> _components = new();
        public void Add(EntityID entity, T component) => _components[entity] = component;

        public void Remove(EntityID entity) => _components.Remove(entity);

        public bool Has(EntityID entity) => _components.ContainsKey(entity);

        public bool TryGet(EntityID entity, out T component) => _components.TryGetValue(entity, out component);

        public Dictionary<EntityID, T>.Enumerator GetEnumerator() => _components.GetEnumerator();

        public int Count => _components.Count;
    }
    // Maps component types to a dictionary of EntityIDs and their component instances.
    // This is a more efficient structure for querying entities by component type, which is a common operation in ECS.
    private Dictionary<Type, IComponentPool> _entityComponentPool;

    // Still need reference to gameObject for now since we need to manipulate the GameObject in presentation system.
    private Dictionary<EntityID, GameObject> _entityObjects;

    // Event and Command State
    public EventBus EventBus { get; private set; }
    public CommandBuffer Commands { get; private set; }
    public EventBuffer Events { get; private set; }

    // Entity State
    public List<EntityID> selectedEntities;
    public List<EntityID> highlightEntities;

    // System State
    public Dictionary<EWorldPhase, WorldPhase> Phases { get; private set; }
    private Dictionary<Type, IGameSystem> _systems;

    // Versioning and Synchronization
    private long _worldEventBufferVersionUpdate;
    private long _worldEventBufferVersionFixedUpdate;


    public enum EWorldPhase
    {
        Command,
        Simulation,
        EventProcessing,
        Presentation
    }

    public void Initialize()
    {
        // Initialize Event and Command Managers
        EventBus = new EventBus();
        Commands = new CommandBuffer();
        Events = new EventBuffer();

        // Initialize Entity State
        selectedEntities = new List<EntityID>();
        highlightEntities = new List<EntityID>();

        // Initialize Entity Data
        _entityComponentPool = new Dictionary<Type, IComponentPool>();
        _entityObjects = new Dictionary<EntityID, GameObject>();

        // Initialize component pools for each component type. This allows us to efficiently manage components of different types.
        foreach (Type componentType in ComponentRegistry.Types)
        {
            //Take the generic class ComponentPool<T> and replace T with a runtime type. AKA this is create ComponentPool<componentType>
            Type poolType = typeof(ComponentPool<>).MakeGenericType(componentType);

            //Instantiate an object from a Type that you only know at runtime.
            _entityComponentPool[componentType] = (IComponentPool)Activator.CreateInstance(poolType);
        }

        // Initialize Systems
        _systems = new Dictionary<Type, IGameSystem>();
        Phases = new Dictionary<EWorldPhase, WorldPhase>
        {
            [EWorldPhase.Command] = new WorldPhase(this),
            [EWorldPhase.Simulation] = new WorldPhase(this),
            [EWorldPhase.EventProcessing] = new WorldPhase(this),
            [EWorldPhase.Presentation] = new WorldPhase(this)
        };

        // Initialize versioning
        // Start at -1 so that it always process the first batch of events and commands.
        _worldEventBufferVersionUpdate = -1;
        _worldEventBufferVersionFixedUpdate = -1;
    }

    // Register an entity and return its EntityID. This can be used when you want to create an entity without a GameObject, such as for pure data entities.
    public EntityID RegisterEntity()
    {
        EntityID entityId = EntityIDGenerator.GenerateID(); // Generate a unique EntityID

        Debug.Log($"EntityView with EntityID {entityId} registered to the world.");
        return entityId;
    }

    // Register an entity and return its EntityID. This can be used when you want to create an entity with a GameObject, such as for entities that have a visual representation in the scene.
    public EntityID RegisterEntity(GameObject entityObject)
    {
        EntityID entityId = EntityIDGenerator.GenerateID(); // Generate a unique EntityID
        _entityObjects[entityId] = entityObject; // Store the GameObject for this entity

        Debug.Log($"EntityView with EntityID {entityId} registered to the world.");
        return entityId;
    }

    // Update is called once per frame
    public void Update(float deltaTime)
    {
        // This solution has a problem though, we are essentially binding Update to run at the same rate as FixedUpdate
        // We might need to go a step further and consider a PerSystem versioning if we want to allow different systems to run at different rates, but that might be an overkill for our current need.
        // Only process events if there are new events in the buffer that haven't been processed in the update phase yet.
        // Debug.Log($"World Update checked");
        if (_worldEventBufferVersionUpdate == Events.Version)
        {
            if (_worldEventBufferVersionUpdate == _worldEventBufferVersionFixedUpdate)
            {
                Events.SwapBuffers();
            }
            return;
        }
        // Debug.Log("World Update started");

        // Currently only Presentation Systems need to be called in update.
        // The input System is handled by Unity's so it is also considered to be an "Update type" System.
        Phases[EWorldPhase.Presentation].Update(deltaTime);

        _worldEventBufferVersionUpdate = Events.Version;

        // For clean lifecycle ownership, Update clock is incharge of updating EventBuffer.
        // However, it should only do so if FixedUpdate has already processed the events,
        //  otherwise we might end up in a situation where FixedUpdated is supposed to process events
        // but Update has already swapped the buffer and cleared the events before FixedUpdate can process them.

    }

    public void FixedUpdate(float fixedDeltaTime)
    {
        // Debug.Log($"World FixedUpdate checked");
        if (_worldEventBufferVersionFixedUpdate >= Events.Version)
        {
            // Process events and commands for the fixed update phase
            return;
        }

        // Debug.Log("World FixedUpdate started");

        // Currently only Simulation Systems need to be called in fixed update.
        Phases[EWorldPhase.Command].FixedUpdate(fixedDeltaTime);
        Phases[EWorldPhase.EventProcessing].FixedUpdate(fixedDeltaTime);
        Phases[EWorldPhase.Simulation].FixedUpdate(fixedDeltaTime);

        _worldEventBufferVersionFixedUpdate = Events.Version;

        // For clean lifecycle ownership, FixedUpdate is incharge of updating CommandsBuffer.
        // We don't need to consider about the version of CommandBuffer
        // since only FixedUpdate Consume the event here, so we can safely swap the buffer and clear the commands without worrying about synchronization with Update.
        Commands.SwapBuffers();
    }

    // Retrieve the GameObject associated with an EntityID, return null if not found.
    public bool GetEntityObject(EntityID entityId, out GameObject gameObject)
    {
        if (_entityObjects.TryGetValue(entityId, out GameObject entityObject))
        {
            gameObject = entityObject;
            return true;
        }
        gameObject = null;
        return false;
    }

    // IGameSystem must be a class since they need to implement behaviour, this allow null by default.
    public void AddSystem<T>(T system) where T : class, IGameSystem => _systems[typeof(T)] = system;
    public T GetSystem<T>() where T : class, IGameSystem => _systems.TryGetValue(typeof(T), out IGameSystem system) ? system as T : null;

    // why not just use object as the component (AKA object component instead of T component) and get type later? You will need to cast later since now component is stored as object type.
    // Having T here make it easy to define the casting type at adding time
    public void AddComponentToEntity<T>(EntityID entityId, T component) where T : IComponent
    {
        // We make sure to create an entry for each entity in RegisterEntity, so we can assume the entityId is always valid and has an entry in _entityComponents.
        ((ComponentPool<T>)_entityComponentPool[typeof(T)]).Add(entityId, component);
        Debug.Log($"Add component to World: {typeof(T)} for EntityID: {entityId}");
    }

    public void RemoveComponentFromEntity<T>(EntityID entityId) where T : IComponent
    {
        ((ComponentPool<T>)_entityComponentPool[typeof(T)]).Remove(entityId);
        Debug.Log($"Remove component from World: {typeof(T)} for EntityID: {entityId}");
    }

    // Try to get component, return false if entity does not exist or doesn't have the component.
    public bool GetComponentFromEntity<T>(EntityID entityId, out T component) where T : IComponent
    {
        component = default;
        if (((ComponentPool<T>)_entityComponentPool[typeof(T)]).TryGet(entityId, out component))
        {
            return true;
        }
        return false;
    }
}
