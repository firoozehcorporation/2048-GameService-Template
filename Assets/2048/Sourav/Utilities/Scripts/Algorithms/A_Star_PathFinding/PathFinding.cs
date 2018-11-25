using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Sourav.Utilities.Scripts.DataStructures;
using Sourav.Utilities.Extensions;
using System;

namespace Sourav.Utilities.Scripts.Algorithms.AStarPathfinding
{
    [RequireComponent(typeof(Grid), typeof(PathRequestManager))]
    public class PathFinding : MonoBehaviour
    {
        //public Transform Seeker;
        //public Transform Target;

        PathRequestManager RequestManager;

        private Grid grid;

        private void Awake()
        {
            grid = GetComponent<Grid>();
            RequestManager = GetComponent<PathRequestManager>();
        }

        public void StartFindPath(Vector3 StartPos, Vector3 TargetPos)
        {
            StartCoroutine(FindPath(StartPos, TargetPos));
        }

        IEnumerator FindPath(Vector3 startPos, Vector3 endPos)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Vector3[] wayPoints = new Vector3[0];
            bool pathSuccess = false;

            Node startNode = grid.GetNodeFromWorldPosition(startPos);
            Node endNode = grid.GetNodeFromWorldPosition(endPos);

            if(!endNode.Walkable)
            {
                Debug.LogError("End Node NOT Walkable");
                endNode = FindNearestNode(endNode);
                
            }

            if(startNode.Walkable && endNode.Walkable)
            {
                Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
                HashSet<Node> closedSet = new HashSet<Node>();
                openSet.Add(startNode);

                while(openSet.Count > 0)
                {
                    Node currentNode = openSet.RemoveFirstItem();
                    closedSet.Add(currentNode);

                    if(currentNode == endNode)
                    {
                        sw.Stop();
                        pathSuccess = true;
                        Debug.Log("Path found in = "+sw.ElapsedMilliseconds+" ms");
                    }

                    foreach(Node neighbour in grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.Walkable || closedSet.Contains(neighbour))
                            continue;

                        int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                        if(newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                        {
                            neighbour.GCost = newMovementCostToNeighbour;
                            neighbour.HCost = GetDistance(neighbour, endNode);
                            neighbour.parent = currentNode;

                            if(!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }
                            else
                            {
                                openSet.UpdateItem(neighbour);
                            }
                        }
                    }
                }
            }

            yield return null;
            if(pathSuccess)
            {
                wayPoints = RetracePath(startNode, endNode);
            }
            RequestManager.FinishedProcessingPath(wayPoints, pathSuccess);
        }

        public Node FindNearestNode(Node endNode)
        {
            List<Node> Nodes = grid.GetNeighbours(endNode);
            int i = 0;
            Node nearestNode = Nodes[i + 1]; ;
            while (!Nodes[i].Walkable)
            {
                if(i < Nodes.Count)
                {
                    nearestNode = Nodes[i+1]; 
                }
                foreach(Node n in grid.GetNeighbours(Nodes[i]))
                {
                    Nodes.Add(n);
                }
                i++;
            }

            return nearestNode;
        }

        Vector3[] RetracePath(Node startNode, Node endNode)
        {
            List<Node> Path = new List<Node>();
            Node currentNode = endNode;
            while(currentNode != startNode)
            {
                Path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            Vector3[] wayPoints = SimplifyPath(Path);
            wayPoints = wayPoints.Reverse();
            grid.Path = Path;
            return wayPoints;
        }

        Vector3[] SimplifyPath(List<Node> Path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;

            for (int i = 1; i < Path.Count; i++)
            {
                Vector2 directionNew = new Vector2(Path[i - 1].GridX - Path[i].GridX, Path[i - 1].GridY - Path[i].GridY);
                if(directionNew != directionOld)
                {
                    Debug.Log("Direction changed");
                    waypoints.Add(Path[i].WorldPoint);
                }
            }
            return waypoints.ToArray();
        }

        int GetDistance(Node NodeA, Node NodeB)
        {
            int distanceX = Mathf.Abs(NodeA.GridX - NodeB.GridX);
            int distanceY = Mathf.Abs(NodeA.GridY - NodeB.GridY);

            if(distanceX > distanceY)
            {
                return (14 * distanceY + 10 * (distanceX - distanceY));
            }
            else
            {
                return (14 * distanceX + 10 * (distanceY - distanceX));
            }
        }
    }
}
