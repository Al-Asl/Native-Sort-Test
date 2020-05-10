using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EnumeratorRunner
{
    public static int MaxTaskPerTick = 8;

    private static List<IEnumerator> tasks = new List<IEnumerator>();

    public static void AddTask(IEnumerator enumerator)
    {
        tasks.Add(enumerator);
    }

    private static void RemoveTask(IEnumerator enumerator)
    {
        tasks.Remove(enumerator);
    }

    public static void Run()
    {
        while (true)
        {
            for (int i = 0; i < Mathf.Min(MaxTaskPerTick, tasks.Count); i++)
            {
                if (!tasks[i].MoveNext()) RemoveTask(tasks[i]);
            }
            if (tasks.Count == 0) break;
        }

    }
}
