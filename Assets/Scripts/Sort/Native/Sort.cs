using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;

namespace NativeSort
{
    public static class Sort {

        private static void BurstWorkAround()
        {
            var selectionInt = new SelectionSortJob<int>();
            selectionInt.end = 0;
            var selectionUInt = new SelectionSortJob<uint>();
            selectionUInt.end = 0;
            var selectionFloat = new SelectionSortJob<float>();
            selectionFloat.end = 0;
            var insertionInt = new InsertionSortJob<int>();
            insertionInt.end = 0;
            var insertionUInt = new InsertionSortJob<uint>();
            insertionUInt.end = 0;
            var insertionFloat = new InsertionSortJob<float>();
            insertionFloat.end = 0;
            var bubbleInt = new BubbleSortJob<int>();
            bubbleInt.end = 0;
            var bubbleUInt = new BubbleSortJob<uint>();
            bubbleUInt.end = 0;
            var bubbleFloat = new BubbleSortJob<float>();
            bubbleFloat.end = 0;
            var combInt = new BubbleSortJob<int>();
            combInt.end = 0;
            var combUInt = new BubbleSortJob<uint>();
            combUInt.end = 0;
            var combFloat = new BubbleSortJob<float>();
            combFloat.end = 0;
            var shellInt = new ShellSortJob<int>();
            shellInt.end = 0;
            var shellUInt = new ShellSortJob<uint>();
            shellUInt.end = 0;
            var shellFloat = new ShellSortJob<float>();
            shellFloat.end = 0;
            var HeapInt = new HeapSortJob<int>();
            HeapInt.end = 0;
            var HeapUInt = new HeapSortJob<uint>();
            HeapUInt.end = 0;
            var HeapFloat = new HeapSortJob<float>();
            HeapFloat.end = 0;
            var MergeInt = new MergeSortJob<int>();
            MergeInt.end = 0;
            var MergeUInt = new MergeSortJob<uint>();
            MergeUInt.end = 0;
            var MergeFloat = new MergeSortJob<float>();
            MergeFloat.end = 0;
            var MergeInsertionInt = new MergeInsertionSortJob<int>();
            MergeInsertionInt.end = 0;
            var MergeInsertionUInt = new MergeInsertionSortJob<uint>();
            MergeInsertionUInt.end = 0;
            var MergeInsertionFloat = new MergeInsertionSortJob<float>();
            MergeInsertionFloat.end = 0;
            var QuickInt = new QuickSortJob<int>();
            QuickInt.end = 0;
            var QuickUInt = new QuickSortJob<uint>();
            QuickUInt.end = 0;
            var QuickFloat = new QuickSortJob<float>();
            QuickFloat.end = 0;
        }

        #region SelectionSort

        [BurstCompile]
        unsafe struct SelectionSortJob<T> : IJob
            where T : unmanaged, System.IComparable<T>, System.IComparable
        {
            [NativeDisableUnsafePtrRestriction]
            public T* array;
            public int start;
            public int end;

            public void Execute()
            {
                for (int i = start + 1; i < end; i++)
                {
                    for (int j = i - 1; j >= start; j--)
                    {
                        if (!SwapIfGreater(array, j, j + 1))
                            continue;
                    }
                }
            }
        }

        [Sort]
        public unsafe static void SelectionSort<T>(NativeArray<T> array) where T : unmanaged, System.IComparable, System.IComparable<T>
        {
            var sortJob = new SelectionSortJob<T>()
            {
                array = (T*)array.GetUnsafePtr(),
                start = 0,
                end = array.Length
            };
            sortJob.Schedule().Complete();
        }

        #endregion

        #region Insertion

        [BurstCompile]
        unsafe struct InsertionSortJob<T> : IJob
            where T : unmanaged, System.IComparable<T>, System.IComparable
        {
            [NativeDisableUnsafePtrRestriction]
            public T* array;
            public int start;
            public int end;

            public void Execute()
            {
                InsertionSort(array, start, end);
            }
        }

        private unsafe static void InsertionSort<T>(T* array , int start , int end)
            where T : unmanaged , System.IComparable<T> 
        {
            for (int i = start; i < end; i++)
            {
                T temp = UnsafeUtility.ReadArrayElement<T>(array, i);
                int j = i - 1;
                for (; j >= start; j--)
                {
                    if (temp.CompareTo(UnsafeUtility.ReadArrayElement<T>(array, j)) < 0)
                        UnsafeUtility.WriteArrayElement(array, j + 1, UnsafeUtility.ReadArrayElement<T>(array, j));
                    else
                        break;
                }
                UnsafeUtility.WriteArrayElement(array, j + 1, temp);
            }
        }

        [Sort]
        public unsafe static void InsertionSort<T>(NativeArray<T> array) where T : unmanaged , System.IComparable, System.IComparable<T>
        {
            var sortJob = new InsertionSortJob<T>()
            {
                array = (T*)array.GetUnsafePtr(),
                start = 0,
                end = array.Length
            };
            sortJob.Schedule().Complete();
        }

        #endregion

        #region BubbbleSort

        [BurstCompile]
        unsafe struct BubbleSortJob<T> : IJob
        where T : unmanaged, System.IComparable<T>, System.IComparable
        {
            [NativeDisableUnsafePtrRestriction]
            public T* array;
            public int start;
            public int end;

            public void Execute()
            {
                for (int i = end; i >= start + 1; i--)
                {
                    for (int j = start + 1; j < i; j++)
                    {
                        SwapIfGreater(array ,j - 1, j);
                    }
                }
            }
        }

        [Sort]
        public unsafe static void BubbleSort<T>(NativeArray<T> array) where T : unmanaged, System.IComparable, System.IComparable<T>
        {
            var sortJob = new BubbleSortJob<T>()
            {
                array = (T*)array.GetUnsafePtr(),
                start = 0,
                end = array.Length
            };
            sortJob.Schedule().Complete();
        }

        #endregion

        #region CombSort

        [BurstCompile]
        unsafe struct CombSortJob<T> : IJob
        where T : unmanaged, System.IComparable<T>, System.IComparable
        {
            [NativeDisableUnsafePtrRestriction]
            public T* array;
            public int start;
            public int end;

            public void Execute()
            {
                int length = end - start;
                int gap = length;
                bool sorted = false;

                while (!sorted)
                {
                    if (gap < 1)
                    {
                        sorted = true;
                        gap = 1;
                    }

                    for (int i = start; i < length - gap; i++)
                    {
                        if (SwapIfGreater(array, i, i + gap))
                            sorted = false;
                    }
                    gap = (int)(gap / 1.3f);
                }
            }
        }

        [Sort]
        public unsafe static void CombSort<T>(NativeArray<T> array) where T : unmanaged, System.IComparable, System.IComparable<T>
        {
            var sortJob = new CombSortJob<T>()
            {
                array = (T*)array.GetUnsafePtr(),
                start = 0,
                end = array.Length
            };
            sortJob.Schedule().Complete();
        }

        #endregion

        #region ShellSort

        [BurstCompile]
        unsafe struct ShellSortJob<T> : IJob
        where T : unmanaged, System.IComparable<T>, System.IComparable
        {
            [NativeDisableUnsafePtrRestriction]
            public T* array;
            public int start;
            public int end;
            int length;

            public void Execute()
            {
                length = end - start;
                ShellSort(length / 2);
            }

            private void ShellSort (int step)
            {
                if (step > 0)
                {
                    for (int i = 0; i < length - step; i++)
                    {
                        SwapIfGreater(array, i, i + step);
                    }
                    ShellSort(step / 2);
                }
            }
        }

        [Sort]
        public unsafe static void ShellSort<T>(NativeArray<T> array) where T : unmanaged, System.IComparable, System.IComparable<T>
        {
            var sortJob = new ShellSortJob<T>()
            {
                array = (T*)array.GetUnsafePtr(),
                start = 0,
                end = array.Length
            };
            sortJob.Schedule().Complete();
            InsertionSort(array);
        }

        #endregion

        #region HeapSort

        [BurstCompile]
        unsafe struct HeapSortJob<T> : IJob
        where T : unmanaged, System.IComparable<T>, System.IComparable
        {
            [NativeDisableUnsafePtrRestriction]
            public T* array;
            public int start;
            public int end;
            int Length;

            public void Execute()
            {
                Length = end - start;

                for (int i = Length / 2 - 1; i >= start; i--)
                    heapify(end,i);

                for (int i = end - 1; i >= start; i--)
                {
                    Swap(array,start, i);

                    heapify(i, start);
                }
            }

            void heapify(int end, int i) 
            {
                int largest = i;
                int l = 2 * i + 1;
                int r = 2 * i + 2;

                if (l < end && Compare(array,l,largest) > 0)
                    largest = l;

                if (r < end && Compare(array, r, largest) > 0)
                    largest = r;

                if (largest != i)
                {
                    Swap(array, i, largest);

                    heapify(end, largest);
                }
            }
        }

        [Sort]
        public unsafe static void HeapSort<T>(NativeArray<T> array) where T : unmanaged, System.IComparable, System.IComparable<T>
        {
            var sortJob = new HeapSortJob<T>()
            {
                array = (T*)array.GetUnsafePtr(),
                start = 0,
                end = array.Length
            };
            sortJob.Schedule().Complete();
        }

        #endregion

        #region MergeSort

        [BurstCompile]
        unsafe struct MergeSortJob<T> : IJob
        where T : unmanaged, System.IComparable<T>, System.IComparable
        {
            [NativeDisableUnsafePtrRestriction]
            public T* array;
            [NativeDisableUnsafePtrRestriction]
            public T* tempArray;
            public int start;
            public int end;

            public void Execute()
            {
                Partion(start,end - start);
            }

            void Partion (int start , int count )
            {
                if (count > 2)
                {
                    int hcount = count / 2;
                    int mid = start + hcount;

                    Partion(start, hcount);
                    Partion(mid, count - hcount);

                    Merging(array,tempArray, start, hcount, mid, count - hcount);
                }
                else
                {
                    SwapIfGreater(array,start, start + count - 1);
                }
            }
        }

        private unsafe static void Merging<T>(T* array, T* tempArray, int lstart, int lcount, int rstart, int rcount)
            where T : unmanaged, System.IComparable<T>, System.IComparable
        {
            int counterG = 0, counterLeft = 0, counterRight = 0;

            while (counterLeft < lcount && counterRight < rcount)
            {
                if (Compare(array,lstart + counterLeft,rstart +counterRight) < 0)
                {
                    Copy(tempArray, counterG, array, lstart + counterLeft);
                    counterLeft++;
                }
                else
                {
                    Copy(tempArray, counterG, array, rstart + counterRight);
                    counterRight++;
                }
                counterG++;
            }

            while (counterLeft < lcount)
            {
                Copy(tempArray, counterG, array, lstart + counterLeft);
                counterLeft++;
                counterG++;
            }
            while (counterRight < rcount)
            {
                Copy(tempArray, counterG, array, rstart + counterRight);
                counterRight++;
                counterG++;
            }

            int count = lcount + rcount;
            for (int i = 0; i < count; i++)
            {
                Copy(array, lstart + i, tempArray, i);
            }
        }

        [Sort]
        public unsafe static void MergeSort<T>(NativeArray<T> array) where T : unmanaged, System.IComparable, System.IComparable<T>
        {
            NativeArray<T> tempArray = new NativeArray<T>(array.Length, Allocator.TempJob);
            var sortJob = new MergeSortJob<T>()
            {
                array = (T*)array.GetUnsafePtr(),
                tempArray = (T*)tempArray.GetUnsafePtr(),
                start = 0,
                end = array.Length
            };
            sortJob.Schedule().Complete();
            tempArray.Dispose();
            
        }

        #endregion

        #region MergeInsertionSort

        [BurstCompile]
        unsafe struct MergeInsertionSortJob<T> : IJob
        where T : unmanaged, System.IComparable<T>, System.IComparable
        {
            [NativeDisableUnsafePtrRestriction]
            public T* array;
            [NativeDisableUnsafePtrRestriction]
            public T* tempArray;
            public int start;
            public int end;

            public void Execute()
            {
                Partion(start, end - start);
            }

            void Partion(int start, int count)
            {
                if (count <= 1)
                    return;

                if (count == 2)
                {
                    SwapIfGreater(array, start, start + count - 1);
                }else if (count <= 16)
                {
                    InsertionSort(array, start, start + count);
                }
                else
                {
                    int hcount = count / 2;
                    int mid = start + hcount;

                    Partion(start, hcount);
                    Partion(mid, count - hcount);

                    Merging(array, tempArray, start, hcount, mid, count - hcount);
                }
            }
        }

        [Sort]
        public unsafe static void MergeInsertionSort<T>(NativeArray<T> array) where T : unmanaged, System.IComparable, System.IComparable<T>
        {
            NativeArray<T> tempArray = new NativeArray<T>(array.Length, Allocator.TempJob);
            var sortJob = new MergeInsertionSortJob<T>()
            {
                array = (T*)array.GetUnsafePtr(),
                tempArray = (T*)tempArray.GetUnsafePtr(),
                start = 0,
                end = array.Length
            };
            sortJob.Schedule().Complete();
            tempArray.Dispose();
        }

        #endregion

        #region QuickSort

        [BurstCompile]
        unsafe struct QuickSortJob<T> : IJob
        where T : unmanaged, System.IComparable<T>, System.IComparable
        {
            [NativeDisableUnsafePtrRestriction]
            public T* array;
            public int start;
            public int end;

            public void Execute()
            {
                Sort(start, end);
            }

            void Sort(int start , int end)
            {
                int count = end - start;
                if (count == 3)
                {
                    SwapIfGreater(array, start, start + 2);
                    SwapIfGreater(array, start +1, start + 2);
                    SwapIfGreater(array, start, start + 1);
                }
                else if (count == 2)
                {
                    SwapIfGreater(array, start, start + 1);
                }
                else if (count <= 16)
                {
                    InsertionSort(array,start,end);
                }
                else if (count > 3)
                {
                    int mid = Partation(start,end);

                    Sort(start,mid);
                    Sort(mid + 1,end);
                }
            }

            int Partation(int start , int end )
            {
                int left = start, right = end - 2;
                T pivot = UnsafeUtility.ReadArrayElement<T>(array,right + 1);

                while (left < right)
                {
                    if (UnsafeUtility.ReadArrayElement<T>(array,left).CompareTo(pivot) < 0)
                    {
                        left++;
                        continue;
                    }
                    if (UnsafeUtility.ReadArrayElement<T>(array, right).CompareTo(pivot) > 0)
                    {
                        right--;
                        continue;
                    }
                    Swap(array,left, right);
                    left++;
                    right--;
                }

                int mid = UnsafeUtility.ReadArrayElement<T>(array, right).CompareTo(pivot) > 0 ? right : right + 1;
                Swap(array,mid, end - 1);

                return mid;
            }
        }

        [Sort]
        public unsafe static void QuickSort<T>(NativeArray<T> array) where T : unmanaged, System.IComparable, System.IComparable<T>
        {
            var sortJob = new QuickSortJob<T>()
            {
                array = (T*)array.GetUnsafePtr(),
                start = 0,
                end = array.Length
            };
            sortJob.Schedule().Complete();
        }

        #endregion

        #region Radix

        #region RaidxLSB

        [BurstCompile]
        unsafe struct RadixLSBSortJob : IJob
        {
            public NativeArray<uint> array;
            public NativeArray<uint> tempArray;
            public int start;
            public int end;
            public int maxDigit;

            public void Execute()
            {

                int l, r, temp = 0;
                uint digit = 1;

                for (uint i = 0; i < maxDigit; i++)
                {
                    l = start; r = end - 1;

                    for (int j = start; j < end; j++)
                    {
                        uint elemnet = array[j];
                        if ((elemnet & digit) == 0)
                            tempArray[l++] = elemnet;
                        else
                            tempArray[r--] = elemnet;
                    }

                    for (int j = start; j <= r; j++)
                    {
                        array[j] = tempArray[j];
                    }
                    temp = l;
                    for (int j = end - 1; j >= l; j--)
                    {
                        array[temp] = tempArray[j];
                        temp++;
                    }

                    digit <<= 1;
                }
            }
        }

        [Sort]
        public unsafe static void RadixLSBSort(NativeArray<float> array)
        {
            var radixble = ToRadix(array);
            RadixLSBSort(radixble);
            FromRadix(radixble, array);
            radixble.Dispose();
        }

        [Sort]
        public unsafe static void RadixLSBSort(NativeArray<int> array)
        {
            var radixble = ToRadix(array);
            RadixLSBSort(radixble);
            FromRadix(radixble, array);
            radixble.Dispose();
        }

        [Sort]
        public unsafe static void RadixLSBSort(NativeArray<uint> array)
        {
            int maxDigit = (int)RadixUtil.GetMaxBinaryDigit(GetMaxValue(array));
            var tempArray = new NativeArray<uint>(array.Length, Allocator.TempJob);
            var sortJob = new RadixLSBSortJob()
            {
                array = array,
                tempArray = tempArray,
                start = 0,
                end = array.Length,
                maxDigit = maxDigit
            };
            sortJob.Schedule().Complete();
            tempArray.Dispose();
        }

        #endregion

        #region RadixMSDInPlace

        [BurstCompile]
        unsafe struct RadixMSDInPlaceSortJob : IJob
        {
            [NativeDisableUnsafePtrRestriction]
            public uint* array;
            [NativeDisableUnsafePtrRestriction]
            public int* histogram;
            [NativeDisableUnsafePtrRestriction]
            public int* heads;
            [NativeDisableUnsafePtrRestriction]
            public int* tails;
            public int maxDigit;
            public int multi;
            public int start;
            public int end;

            public void Execute()
            {
                sort(start, end, maxDigit);
            }

            void sort(int min, int max, int digitIndex)
            {
                if (max - min <= 1)
                    return;

                int exp = (int)System.Math.Pow(multi, digitIndex);
                int tailsStart = multi * digitIndex;
                int tailsEnd = multi * (digitIndex + 1);

                for (int i = 0; i < multi; i++)
                    UnsafeUtility.WriteArrayElement(histogram, i, 0);

                for (int i = min; i < max; i++)
                    RadixUtil.IncrementArrayElement(histogram, RadixUtil.GetDigit(array, i, exp, multi));

                UnsafeUtility.WriteArrayElement(heads, 0, min);
                UnsafeUtility.WriteArrayElement(tails, tailsStart, min + UnsafeUtility.ReadArrayElement<int>(histogram, 0));

                for (int i = 1; i < multi; i++)
                {
                    UnsafeUtility.WriteArrayElement(heads, i,
                        UnsafeUtility.ReadArrayElement<int>(heads, i - 1) +
                        UnsafeUtility.ReadArrayElement<int>(histogram, i - 1));
                    UnsafeUtility.WriteArrayElement(tails, tailsStart + i,
                        UnsafeUtility.ReadArrayElement<int>(tails, tailsStart + i - 1) +
                        UnsafeUtility.ReadArrayElement<int>(histogram, i));
                }

                uint temp = 0;
                for (int i = 0; i < multi; i++)
                {
                    while (UnsafeUtility.ReadArrayElement<int>(heads, i) < UnsafeUtility.ReadArrayElement<int>(tails, tailsStart + i))
                    {
                        uint element = UnsafeUtility.ReadArrayElement<uint>(array, UnsafeUtility.ReadArrayElement<int>(heads, i));
                        int elementIndex = (int)((element / exp) % multi);
                        while (elementIndex != i)
                        {
                            temp = element;
                            element = UnsafeUtility.ReadArrayElement<uint>(array, UnsafeUtility.ReadArrayElement<int>(heads, elementIndex));
                            UnsafeUtility.WriteArrayElement(array, UnsafeUtility.ReadArrayElement<int>(heads, elementIndex), temp);
                            RadixUtil.IncrementArrayElement(heads, elementIndex);
                            elementIndex = (int)((element / exp) % multi);
                        }
                        UnsafeUtility.WriteArrayElement(array, UnsafeUtility.ReadArrayElement<int>(heads, i), (int)element);
                        RadixUtil.IncrementArrayElement(heads, i);
                    }
                }

                digitIndex--;
                if (digitIndex >= 0)
                {
                    sort(min, UnsafeUtility.ReadArrayElement<int>(tails, tailsStart), digitIndex);
                    for (int i = tailsStart + 1; i < tailsEnd; i++)
                    {
                        sort(UnsafeUtility.ReadArrayElement<int>(tails, i - 1), UnsafeUtility.ReadArrayElement<int>(tails, i), digitIndex);
                    }
                }
            }
        }

        [Sort]
        public unsafe static void RadixMSDInPlaceSort(NativeArray<float> array)
        {
            var radixble = ToRadix(array);
            RadixMSDInPlaceSort(radixble);
            FromRadix(radixble, array);
            radixble.Dispose();
        }

        [Sort]
        public unsafe static void RadixMSDInPlaceSort(NativeArray<int> array)
        {
            var radixble = ToRadix(array);
            RadixMSDInPlaceSort(radixble);
            FromRadix(radixble, array);
            radixble.Dispose();
        }

        [Sort]
        public unsafe static void RadixMSDInPlaceSort(NativeArray<uint> array)
        {
            RadixMSDInPlaceSort(array, 10);
        }

        public unsafe static void RadixMSDInPlaceSort(NativeArray<uint> array , int multi)
        {
            //the log should be to multi base
            int maxDigit = (int)math.log10((double)uint.MaxValue);
            var histogram = new NativeArray<int>(multi , Allocator.TempJob);
            var heads = new NativeArray<int>(multi, Allocator.TempJob);
            var tails = new NativeArray<int>(multi * (maxDigit + 1), Allocator.TempJob);
            var sortJob = new RadixMSDInPlaceSortJob()
            {
                array = (uint*)array.GetUnsafePtr(),
                histogram = (int*)histogram.GetUnsafePtr(),
                heads = (int*)heads.GetUnsafePtr(),
                tails = (int*)tails.GetUnsafePtr(),
                maxDigit = maxDigit,
                multi = multi,
                start = 0,
                end = array.Length
            };
            sortJob.Schedule().Complete();
            histogram.Dispose();
            heads.Dispose();
            tails.Dispose();
        }

        #endregion

        #region RadixMSD

        [BurstCompile]
        struct RadixMSDSSortJob : IJob
        {
            public NativeArray<uint> array;
            public NativeArray<uint> tempArray;
            public NativeArray<int> globalHistogram;
            public int maxDigit;
            public int multi;
            public int start;
            public int end;

            public void Execute()
            {
                sort(array , tempArray , globalHistogram.Slice(multi * maxDigit, multi) , maxDigit);
            }

            void sort(NativeSlice<uint> array , NativeSlice<uint> tempArray , NativeSlice<int> Histogram, int digitIndex)
            {
                if (digitIndex < 0 || array.Length <= 1) return;

                int exp = (int)math.pow((double)multi, digitIndex);

                for (int i = 0; i < multi; i++)
                    Histogram[i] = 0;

                for (int i = 0; i < array.Length; i++)
                    Histogram[(int)((array[i] / exp) % multi)]++;

                for (int i = 1; i < multi; i++)
                    Histogram[i] += Histogram[i - 1];

                for (int i = 0; i < array.Length; i++)
                    tempArray[--Histogram[(int)((array[i] / exp) % multi)]] = array[i];

                for (int i = 0; i < array.Length; i++)
                    array[i] = tempArray[i];

                digitIndex--;
                var newHistogram = globalHistogram.Slice(multi * digitIndex, multi);
                int preHead = 0; int length = 0;
                for (int i = 1; i < multi; i++)
                {
                    preHead = Histogram[i - 1];
                    length = Histogram[i] - preHead;
                    sort(array.Slice(preHead, length), tempArray.Slice(preHead, length) , newHistogram, digitIndex);
                }
                preHead = Histogram[multi - 1];
                length = array.Length - preHead;
                sort(array.Slice(preHead, length), tempArray.Slice(preHead, length), newHistogram, digitIndex);
            }
        }

        [Sort]
        public unsafe static void RadixMSDSort(NativeArray<float> array)
        {
            var radixble = ToRadix(array);
            RadixMSDSort(radixble);
            FromRadix(radixble, array);
            radixble.Dispose();
        }

        [Sort]
        public unsafe static void RadixMSDSort(NativeArray<int> array)
        {
            var radixble = ToRadix(array);
            RadixMSDSort(radixble);
            FromRadix(radixble, array);
            radixble.Dispose();
        }

        [Sort]
        public unsafe static void RadixMSDSort(NativeArray<uint> array)
        {
            RadixMSDSort(array, 10);
        }

        public unsafe static void RadixMSDSort(NativeArray<uint> array, int multi)
        {
            //the log should be to multi base
            int maxDigit = (int)math.log10((double)GetMaxValue(array));
            var histogram = new NativeArray<int>(multi *(maxDigit+1), Allocator.TempJob);
            var tempArray = new NativeArray<uint>(array.Length, Allocator.TempJob);
            var sortJob = new RadixMSDSSortJob()
            {
                array = array,
                tempArray = tempArray,
                globalHistogram = histogram,
                maxDigit = maxDigit,
                multi = multi,
                start = 0,
                end = array.Length
            };
            sortJob.Schedule().Complete();
            histogram.Dispose();
            tempArray.Dispose();
        }

        #endregion

        #region Util

        private static NativeArray<uint> ToRadix(NativeArray<int> array)
        {
            var converted = new NativeArray<uint>(array.Length,Allocator.TempJob);
            for (int i = 0; i < array.Length; i++)
            {
                converted[i] = (RadixUtil.ToRadix(array[i]));
            }
            return converted;
        }

        public static NativeArray<uint> ToRadix(NativeArray<float> array)
        {
            var converted = new NativeArray<uint>(array.Length,Allocator.TempJob);
            for (int i = 0; i < array.Length; i++)
            {
                converted[i] = RadixUtil.ToRadix(array[i]);
            }
            return converted;
        }

        private static void FromRadix(NativeArray<uint> source, NativeArray<int> destenation)
        {
            for (int i = 0; i < source.Length; i++)
            {
                destenation[i] = RadixUtil.FromRadix_int(source[i]);
            }
        }

        public static void FromRadix(NativeArray<uint> source, NativeArray<float> destenation)
        {
            for (int i = 0; i < source.Length; i++)
            {
                destenation[i] = RadixUtil.FromRadix_float(source[i]);
            }
        }

        #endregion

        #endregion

        private static uint GetMaxValue(NativeArray<uint> array)
        {
            uint max = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > max)
                    max = array[i];
            }
            return max;
        }

        private unsafe static void Copy<T>(T* dest, int dIndex, T* source, int sIndex) where T : unmanaged
        {
            UnsafeUtility.WriteArrayElement(dest, dIndex, UnsafeUtility.ReadArrayElement<T>(source, sIndex));
        }

        private unsafe static void Swap<T>(T* array , int i , int j) where T : unmanaged
        {
            T temp = UnsafeUtility.ReadArrayElement<T>(array, i);
            UnsafeUtility.WriteArrayElement(array, i, UnsafeUtility.ReadArrayElement<T>(array, j));
            UnsafeUtility.WriteArrayElement(array, j, temp);
        }

        private unsafe static bool SwapIfGreater<T>(T* array, int i, int j) where T : unmanaged , System.IComparable<T> 
        {
            if (Compare(array,i,j) > 0)
            {
                Swap(array, i, j);
                return true;
            }
            return false;

        }

        private unsafe static int Compare<T>(T* array , int i , int j) where T : unmanaged, System.IComparable<T>
        {
            return UnsafeUtility.ReadArrayElement<T>(array, i).CompareTo(UnsafeUtility.ReadArrayElement<T>(array, j));
        }

    }
}