using System.Collections.Generic;
using UnityEngine;

namespace Sourav.Utilities.Scripts.Algorithms.Shuffle
{
    public class FisherYatesShuffle
    {
        public List<int> ShuffledList;
        public int NumberOfItems;

        public FisherYatesShuffle(int n)
            : this(n, 0)
        {
        }

        public FisherYatesShuffle(int n, int start)
        {
            NumberOfItems = n;
            ShuffledList = new List<int>();
            
            //Initializing List
            for (int i = start; i < start + n; i++)
            {
                ShuffledList.Add(i);
            }
        }

        public void ShuffleList()
        {
            int lastElement = NumberOfItems;
            for (int i = 0; i < NumberOfItems - 1; i++)
            {
                int randomNumber = (int)Random.Range(0, lastElement);
                Swap(randomNumber, lastElement - 1);
                lastElement--;
            }
        }

        private void Swap(int from, int to)
        {
            if (ShuffledList.Count - 1 < to || from < 0 || from == to)
                return;
            
            ShuffledList[from] += ShuffledList[to];
            ShuffledList[to] = ShuffledList[from] - ShuffledList[to];
            ShuffledList[from] = ShuffledList[from] - ShuffledList[to];
        }
    }
}