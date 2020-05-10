using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

public class CustomSortSource 
{
    private static CustomSortSource instance;

    public static CustomSortSource Instance
    {
        get
        {
            if (instance == null)
                instance = new CustomSortSource();
            return instance;
        }
    }

    Dictionary<string, System.Func<SortSettings,string>> methods = new Dictionary<string, System.Func<SortSettings, string>>();

    private CustomSortSource()
    {
        CustomSortRepo customSortRepo = new CustomSortRepo();
        customSortRepo.AddMethods(methods);
    }

    public System.Func<SortSettings, string> GetSort(string name)
    {
        return methods[name];
    }

    public List<string> GetMethods()
    {
        var methods = new List<string>();
        foreach(var pair in this.methods)
        {
            methods.Add(pair.Key);
        }
        return methods;
    }


}
