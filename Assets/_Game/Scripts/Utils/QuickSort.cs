using System;

public static class Quicksort
{
    public static void Sort<T>(T[] array) where T : IComparable
    {
        Sort(array, 0, array.Length - 1);
    }

    private static T[] Sort<T>(T[] array, int lower, int higher) where T : IComparable
    {
        if (lower < higher)
        {
            var p = Partition(array, lower, higher);
            Sort(array, lower, p);
            Sort(array, p + 1, higher);
        }

        return array;
    }

    private static int Partition<T>(T[] array, int lower, int higher) where T : IComparable
    {
        var i = lower;
        var j = higher;
        var pivot = array[lower];
        do
        {
            while (array[i].CompareTo(pivot) < 0) i++;
            while (array[j].CompareTo(pivot) > 0) j--;
            if (i >= j) break;
            Swap(array, i, j);
        } while (i <= j);

        return j;
    }

    private static void Swap<T>(T[] array, int first, int second)
    {
        var temp = array[first];
        array[first] = array[second];
        array[second] = temp;
    }
}