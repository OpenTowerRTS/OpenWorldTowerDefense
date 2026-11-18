using UnityEngine;

// This is the IComponent request that is attached to an entity
public struct PathFindingRequest : IComponent
{
    public Vector3 StartPosition;
    public Vector3 EndPosition;
    public RequestId requestId { get; set; }

    public PathFindingRequest(Vector3 start, Vector3 end, RequestId requestId)
    {
        StartPosition = start;
        EndPosition = end;
        this.requestId = requestId;
    }
}

// This is the PathQuery that is created inside the NavigationService
public struct PathQuery
{
    public Vector3 StartPosition;
    public Vector3 EndPosition;
    public RequestId requestId { get; set; }

    public PathQuery(Vector3 start, Vector3 end, RequestId requestId)
    {
        StartPosition = start;
        EndPosition = end;
        this.requestId = requestId;
    }
}

public enum PathRequestStatus
{
    Pending,
    Completed,
    Unreachable
}
