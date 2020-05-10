using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.Collections;

public static class ManagedDataGenerator 
{
    public static void SortPartial<T>(List<T> list, float percentage) where T : IComparable<T>, IComparable
    {
        if (percentage == 0) return;
        list.Sort();
        if (percentage == 1) return;
        int swaps = Mathf.FloorToInt(percentage * list.Count / 2);
        ShuffleList(list, percentage);
    }

    public static void ShuffleList <T>(List<T> list , float percentage) where T : IComparable<T>, IComparable
    {
        int swaps = Mathf.FloorToInt(percentage * list.Count / 2);
        for (int i = 0; i < swaps; i++)
        {
            int a = UnityEngine.Random.Range(0, list.Count);
            int b = UnityEngine.Random.Range(0, list.Count);
            T temp = list[a];
            list[a] = list[b];
            list[b] = temp;
        }
    }

    public static List<int> GenrateRandomList(int count, int min, int max)
    {
        var list = new List<int>(count);
        for (int i = 0; i < count; i++)
        {
            list.Add(UnityEngine.Random.Range(min, max));
        }
        return list;
    }

    public static List<float> GenrateRandomList(int count, float min, float max)
    {
        var list = new List<float>(count);
        for (int i = 0; i < count; i++)
        {
            list.Add(UnityEngine.Random.Range(min, max));
        }
        return list;
    }

    public static List<uint> GenrateRandomList(int count, uint min, uint max)
    {
        var list = new List<uint>(count);
        for (int i = 0; i < count; i++)
        {
            list.Add((uint)UnityEngine.Random.Range((int)min, (int)max));
        }
        return list;
    }
}


public static class NativeDataGenerator
{
    public static void SortPartial<T>(NativeArray<T> array, float percentage) where T : unmanaged, IComparable<T>, IComparable
    {
        if (percentage == 0) return;
        array.Sort<T>();
        if (percentage == 1) return;
        ShuffleList(array, percentage);
    }

    public static void ShuffleList<T>(NativeArray<T> array, float percentage) where T : unmanaged , IComparable<T>, IComparable
    {
        int swaps = Mathf.FloorToInt(percentage * array.Length / 2);
        for (int i = 0; i < swaps; i++)
        {
            int a = UnityEngine.Random.Range(0, array.Length);
            int b = UnityEngine.Random.Range(0, array.Length);
            T temp = array[a];
            array[a] = array[b];
            array[b] = temp;
        }
    }

    public static NativeArray<int> GenrateRandomArray(int count, int min, int max)
    {
        var array = new NativeArray<int>(count, Allocator.Persistent);
        for (int i = 0; i < count; i++)
        {
            array[i] = UnityEngine.Random.Range(min, max);
        }
        return array;
    }

    public static NativeArray<uint> GenrateRandomArray(int count, uint min, uint max)
    {
        var array = new NativeArray<uint>(count, Allocator.TempJob);
        for (int i = 0; i < count; i++)
        {
            array[i] = (uint)UnityEngine.Random.Range((int)min, (int)max);
        }
        return array;
    }

    public static NativeArray<float> GenrateRandomArray(int count, float min, float max)
    {
        var array = new NativeArray<float>(count, Allocator.Persistent);
        for (int i = 0; i < count; i++)
        {
            array[i] = UnityEngine.Random.Range(min, max);
        }
        return array;
    }
}