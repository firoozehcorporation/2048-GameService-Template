using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sourav.Utilities.Scripts.Algorithms.AStarPathfinding
{
    [RequireComponent(typeof(PathFinding))]
    public class PathRequestManager : MonoBehaviour
    {
        Queue<PathRequest> PathRequestQueue;
        PathRequest CurrentPathRequest;

        PathFinding pathfinding;
        bool isProcessingPath;

        public static PathRequestManager Instance;

        void Awake()
        {
            Instance = this;
            PathRequestQueue = new Queue<PathRequest>();
            pathfinding = GetComponent<PathFinding>();
        }

        public void RequestPath(Vector3 PathStart, Vector3 PathEnd, Action<Vector3[], bool> CallBack)
        {
            PathRequest pathRequest = new PathRequest(PathStart, PathEnd, CallBack);
            PathRequestQueue.Enqueue(pathRequest);
            TryProcessNext();
        }

        private void TryProcessNext()
        {
            if(!isProcessingPath && PathRequestQueue.Count > 0)
            {
                CurrentPathRequest = PathRequestQueue.Dequeue();
                isProcessingPath = true;
                pathfinding.StartFindPath(CurrentPathRequest.PathStart, CurrentPathRequest.PathEnd);
            }
            else
            {
                if (PathRequestQueue.Count == 0)
                    Debug.Log("PathRequestQueue.Count = 0");
            }
        }

        public void FinishedProcessingPath(Vector3[]Path, bool Success)
        {
            CurrentPathRequest.Callback(Path, Success);
            isProcessingPath = false;
            TryProcessNext();
        }
    }

    [Serializable]
    public struct PathRequest
    {
        public Vector3 PathStart;
        public Vector3 PathEnd;
        public Action<Vector3[], bool> Callback;

        public PathRequest(Vector3 _pathStart, Vector3 _pathEnd, Action<Vector3[], bool> _callBack)
        {
            PathStart = _pathStart;
            PathEnd = _pathEnd;
            Callback = _callBack;
        }
    }
}

