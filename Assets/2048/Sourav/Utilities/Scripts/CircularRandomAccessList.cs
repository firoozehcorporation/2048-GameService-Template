using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sourav.Utilities.Scripts
{
    public class CircularRandomAccessList<T>
    {
        private static List<T> circularList;
        private int accessPoint;

        private int lastElement;
        private int nextElement;
        private int currentElement;

        public CircularRandomAccessList() : this(0, new List<T>())
        {}

        public CircularRandomAccessList(int accessPoint, List<T> list)
        {
            circularList = list;
            this.accessPoint = accessPoint < circularList.Count? accessPoint : -1;
            lastElement = GetCorrectIndex(accessPoint - 1);
            currentElement = lastElement;
            nextElement = -1;
        }

        public T GetNextIndex()
        {
            nextElement = GetCorrectIndex(currentElement + 1);
            currentElement = nextElement;
            return circularList[currentElement];
        }

        private int GetCorrectIndex(int inputIndex)
        {
            if(inputIndex >= circularList.Count)
            {
                return 0;
            }
            else if(inputIndex < 0)
            {
                return circularList.Count - 1;
            }
            else
            {
                return inputIndex;
            }
        }
    }
    
}