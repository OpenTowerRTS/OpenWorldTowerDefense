using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class DefaultPathFinder : MonoBehaviour, IPathFinder
{
    private readonly GameObject agentPrefab;

    private readonly int agentCount = 4;

    private Dictionary<RequestId, PathQuery> pendingPathQueries;
    private Queue<PathQuery> pathQueryQueue;
    private Queue<NavMeshAgent> availableAgents;
    private Dictionary<RequestId, NavMeshAgent> workingAgents;
    private Dictionary<RequestId, Vector3[]> pathResults;
    private void Start()
    {
        pendingPathQueries = new Dictionary<RequestId, PathQuery>();
        pathQueryQueue = new Queue<PathQuery>();

        availableAgents = new Queue<NavMeshAgent>();
        workingAgents = new Dictionary<RequestId, NavMeshAgent>();
        pathResults = new Dictionary<RequestId, Vector3[]>();

        for (int i = 0; i < agentCount; i++)
        {
            GameObject agentObject = Instantiate(agentPrefab, Vector3.zero, Quaternion.identity, transform);
            NavMeshAgent agent = agentObject.GetComponent<NavMeshAgent>();
            availableAgents.Enqueue(agent);
        }
    }

    public void Update()
    {
        // Check if agents are done
        List<RequestId> completedRequests = new();
        foreach ((RequestId requestId, NavMeshAgent agent) in workingAgents)
        {
            if (agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                pathResults[requestId] = (Vector3[])agent.path.corners.Clone();
            }
            // For simplicity if the target is not reachable, we will just return a null path
            // The PathFindingSystem reading a null path will understand that it is not reachable
            // TODO: This is also a kind of impl
            else if (agent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                Debug.Log("Path result: PathPartial");
                pathResults[requestId] = null;
            }
            availableAgents.Enqueue(agent);
            completedRequests.Add(requestId);
        }

        // Remove completed request from workingAgents and pendingPathQueries
        foreach (RequestId requestId in completedRequests)
        {
            pendingPathQueries.Remove(requestId);
            workingAgents.Remove(requestId);
        }

        // Assign new path queries to available agents
        while (pathQueryQueue.Count > 0 && availableAgents.Count > 0)
        {
            PathQuery query = pathQueryQueue.Dequeue();
            // ignore already cancelled request
            if (!pendingPathQueries.TryGetValue(query.requestId, out _))
            {
                continue;
            }
            NavMeshAgent agent = availableAgents.Dequeue();
            agent.Warp(new Vector3(query.StartPosition.x, query.StartPosition.y, 0));
            agent.SetDestination(new Vector3(query.EndPosition.x, query.EndPosition.y, 0));
            workingAgents[query.requestId] = agent;
        }
    }

    public void submitPathQuery(PathQuery request)
    {
        pendingPathQueries[request.requestId] = request;
        pathQueryQueue.Enqueue(request);
    }

    // TODO: how to invalidate a Path when it is staled? For example, the World map change with new obstacle
    // And the the worker is following a Path that doesn't work any more.
    // => Need a MapSystem that keep track of these change and annouce an invalidate event
    public PathRequestStatus TryGetPathResult(RequestId requestId, out Vector3[] pathResult)
    {
        bool return_result = pathResults.TryGetValue(requestId, out pathResult);
        Debug.Log($"Path result: {(pathResult == null ? "NULL" : string.Join(", ", pathResult))}");
        Debug.Log($"Path result: return result {return_result}");
        if (return_result)
        {
            pathResults.Remove(requestId);
            if (pathResult != null)
            {
                return PathRequestStatus.Completed;
            }
            else
            {
                return PathRequestStatus.Unreachable;
            }
        }
        else
        {
            return PathRequestStatus.Pending;
        }

    }

    public void cancelPathQuery(RequestId requestId)
    {
        // Remove cancelRequest from pendingPathQueries
        pendingPathQueries.Remove(requestId);
        // Force making the agent available if it is working on this request
        workingAgents.Remove(requestId);
        // Remove the result as well if any
        pathResults.Remove(requestId);
    }
}
