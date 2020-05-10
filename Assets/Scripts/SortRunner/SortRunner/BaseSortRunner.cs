using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class BaseSortRunner
{
    public string Run(SortSettings settings)
    {
        switch (settings.elementType)
        {
            case "UInt32":
                return RunUIntSort(settings);
            case "Int32":
                return RunIntSort(settings);
            case "Single":
                return RunFloatSort(settings);
        }
        return "type not supported!!";
    }

    protected abstract string RunUIntSort(SortSettings settings);

    protected abstract string RunIntSort(SortSettings settings);

    protected abstract string RunFloatSort(SortSettings settings);

    public static string GetResult(SortSettings settings, Action randomizeList ,Action sorting)
    {
        string result = "";

        var res = settings.MemoryDebug ? RunSortWithMemoryDebug(settings, randomizeList , sorting) : RunSort(settings, randomizeList , sorting);

        result += string.Format("> average run time for {0} after {1} test is {2} ms " +
                "for {3} element of type {4} with sorted percentage {5}/100 with numbers ranges" +
                "from {6} to {7} \n"
                , settings.name, settings.TestCount, res.Item1, settings.Count,
                settings.elementType, settings.Sorted, settings.Min, settings.Max);

        if (settings.MemoryDebug)
        {
            result += string.Format("> the average memory cost is {0} kb", res.Item2);
        }

        return result;
    }

    static (float, double) RunSort(SortSettings settings, Action randomizeList, Action sorting)
    {
        float timeMs = 0;

        for (int i = 0; i < settings.TestCount; i++)
        {
            randomizeList();
            timeMs += Profiling.ExecutingProfile(sorting);
        }

        return (timeMs / settings.TestCount, 0);
    }

    static (float, double) RunSortWithMemoryDebug(SortSettings settings, Action randomizeList, Action sorting)
    {
        float timeMs = 0;
        double memoryKb = 0;

        for (int i = 0; i < settings.TestCount; i++)
        {
            randomizeList();
            memoryKb += Profiling.MemoryProfile(() =>
            {
                timeMs += Profiling.ExecutingProfile(sorting);
            });
        }

        return (timeMs / settings.TestCount, memoryKb / settings.TestCount);
    }
}