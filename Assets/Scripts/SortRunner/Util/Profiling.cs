using UnityEngine.Profiling;
using System.Collections.Generic;

public static class Profiling
{
    static Dictionary<string, long> samples = new Dictionary<string, long>();

    public static double MemoryProfile(System.Action method)
    {
        long startMemory = Profiler.GetMonoUsedSizeLong();
        method();
        return (Profiler.GetMonoUsedSizeLong() - startMemory) / (1024.0);
    }

    public static void BeginMemorySample(string name = "")
    {
        if (samples.ContainsKey(name))
            samples[name] = Profiler.GetMonoUsedSizeLong();
        else
            samples.Add(name, Profiler.GetMonoUsedSizeLong());
    }

    /// <summary>
    /// used memory in kb
    /// </summary>
    public static double EndMemorySample(string name = "")
    {
        var lastrecord = samples[name];
        samples.Remove(name);
        return (Profiler.GetMonoUsedSizeLong() - lastrecord) / (1024.0);
    }

    public static float ExecutingProfile(System.Action method)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        method();
        watch.Stop();
        return (watch.ElapsedTicks / 10000f);
    }
}