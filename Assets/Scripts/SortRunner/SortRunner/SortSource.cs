using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using Unity.Collections;

public class SortSource
{
    private static SortSource instance;

    public static SortSource Instance
    {
        get
        {
            if (instance == null)
                instance = new SortSource();
            return instance;
        }
    }

    Dictionary<Type,Dictionary<string, MethodInfo>> managedMethod = new Dictionary<Type, Dictionary<string, MethodInfo>>();
    Dictionary<Type, Dictionary<string, MethodInfo>> nativeMethod = new Dictionary<Type, Dictionary<string, MethodInfo>>();

    Type[] supportedTypes = new Type[] { typeof(uint), typeof(int), typeof(float) };

    private SortSource()
    {
        GenrateMethodDictionary(typeof(ManagedSort.Sort), managedMethod);
        GenrateMethodDictionary(typeof(NativeSort.Sort), nativeMethod);
    }

    public List<string> GetSupportedTypes()
    {
        List<string> types = new List<string>();
        for (int i = 0; i < supportedTypes.Length; i++)
        {
            types.Add(supportedTypes[i].Name);
        }
        return types;
    }

    public Action<List<int>> GetManagedIntSorter(string name)
    {
        return (Action<List<int>>)(managedMethod[typeof(int)][name].CreateDelegate(typeof(Action<List<int>>)));
    }

    public Action<List<uint>> GetManagedUIntSorter(string name)
    {
        return (Action<List<uint>>)(managedMethod[typeof(uint)][name].CreateDelegate(typeof(Action<List<uint>>)));
    }

    public Action<List<float>> GetManagedFloatSorter(string name)
    {
        return (Action<List<float>>)(managedMethod[typeof(float)][name].CreateDelegate(typeof(Action<NativeArray<float>>)));
    }

    public Action<NativeArray<int>> GetNativeIntSorter(string name)
    {
        return (Action<NativeArray<int>>)(nativeMethod[typeof(int)][name].CreateDelegate(typeof(Action<NativeArray<int>>)));
    }

    public Action<NativeArray<uint>> GetNativeUIntSorter(string name)
    {
        return (Action<NativeArray<uint>>)(nativeMethod[typeof(uint)][name].CreateDelegate(typeof(Action<NativeArray<uint>>)));
    }

    public Action<NativeArray<float>> GetNativeFloatSorter(string name)
    {
        return (Action<NativeArray<float>>)(nativeMethod[typeof(float)][name].CreateDelegate(typeof(Action<NativeArray<float>>)));
    }

    public List<string> GetMethods(SortType sortType = SortType.Managed)
    {
        List<string> methods = new List<string>();
        Dictionary<string,MethodInfo> dic = null;

        switch(sortType){
            case SortType.Managed:
                dic = managedMethod[supportedTypes[0]];
                break;
            case SortType.Native:
                dic = nativeMethod[supportedTypes[0]];
                break;
        }

        foreach (var pair in dic)
        {
            methods.Add(pair.Key);
        }

        return methods;
    }

    private void GenrateMethodDictionary(Type methodClass, Dictionary<Type, Dictionary<string, MethodInfo>> methods)
    {
        for (int i = 0; i < supportedTypes.Length; i++)
            methods.Add(supportedTypes[i], new Dictionary<string, MethodInfo>());

        var sortMethods = GetMethodsWithCustomAttribute<SortAttribute>(methodClass.GetMethods());

        for (int i = 0; i < sortMethods.Count; i++)
        {
            var methodInfo = sortMethods[i];
            if (methodInfo.IsGenericMethod)
                AddGenericMethod(methodInfo, methods);
            else
                AddNormalMethod(methodInfo, methods);
        }
    }

    private void AddGenericMethod(MethodInfo methodInfo, Dictionary<Type, Dictionary<string, MethodInfo>> methods)
    {
        for (int i = 0; i < supportedTypes.Length; i++)
        {
            var method = methodInfo.MakeGenericMethod(supportedTypes[i]);
            methods[supportedTypes[i]].Add(method.Name, method);
        }
    }

    private void AddNormalMethod(MethodInfo methodInfo, Dictionary<Type, Dictionary<string, MethodInfo>> methods)
    {
        var genericParameter = methodInfo.GetParameters()[0].ParameterType.GetGenericArguments()[0];
        if (methods.ContainsKey(genericParameter))
        {
            methods[genericParameter].Add(methodInfo.Name, methodInfo);
        }
    }

    private List<MethodInfo> GetMethodsWithCustomAttribute<T>(MethodInfo[] methodInfos) where T : System.Attribute
    {
        List<MethodInfo> result = new List<MethodInfo>(methodInfos.Length);

        for (int i = 0; i < methodInfos.Length; i++)
        {
            if (methodInfos[i].GetCustomAttribute<T>() != null)
                result.Add(methodInfos[i]);
        }

        return result;
    }
}
