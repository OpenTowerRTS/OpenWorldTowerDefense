using System.Collections.Generic;
using UnityEngine;

public interface IPathFinder
{
    public void submitPathQuery(PathQuery request);
    public PathRequestStatus TryGetPathResult(RequestId requestId, out Vector3[] pathResult);
    public void cancelPathQuery(RequestId requestId);
}

