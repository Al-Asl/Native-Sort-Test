using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IL2CPPWorkaround : MonoBehaviour
{
    void Start()
    {
        ManagedSort.Sort.SelectionSort<int>(null);
        ManagedSort.Sort.SelectionSort<float>(null);
        ManagedSort.Sort.SelectionSort<uint>(null);
        ManagedSort.Sort.InsertionSort<int>(null);
        ManagedSort.Sort.InsertionSort<float>(null);
        ManagedSort.Sort.InsertionSort<uint>(null);
        ManagedSort.Sort.BubbleSort<int>(null);
        ManagedSort.Sort.BubbleSort<float>(null);
        ManagedSort.Sort.BubbleSort<uint>(null);
        ManagedSort.Sort.HeapSort<int>(null);
        ManagedSort.Sort.HeapSort<float>(null);
        ManagedSort.Sort.HeapSort<uint>(null);
        ManagedSort.Sort.CombSort<int>(null);
        ManagedSort.Sort.CombSort<float>(null);
        ManagedSort.Sort.CombSort<uint>(null);
        ManagedSort.Sort.ShellSort<int>(null);
        ManagedSort.Sort.ShellSort<float>(null);
        ManagedSort.Sort.ShellSort<uint>(null);
        ManagedSort.Sort.QuickSort<int>(null);
        ManagedSort.Sort.QuickSort<float>(null);
        ManagedSort.Sort.QuickSort<uint>(null);
        ManagedSort.Sort.MergeSort<int>(null);
        ManagedSort.Sort.MergeSort<float>(null);
        ManagedSort.Sort.MergeSort<uint>(null);
        ManagedSort.Sort.MergeInsertionSort<int>(null);
        ManagedSort.Sort.MergeInsertionSort<float>(null);
        ManagedSort.Sort.MergeInsertionSort<uint>(null);

        NativeSort.Sort.SelectionSort<int>(default);
        NativeSort.Sort.SelectionSort<float>(default);
        NativeSort.Sort.SelectionSort<uint>(default);
        NativeSort.Sort.InsertionSort<int>(default);
        NativeSort.Sort.InsertionSort<float>(default);
        NativeSort.Sort.InsertionSort<uint>(default);
        NativeSort.Sort.BubbleSort<int>(default);
        NativeSort.Sort.BubbleSort<float>(default);
        NativeSort.Sort.BubbleSort<uint>(default);
        NativeSort.Sort.CombSort<int>(default);
        NativeSort.Sort.CombSort<float>(default);
        NativeSort.Sort.CombSort<uint>(default);
        NativeSort.Sort.ShellSort<int>(default);
        NativeSort.Sort.ShellSort<float>(default);
        NativeSort.Sort.ShellSort<uint>(default);
        NativeSort.Sort.HeapSort<int>(default);
        NativeSort.Sort.HeapSort<float>(default);
        NativeSort.Sort.HeapSort<uint>(default);
        NativeSort.Sort.MergeSort<int>(default);
        NativeSort.Sort.MergeSort<float>(default);
        NativeSort.Sort.MergeSort<uint>(default);
        NativeSort.Sort.MergeInsertionSort<int>(default);
        NativeSort.Sort.MergeInsertionSort<float>(default);
        NativeSort.Sort.MergeInsertionSort<uint>(default);
        NativeSort.Sort.QuickSort<int>(default);
        NativeSort.Sort.QuickSort<float>(default);
        NativeSort.Sort.QuickSort<uint>(default);
    }
}
