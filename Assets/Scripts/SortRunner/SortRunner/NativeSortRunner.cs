using UnityEngine;
using System.Collections;
using Unity.Collections;
using System;

public class NativeSortRunner : BaseSortRunner
{
    protected override string RunUIntSort(SortSettings settings)
    {
        var sorter = SortSource.Instance.GetNativeUIntSorter(settings.name);
        var array = NativeDataGenerator.GenrateRandomArray(settings.Count, (uint)settings.Min, (uint)settings.Max);
        var result = GetResult(settings,
            () =>
            {
                NativeDataGenerator.ShuffleList(array, 1f);
                NativeDataGenerator.SortPartial(array, settings.Sorted);
            },
            () =>
            {
                sorter(array);
            }
            );
        array.Dispose();
        return result;
    }

    protected override string RunIntSort(SortSettings settings)
    {
        var sorter = SortSource.Instance.GetNativeIntSorter(settings.name);
        var array = NativeDataGenerator.GenrateRandomArray(settings.Count, settings.Min, settings.Max);
        var result = GetResult(settings,
            () =>
            {
                NativeDataGenerator.ShuffleList(array, 1f);
                NativeDataGenerator.SortPartial(array, settings.Sorted);
            },
            () =>
            {
                sorter(array);
            }
            );
        array.Dispose();
        return result;
    }

    protected override string RunFloatSort(SortSettings settings)
    {
        var sorter = SortSource.Instance.GetNativeFloatSorter(settings.name);
        var array = NativeDataGenerator.GenrateRandomArray(settings.Count, (float)settings.Min, (float)settings.Max);
        var result = GetResult(settings,
            () =>
            {
                NativeDataGenerator.ShuffleList(array, 1f);
                NativeDataGenerator.SortPartial(array, settings.Sorted);
            },
            () =>
            {
                sorter(array);
            }
            );
        array.Dispose();
        return result;
    }
}