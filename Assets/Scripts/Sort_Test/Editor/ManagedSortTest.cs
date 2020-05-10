using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using ManagedSort;

public class ManagedSortTest 
{
    private static float[][] TestSource = new float[][]
    {
        new float[] {10,-10,10},
        new float[] {100,-100,100},
        new float[] {1000,-1000,1000},
    };

    [Test]
    [TestCaseSource("TestSource")]
    public void selection_sort_test(float count, float min, float max)
    {
        var list = ManagedDataGenerator.GenrateRandomList((int)count, min, max);
        SortTest(list, "Selection sort ", Sort.SelectionSort);
    }

    [Test]
    [TestCaseSource("TestSource")]
    public void insertion_sort_test(float count, float min, float max)
    {
        var list = ManagedDataGenerator.GenrateRandomList((int)count, min, max);
        SortTest(list, "Insertion sort ", Sort.InsertionSort);
    }

    [Test]
    [TestCaseSource("TestSource")]
    public void bubble_sort_test(float count, float min, float max)
    {
        var list = ManagedDataGenerator.GenrateRandomList((int)count, min, max);
        SortTest(list, "Bubble sort ", Sort.BubbleSort);
    }

    [Test]
    [TestCaseSource("TestSource")]
    public void comb_sort_test(float count, float min, float max)
    {
        var list = ManagedDataGenerator.GenrateRandomList((int)count, min, max);
        SortTest(list, "Comb sort ", Sort.CombSort);
    }

    [Test]
    [TestCaseSource("TestSource")]
    public void shell_sort_test(float count, float min, float max)
    {
        var list = ManagedDataGenerator.GenrateRandomList((int)count, min, max);
        SortTest(list, "Shell sort ", Sort.ShellSort);
    }

    [Test]
    [TestCaseSource("TestSource")]
    public void merge_sort_test(float count, float min, float max)
    {
        var list = ManagedDataGenerator.GenrateRandomList((int)count, min, max);
        SortTest(list, "Merge sort ", Sort.MergeSort);
    }

    [Test]
    [TestCaseSource("TestSource")]
    public void quick_sort_test(float count, float min, float max)
    {
        var list = ManagedDataGenerator.GenrateRandomList((int)count, min, max);
        SortTest(list, "MergeInsertion sort ", Sort.QuickSort);
    }

    [Test]
    [TestCaseSource("TestSource")]
    public void mergeInsertion_sort_test(float count, float min, float max)
    {
        var list = ManagedDataGenerator.GenrateRandomList((int)count, min, max);
        SortTest(list, "Quick sort ", Sort.MergeInsertionSort);
    }

    [Test]
    [TestCaseSource("TestSource")]
    public void heap_sort_test(float count, float min, float max)
    {
        var list = ManagedDataGenerator.GenrateRandomList((int)count, min, max);
        SortTest(list, "Heap sort ", Sort.HeapSort);
    }

    [Test]
    [TestCaseSource("TestSource")]
    public void radixLSB_sort_test(float count, float min, float max)
    {
        var list = ManagedDataGenerator.GenrateRandomList((int)count, min, max);
        SortTest(list, "RadixLSB sort ", Sort.RadixLSBSort);
    }

    [Test]
    [TestCaseSource("TestSource")]
    public void radixMSD_sort_test(float count, float min, float max)
    {
        var list = ManagedDataGenerator.GenrateRandomList((int)count, min, max);
        SortTest(list, "RadixMSD sort ", Sort.RadixMSDSort);
    }

    [Test]
    [TestCaseSource("TestSource")]
    public void inPlaceRadix_sort_test(float count, float min, float max)
    {
        var list = ManagedDataGenerator.GenrateRandomList((int)count, min, max);
        SortTest(list, "InPlaceRadix sort ", Sort.InPlaceRadixSort);
    }

    #region Util

    private void SortTest<T>(List<T> list, string message, System.Action<List<T>> method) where T : System.IEquatable<T>
    {
        var copy = new List<T>(list);
        copy.Sort();
        method(list);
        Assert.That(ListCompare(copy, list ));
    }

    private bool ListCompare<T>(List<T> a, List<T> b, bool log = false) where T : System.IEquatable<T>
    {
        bool pass = true;

        for (int i = 0; i < a.Count; i++)
        {
            if (log)
                Debug.Log(string.Format("compare {0} to {1}", a[i], b[i]));
            if (!a[i].Equals(b[i]))
            {
                pass = false;
            }
                
        }

        return pass;
    }

    #endregion
}