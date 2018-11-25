using Sourav.Utilities.Scripts.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortTest : MonoBehaviour
{
    public int[] IntegerArray;

    [Button()]
    public void Sort()
    {
        CountingSort_Integer.Sort(IntegerArray, -1000000, 1000000);
        IntegerArray = CountingSort_Integer.newArray;
    }
}
