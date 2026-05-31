public interface IComponentAuthor
{
    // Method to create a new component instance. The returned object should be cast to the specific component type.
    public void RegisterToWorld(World world, EntityID entityId);
}
