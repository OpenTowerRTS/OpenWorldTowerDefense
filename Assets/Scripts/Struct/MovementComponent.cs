public struct MovementComponent : IComponent
{
    public float MaxSpeed { get; set; } // Maximum speed the entity can move at
    public float CurrSpeed { get; set; } // Current speed of the entity, can

    public MovementComponent(float maxSpeed)
    {
        MaxSpeed = maxSpeed;
        CurrSpeed = 0f; // Initialize current speed to 0
    }
}
