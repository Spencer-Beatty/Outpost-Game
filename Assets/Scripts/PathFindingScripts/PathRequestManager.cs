using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    PathFinding pathfinding;

    bool isProccessingPath;
    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<PathFinding>();
    }
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[],bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProccessNext();
    }

    void TryProccessNext()
    {
        if (!isProccessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProccessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProccesingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProccessingPath = false;
        TryProccessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
