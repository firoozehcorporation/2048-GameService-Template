using Sourav.Utilities.Scripts.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sourav.Utilities.Scripts.Algorithms.AStarPathfinding
{
    public class Grid : MonoBehaviour
    {
        public bool ShowHelperGizmos;
        public bool OnlyDrawPath;
        public Mode Mode;
        public bool AllowDiagonalMovement;

        public LayerMask UnwalkableLayer;
        public Vector2 GridWorldSize;
        public float NodeRadius;
        public float GapBetweenNodes;

        private Node[,] grid;
        public Node[,] _Grid
        {
            get { return grid; }
        }
        float nodeDiameter;
        [SerializeField]
        private Vector2Int gridSize;
        public Vector2Int GridSize
        {
            get { return gridSize; }
        }


        public int MaxSize
        {
            get { return gridSize.x * gridSize.y; }
        }

        void Awake()
        {
            nodeDiameter = NodeRadius * 2;
            gridSize.x = Mathf.Abs(Mathf.RoundToInt(GridWorldSize.x / nodeDiameter));
            gridSize.y = Mathf.Abs(Mathf.RoundToInt(GridWorldSize.y / nodeDiameter));
            CreateGrid();
        }

        private void CreateGrid()
        {
            grid = new Node[gridSize.x, gridSize.y];
            Vector3 worldBottomLeft = Vector3.zero;
            if (Mode == Mode.ThreeDimensonal)
            {
                worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y / 2;
            }
            else
            {
                worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.up * GridWorldSize.y / 2;
            }

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Vector3 worldPoint = Vector3.zero;
                    bool walkable = true;

                    if(Mode == Mode.ThreeDimensonal)
                    {
                        worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + NodeRadius) + Vector3.forward * (y * nodeDiameter + NodeRadius);
                        //Debug.Log("worldPoint = "+worldPoint);
                        walkable = !(Physics.CheckSphere(worldPoint, NodeRadius, UnwalkableLayer));
                    }
                    else
                    {
                        worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + NodeRadius) + Vector3.up * (y * nodeDiameter + NodeRadius);
                        //Debug.Log("worldPoint = "+worldPoint);
                        //walkable = !(Physics.CheckSphere(worldPoint, NodeRadius, UnwalkableLayer));
                        walkable = !(Physics2D.OverlapCircle(new Vector2(worldPoint.x, worldPoint.y), NodeRadius, UnwalkableLayer));
                    }
                    grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
            Debug.Log("Grid size = "+grid.Length);
        }

        public Node GetNodeFromWorldPosition(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + GridWorldSize.x / 2) / GridWorldSize.x;
            float percentY = 0.0f;
            if(Mode == Mode.ThreeDimensonal)
            {
                percentY = (worldPosition.z + GridWorldSize.y / 2) / GridWorldSize.y;
            }
            else
            {
                percentY = (worldPosition.y + GridWorldSize.y / 2) / GridWorldSize.y;
            }

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((gridSize.x - 1) * percentX);
            int y = Mathf.RoundToInt((gridSize.y - 1) * percentY);
            return grid[x, y];
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbourNodes = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    if(!AllowDiagonalMovement)
                    {
                        if (x == -1 && y == 1 || x == 1 && y == 1 || x == -1 && y == -1 || x == 1 && y == -1)
                            continue;
                    }

                    int checkX = node.GridX + x;
                    int checkY = node.GridY + y;

                    if(checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
                    {
                        neighbourNodes.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbourNodes;
        }

        public List<Node> Path;
        public List<Node> Target;
        private void OnDrawGizmos()
        {
            if (Target != null)
            {
                Gizmos.color = Color.red;
                foreach (Node n in grid)
                {
                    if (Target.Contains(n))
                    {
                        //Debug.Log("Target found");
                        Debug.Log("x = "+n.GridX.ToString()+" , "+n.GridY.ToString());
                        Gizmos.DrawCube(n.WorldPoint, Vector3.one * (nodeDiameter - 0.01f));
                    }
                }
            }

            if (!ShowHelperGizmos)
                return;

            Gizmos.color = Color.black;
            if(Mode == Mode.ThreeDimensonal)
            {
                Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));
            }
            else
            {
                Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, GridWorldSize.y, 1));
            }

            if(OnlyDrawPath)
            {
                if(Path != null)
                {
                    Debug.Log("Path count = "+Path.Count);
                    foreach(Node n in Path)
                    {
                        Gizmos.color = Color.black;
                        Gizmos.DrawCube(n.WorldPoint, Vector3.one * (nodeDiameter - .1f));
                    }
                }
            }
            else
            {
                if(grid != null)
                {
                    foreach(Node n in grid)
                    {
                        if(Target != null)
                        {
                            if(Target.Contains(n))
                            {
                                continue;
                            }
                        }

                        Gizmos.color = (n.Walkable) ? Color.white : Color.red;
                        if(Path != null)
                        {
                            if(Path.Contains(n))
                            {
                                Gizmos.color = Color.black;
                            }
                        }
                        Gizmos.DrawCube(n.WorldPoint, Vector3.one * (nodeDiameter - .1f));
                    }
                }
            }
        }
    }

    public enum Mode
    {
        TwoDimensional,
        ThreeDimensonal
    }
}
