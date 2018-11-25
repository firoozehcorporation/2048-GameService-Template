public class CountingSort_Integer
{
    public static int[] newArray;

    public static void Sort(int[] A, int min, int max)
    {
        int[] count = new int[(max - min)];
        newArray = new int[A.Length];

        for (int i = 0; i < A.Length; i++)
        {
            count[A[i]]++;
        }

        int index = -1;
        for (int i = 0; i < count.Length; i++)
        {
            int counter = count[i];
            for (int j = 0; j < counter; j++)
            {
                newArray[++index] = i;
            }
        }
    }
}
