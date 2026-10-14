public readonly struct RequestId
{
    public readonly int Id { get; }

    public RequestId(int id) => Id = id;

    public override string ToString() => Id.ToString();
}

// Entity ID generator
public static class RequestIdGenerator
{
    private static int _currentId = 0;

    public static RequestId GenerateID() => new(_currentId++);
}

