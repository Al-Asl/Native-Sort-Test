using UnityEngine.Profiling;
using System.Collections;

public static class MemoryProfiling 
{
    private static long globalMemoryUsage;

    public static void ClearGlobalSample()
    {
        globalMemoryUsage = 0;
    }

    public static void LogGlobalSample()
    {
        UnityEngine.Debug.Log("global usage in kb : " + globalMemoryUsage);
    }

    public static void MemoryProfile(System.Action method)
    {
        long startMemory = Profiler.GetMonoUsedSizeLong();
        method();
        long usedMemory = (Profiler.GetMonoUsedSizeLong() - startMemory) / (1024);
        UnityEngine.Debug.Log("Memory usage in kb : " + usedMemory);
    }

    private static long lastMemoryusage;
    public static void BeginSample()
    {
        lastMemoryusage = Profiler.GetMonoUsedSizeLong();
    }

    public static void EndSample()
    {
        long usedMemory = (Profiler.GetMonoUsedSizeLong() - lastMemoryusage) / (1024);
        globalMemoryUsage += usedMemory;
        UnityEngine.Debug.Log("Memory usage in kb : " + usedMemory);
    }

    private static long lastTotalMemoryUSage;
    public static void BeginTotalSample()
    {
        lastTotalMemoryUSage = Profiler.GetTotalAllocatedMemoryLong();
    }
    public static void EndTotalSample()
    {
        long usedMemory = (Profiler.GetTotalAllocatedMemoryLong() - lastTotalMemoryUSage) / (1024);
        UnityEngine.Debug.Log("Total Memory usage in kb : " + usedMemory);
    }
}
