using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Sourav.Utilities.Scripts.Algorithms.Shuffle;
using Sourav.Utilities.Scripts.Attributes;
using UnityEngine;

namespace Sourav.Test.Scripts
{
    public class ShuffleTest : MonoBehaviour 
    {
        public int N;
        public int StartFrom;
        
        // Use this for initialization
        void Start () 
        {
            TestShuffle();
        }
        
        [Button()]
        public void TestShuffle()
        {
            FisherYatesShuffle fisherYatesShuffle = new FisherYatesShuffle(N, StartFrom);
            PrintIntegerList(fisherYatesShuffle.ShuffledList);
            fisherYatesShuffle.ShuffleList();
            PrintIntegerList(fisherYatesShuffle.ShuffledList);
        }
        
        private void PrintIntegerList(List<int> list)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            for (int i = 0; i < list.Count; i++)
            {
                stringBuilder.Append(list[i]);
                if(i < list.Count - 1)
                {
                    stringBuilder.Append(" , ");
                }
            }
            
            Debug.Log(stringBuilder.ToString());
        }
    }
    
}