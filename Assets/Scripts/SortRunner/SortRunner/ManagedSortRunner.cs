using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ManagedSortRunner : BaseSortRunner
{
    protected override string RunUIntSort(SortSettings settings)
    {
        var sorter = SortSource.Instance.GetManagedUIntSorter(settings.name);
        var list = ManagedDataGenerator.GenrateRandomList(settings.Count, (uint)settings.Min, (uint)settings.Max);
        return GetResult(settings, 
            ()=> 
            { 
                ManagedDataGenerator.ShuffleList(list, 1f);
                ManagedDataGenerator.SortPartial(list, settings.Sorted);
            }, 
            ()=> 
            { 
                sorter(list); 
            }
            );
    }

    protected override string RunIntSort(SortSettings settings)
    {
        var sorter = SortSource.Instance.GetManagedIntSorter(settings.name);
        var list = ManagedDataGenerator.GenrateRandomList(settings.Count, settings.Min, settings.Max);
        return GetResult(settings,
            () =>
            {
                ManagedDataGenerator.ShuffleList(list, 1f);
                ManagedDataGenerator.SortPartial(list, settings.Sorted);
            },
            () =>
            {
                sorter(list);
            }
            );
    }

    protected override string RunFloatSort(SortSettings settings)
    {
        var sorter = SortSource.Instance.GetManagedFloatSorter(settings.name);
        var list = ManagedDataGenerator.GenrateRandomList(settings.Count, (float)settings.Min, (float)settings.Max);
        return GetResult(settings,
            () =>
            {
                ManagedDataGenerator.ShuffleList(list, 1f);
                ManagedDataGenerator.SortPartial(list, settings.Sorted);
            },
            () =>
            {
                sorter(list);
            }
            );
    }
}