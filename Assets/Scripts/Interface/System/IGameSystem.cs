// Marker interface for game systems. Systems contain logic that operates on entities with specific components.
// Systems that implement this interface will be initialized and shut down by a Central Manager.
public interface IGameSystem
{
    public void Initialize();
    public void Shutdown();
}

// System that needs to be updated every frame should implement this interface. The Update method will be called with the time elapsed since the last frame.
public interface IUpdatableSystem
{
    public void Update(float deltaTime);
}

// System that needs to be updated at fixed intervals (e.g., for physics updates) should implement this interface. The FixedUpdate method will be called with the fixed time step.
public interface IFixedUpdatableSystem
{
    public void FixedUpdate(float fixedDeltaTime);
}
