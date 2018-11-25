using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSort_Integer
{
    public static void QuickSort(int[] A, int left, int right)
    {
        if(left < right)
        {
            int pivot = Partition(A, left, right);

            if(pivot > 1)
            {
                QuickSort(A, left, pivot - 1);
            }

            if(pivot + 1 < right)
            {
                QuickSort(A, pivot + 1, right);
            }
        }
    }

    private static int Partition(int[] numbers, int left, int right)
    {
        int pivot = numbers[left];
        while(true)
        {
            while (numbers[left] < pivot)
                left++;
            while (numbers[right] > pivot)
                right--;

            if(left < right)
            {
                int temp = numbers[right];
                numbers[right] = numbers[left];
                numbers[left] = temp;
            }
            else
            {
                return right;
            }
        }
    }
}
