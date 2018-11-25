using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimised_ArraySearchFunction
{
    public static int solution(int[] A)
    {
        CountingSort_Integer.Sort(A, -1000000, 1000000);
        A = CountingSort_Integer.newArray;
        int small = 1;

        for (int i = 0; i < A.Length; i++)
        {
            if(A[i] == small)
            {
                ++small;
            }
        }

        return small;
    }
}
