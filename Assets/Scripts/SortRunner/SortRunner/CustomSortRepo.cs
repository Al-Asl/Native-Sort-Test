using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using Unity.Jobs;

public class CustomSortRepo
{
    public void AddMethods(Dictionary<string, System.Func<SortSettings, string>> dictionary)
    {
        dictionary.Add("ListSort", ListSort);
        dictionary.Add("SelectionUIntArray", SelectionUIntArray);
        dictionary.Add("SelectionUIntArrayWithJob", SelectionUIntArrayWithJob);
        dictionary.Add("SelectionSortGenericArray", SelectionSortGenericArray);
        dictionary.Add("SelectionUnsafePointer", SelectionUnsafePointer);
        dictionary.Add("SelectionUnsafePointerWithJob", SelectionUnsafePointerWithJob);
        dictionary.Add("SelectionUIntSlice", SelectionUIntSlice);
        dictionary.Add("SelectionUIntSliceWithJob", SelectionUIntSliceWithJob);
        dictionary.Add("SelectionGenericArrayWithJob", SelectionGenericArrayWithJob);
        dictionary.Add("SelectionUIntArrayWithJobNative", SelectionUIntArrayWithJobNative);
        dictionary.Add("NativeSliceSortIgnoreSliceCost", NativeSliceSortIgnoreSliceCost);
    }

    string ListSort(SortSettings settings)
    {
        var list = ManagedDataGenerator.GenrateRandomList(settings.Count, settings.Min, settings.Max);
        ManagedDataGenerator.SortPartial(list, settings.Sorted);
        return BaseSortRunner.GetResult(settings,
            ()=>
            {
                ManagedDataGenerator.ShuffleList(list, 1f);
                ManagedDataGenerator.SortPartial(list, settings.Sorted);
            },
            () => 
            {
                list.Sort(); 
            }
            );
    }

    //check if instantiating NativeSlice have performance cost 
    string NativeSliceSortIgnoreSliceCost(SortSettings settings)
    {
        var array = NativeDataGenerator.GenrateRandomArray(settings.Count, (uint)settings.Min, (uint)settings.Max);
        NativeDataGenerator.SortPartial(array, settings.Sorted);
        var slice = new NativeSlice<uint>(array);
        var res = BaseSortRunner.GetResult(settings,
             () =>
             {
                 NativeDataGenerator.ShuffleList(array, 1f);
                 NativeDataGenerator.SortPartial(array, settings.Sorted);
             },
             () =>
             {
                 SelectionSortSlice(array);
             }
             );
        array.Dispose();
        return res;
    }

    static string SelectionUIntArray(SortSettings settings)
    {
        var array = NativeDataGenerator.GenrateRandomArray(settings.Count, (uint)settings.Min, (uint)settings.Max);
        NativeDataGenerator.SortPartial(array, settings.Sorted);
        var res = BaseSortRunner.GetResult(settings,
             () =>
             {
                 NativeDataGenerator.ShuffleList(array, 1f);
                 NativeDataGenerator.SortPartial(array, settings.Sorted);
             },
             () =>
             {
                 SelectionUIntArray(array);
             }
             );
        array.Dispose();
        return res;
    }

    static void SelectionUIntArray(NativeArray<uint> array)
    {
        for (int i = 1; i < array.Length; i++)
        {
            for (int j = i - 1; j >= 0; j--)
            {
                if (array[j + 1].CompareTo(array[j]) < 0)
                {
                    uint temp = array[j + 1];
                    array[j + 1] = array[j];
                    array[j] = temp;
                }
                else
                    continue;
            }
        }
    }

    static string SelectionUIntArrayWithJob(SortSettings settings)
    {
        var array = NativeDataGenerator.GenrateRandomArray(settings.Count, (uint)settings.Min, (uint)settings.Max);
        NativeDataGenerator.SortPartial(array, settings.Sorted);
        var res = BaseSortRunner.GetResult(settings,
             () =>
             {
                 NativeDataGenerator.ShuffleList(array, 1f);
                 NativeDataGenerator.SortPartial(array, settings.Sorted);
             },
             () =>
             {
                 SelectionUIntArrayWithJob(array);
             }
             );
        array.Dispose();
        return res;
    }

    static string SelectionUIntArrayWithJobNative(SortSettings settings)
    {
        var array = NativeDataGenerator.GenrateRandomArray(settings.Count, (uint)settings.Min, (uint)settings.Max);
        NativeDataGenerator.SortPartial(array, settings.Sorted);
        var res = BaseSortRunner.GetResult(settings,
             () =>
             {
                 NativeDataGenerator.ShuffleList(array, 1f);
                 NativeDataGenerator.SortPartial(array, settings.Sorted);
             },
             () =>
             {
                 NativeSort.Sort.SelectionSort(array);
             }
             );
        array.Dispose();
        return res;
    }

    static void SelectionUIntArrayWithJob(NativeArray<uint> array)
    {
        var sortJob = new SelectionSortJob()
        {
            array = array
        };
        sortJob.Schedule().Complete();
    }

    [BurstCompile]
    struct SelectionSortJob : IJob
    {
        public NativeArray<uint> array;

        public void Execute()
        {
            for (int i = 1; i < array.Length; i++)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (array[j + 1].CompareTo(array[j]) < 0)
                    {
                        uint temp = array[j + 1];
                        array[j + 1] = array[j];
                        array[j] = temp;
                    }
                    else
                        continue;
                }
            }
        }
    }

    static string SelectionSortGenericArray(SortSettings settings)
    {
        var array = NativeDataGenerator.GenrateRandomArray(settings.Count, (uint)settings.Min, (uint)settings.Max);
        NativeDataGenerator.SortPartial(array, settings.Sorted);
        var res = BaseSortRunner.GetResult(settings,
              () =>
              {
                  NativeDataGenerator.ShuffleList(array, 1f);
                  NativeDataGenerator.SortPartial(array, settings.Sorted);
              },
              () =>
              {
                  SelectionGenericArray(array);
              }
              );
        array.Dispose();
        return res;
    }

    public static void SelectionGenericArray<T>(NativeArray<T> array) where T : struct, System.IComparable, System.IComparable<T>
    {
        for (int i = 1; i < array.Length; i++)
        {
            for (int j = i - 1; j >= 0; j--)
            {
                if (array[j + 1].CompareTo(array[j]) < 0)
                    Swap(array, j + 1, j);
                else
                    continue;
            }
        }
    }

    [BurstCompile]
    unsafe struct SelectionGenericSortJob<T> : IJob
        where T : unmanaged , System.IComparable<T>
    {
        [NativeDisableUnsafePtrRestriction]
        public T* array;
        public int Length;

        public void Execute()
        {
            for (int i =  1; i < Length; i++)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (!SwapIfGreater(array, j, j + 1))
                        continue;
                }
            }
        }
    }

    private unsafe static void Swap<T>(T* array, int i, int j) where T : unmanaged
    {
        T temp = UnsafeUtility.ReadArrayElement<T>(array, i);
        UnsafeUtility.WriteArrayElement(array, i, UnsafeUtility.ReadArrayElement<T>(array, j));
        UnsafeUtility.WriteArrayElement(array, j, temp);
    }

    private unsafe static bool SwapIfGreater<T>(T* array, int i, int j) where T : unmanaged, System.IComparable<T>
    {
        if (Compare(array, i, j) > 0)
        {
            Swap(array, i, j);
            return true;
        }
        return false;

    }

    private unsafe static int Compare<T>(T* array, int i, int j) where T : unmanaged, System.IComparable<T>
    {
        return UnsafeUtility.ReadArrayElement<T>(array, i).CompareTo(UnsafeUtility.ReadArrayElement<T>(array, j));
    }

    static string SelectionGenericArrayWithJob(SortSettings settings)
    {
        var array = NativeDataGenerator.GenrateRandomArray(settings.Count, (uint)settings.Min, (uint)settings.Max);
        NativeDataGenerator.SortPartial(array, settings.Sorted);
        var res = BaseSortRunner.GetResult(settings,
             () =>
             {
                 NativeDataGenerator.ShuffleList(array, 1f);
                 NativeDataGenerator.SortPartial(array, settings.Sorted);
             },
             () =>
             {
                 SelectionGenericArrayWithJob(array);
             }
             );
        array.Dispose();
        return res;
    }

    public unsafe static void SelectionGenericArrayWithJob<T>(NativeArray<T> array) where T : unmanaged ,System.IComparable<T>
    {
        var sortJob = new SelectionGenericSortJob<T>()
        {
            array = (T*)array.GetUnsafePtr(),
            Length = array.Length
        };
        sortJob.Schedule().Complete();
    }

    static string SelectionUnsafePointer(SortSettings settings)
    {
        var array = NativeDataGenerator.GenrateRandomArray(settings.Count, (uint)settings.Min, (uint)settings.Max);
        NativeDataGenerator.SortPartial(array, settings.Sorted);
        var res = BaseSortRunner.GetResult(settings,
             () =>
             {
                 NativeDataGenerator.ShuffleList(array, 1f);
                 NativeDataGenerator.SortPartial(array, settings.Sorted);
             },
             () =>
             {
                 SelectionUnsafePointer(array);
             }
             );
        array.Dispose();
        return res;
    }

    static unsafe void SelectionUnsafePointer(NativeArray<uint> array)
    {
        var arrayPointer = array.GetUnsafePtr();
        var arrayLength = array.Length;

        for (int i = 1; i < arrayLength; i++)
        {

            for (int j = i - 1; j >= 0; j--)
            {
                var jData = UnsafeUtility.ReadArrayElement<uint>(arrayPointer, j);
                var jPlusData = UnsafeUtility.ReadArrayElement<uint>(arrayPointer, j + 1);
                if (jPlusData < jData)
                {
                    uint temp = jPlusData;
                    UnsafeUtility.WriteArrayElement(arrayPointer, j + 1, jData);
                    UnsafeUtility.WriteArrayElement(arrayPointer, j, temp);
                }
                else
                    continue;
            }
        }
    }

    static string SelectionUnsafePointerWithJob(SortSettings settings)
    {
        var array = NativeDataGenerator.GenrateRandomArray(settings.Count, (uint)settings.Min, (uint)settings.Max);
        NativeDataGenerator.SortPartial(array, settings.Sorted);
        var res = BaseSortRunner.GetResult(settings,
             () =>
             {
                 NativeDataGenerator.ShuffleList(array, 1f);
                 NativeDataGenerator.SortPartial(array, settings.Sorted);
             },
             () =>
             {
                 SelectionUnsafePointerWithJob(array);
             }
             );
        array.Dispose();
        return res;
    }

    unsafe static void SelectionUnsafePointerWithJob(NativeArray<uint> array)
    {
        var sortJob = new SelectionSortUnsafePointerJob()
        {
            arrayPointer = array.GetUnsafePtr(),
            arrayLength = array.Length
        };
        sortJob.Schedule().Complete();
    }

    [BurstCompile]
    unsafe struct SelectionSortUnsafePointerJob : IJob
    {
        [NativeDisableUnsafePtrRestriction]
        public void* arrayPointer;
        public int arrayLength;

        public void Execute()
        {
            for (int i = 1; i < arrayLength; i++)
            {

                for (int j = i - 1; j >= 0; j--)
                {
                    var jData = UnsafeUtility.ReadArrayElement<uint>(arrayPointer, j);
                    var jPlusData = UnsafeUtility.ReadArrayElement<uint>(arrayPointer, j + 1);
                    if (jPlusData < jData)
                    {
                        uint temp = jPlusData;
                        UnsafeUtility.WriteArrayElement(arrayPointer, j + 1, jData);
                        UnsafeUtility.WriteArrayElement(arrayPointer, j, temp);
                    }
                    else
                        continue;
                }
            }
        }
    }

    static string SelectionUIntSlice(SortSettings settings)
    {
        var array = NativeDataGenerator.GenrateRandomArray(settings.Count, (uint)settings.Min, (uint)settings.Max);
        NativeDataGenerator.SortPartial(array, settings.Sorted);
        var res = BaseSortRunner.GetResult(settings,
             () =>
             {
                 NativeDataGenerator.ShuffleList(array, 1f);
                 NativeDataGenerator.SortPartial(array, settings.Sorted);
             },
             () =>
             {
                 SelectionUIntSlice(array);
             }
             );
        array.Dispose();
        return res;
    }

    static void SelectionUIntSlice(NativeArray<uint> array)
    {
        SelectionSortSlice(new NativeSlice<uint>(array, 0, array.Length));
    }

    static void SelectionSortSlice(NativeSlice<uint> array)
    {
        for (int i = 1; i < array.Length; i++)
        {
            for (int j = i - 1; j >= 0; j--)
            {
                if (array[j + 1].CompareTo(array[j]) < 0)
                {
                    uint temp = array[j + 1];
                    array[j + 1] = array[j];
                    array[j] = temp;
                }
                else
                    continue;
            }
        }
    }

    static string SelectionUIntSliceWithJob(SortSettings settings)
    {
        var array = NativeDataGenerator.GenrateRandomArray(settings.Count, (uint)settings.Min, (uint)settings.Max);
        NativeDataGenerator.SortPartial(array, settings.Sorted);
        var res = BaseSortRunner.GetResult(settings,
             () =>
             {
                 NativeDataGenerator.ShuffleList(array, 1f);
                 NativeDataGenerator.SortPartial(array, settings.Sorted);
             },
             () =>
             {
                 SelectionUIntSliceWithJob(array);
             }
             );
        array.Dispose();
        return res;
    }

    static void SelectionUIntSliceWithJob(NativeArray<uint> array)
    {
        var sortJob = new SelectionSortSliceJob()
        {
            array = new NativeSlice<uint>(array, 0, array.Length)
        };
        sortJob.Schedule().Complete();
    }

    [BurstCompile]
    struct SelectionSortSliceJob : IJob
    {
        public NativeSlice<uint> array;

        public void Execute()
        {
            for (int i = 1; i < array.Length; i++)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (array[j + 1].CompareTo(array[j]) < 0)
                    {
                        uint temp = array[j + 1];
                        array[j + 1] = array[j];
                        array[j] = temp;
                    }
                    else
                        continue;
                }
            }
        }
    }

    private static void Swap<T>(NativeArray<T> list, int i, int j) where T : struct
    {
        T temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
}
