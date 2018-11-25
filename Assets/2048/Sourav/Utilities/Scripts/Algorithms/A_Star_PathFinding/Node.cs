using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sourav.Utilities.Scripts.DataStructures;

namespace Sourav.Utilities.Scripts.Algorithms.AStarPathfinding
{
    public class Node : IHeapItem<Node>
    {
        public bool Walkable;
        public int GridX;
        public int GridY;
        public Vector3 WorldPoint;
        public Node parent;

        public int HCost;
        public int GCost;

        public int FCost
        {
            get { return GCost + HCost; }
        }

        private int heapIndex;
        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }

            set
            {
                heapIndex = value;
            }
        }

        public Node(bool walkable, Vector3 worldPoint, int x, int y)
        {
            Walkable = walkable;
            this.WorldPoint = worldPoint;
            this.GridX = x;
            this.GridY = y;
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = FCost.CompareTo(nodeToCompare.FCost);
            if(compare == 0)
            {
                compare = HCost.CompareTo(nodeToCompare.HCost);
            }
            return -compare;
        }
    }
}
