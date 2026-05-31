using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class World
{

    private Dictionary<EntityID, GameObject> _entityObjects;
    private Dictionary<Type, IGameSystem> _systems;
    private Dictionary<EntityID, Dictionary<Type, IComponent>> _entityComponents; // Maps EntityID to a dictionary of component types and their instances
    public void Initialize()
    {
        _systems = new Dictionary<Type, IGameSystem>();
        _entityComponents = new Dictionary<EntityID, Dictionary<Type, IComponent>>();
        _entityObjects = new Dictionary<EntityID, GameObject>();
    }

    // Register an entity and return its EntityID. This can be used when you want to create an entity without a GameObject, such as for pure data entities.
    public EntityID RegisterEntity()
    {
        EntityID entityId = EntityIDGenerator.GenerateID(); // Generate a unique EntityID
        _entityComponents[entityId] = new Dictionary<Type, IComponent>(); // Initialize the component dictionary for this entity

        Debug.Log($"EntityView with EntityID {entityId} registered to the world.");
        return entityId;
    }

    // Register an entity and return its EntityID. This can be used when you want to create an entity with a GameObject, such as for entities that have a visual representation in the scene.
    public EntityID RegisterEntity(GameObject entityObject)
    {
        EntityID entityId = EntityIDGenerator.GenerateID(); // Generate a unique EntityID
        _entityComponents[entityId] = new Dictionary<Type, IComponent>(); // Initialize the component dictionary for this entity
        _entityObjects[entityId] = entityObject; // Store the GameObject for this entity

        Debug.Log($"EntityView with EntityID {entityId} registered to the world.");
        return entityId;
    }

    // Update is called once per frame
    public void Update(float deltaTime)
    {
        foreach (IGameSystem system in _systems.Values)
        {
            if (system is IUpdatableSystem updatable)
            {
                updatable.Update(deltaTime);
            }
        }
    }

    public void FixedUpdate(float fixedDeltaTime)
    {
        foreach (IGameSystem system in _systems.Values)
        {
            if (system is IFixedUpdatableSystem fixedUpdatable)
            {
                fixedUpdatable.FixedUpdate(fixedDeltaTime);
            }
        }
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

    // why not just use object component and get type later? You will need to cast later since now component is stored as object type.
    // Having T here make it easy to define the casting type at adding time
    public void AddComponentToEntity<T>(EntityID entityId, T component) where T : IComponent
    {
        if (!_entityComponents.ContainsKey(entityId))
        {
            _entityComponents[entityId] = new Dictionary<Type, IComponent>();
        }
        _entityComponents[entityId][typeof(T)] = component;
        Debug.Log($"Registering component from author: {typeof(T)} for EntityID: {entityId}");
        Debug.Log(_entityComponents[entityId][typeof(T)]);
    }

    // Try to get component, return false if entity does not exist or doesn't have the component.
    public bool GetComponentFromEntity<T>(EntityID entityId, out T component) where T : IComponent
    {
        component = default;
        Debug.Log(_entityComponents[entityId][typeof(T)]);
        if (_entityComponents.TryGetValue(entityId, out Dictionary<Type, IComponent> components) && components.TryGetValue(typeof(T), out IComponent comp))
        {
            component = (T)comp;
            return true;
        }
        return false;
    }
}
