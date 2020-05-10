using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagedSort
{
    public static class Sort
    {
        #region SelectionSort

        public static void SelectionSort<T>(ListSlice<T> list) where T : System.IComparable, System.IComparable<T>
        {
            for (int i = 1; i < list.Count; i++)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (list[j + 1].CompareTo(list[j]) < 0)
                        list.Swap(j + 1, j);
                    else
                        continue;
                }
            }
        }

        [Sort]
        public static void SelectionSort<T>(List<T> list) where T : System.IComparable, System.IComparable<T>
        {
            for (int i = 1; i < list.Count; i++)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (list[j + 1].CompareTo(list[j]) < 0)
                        Swap(list, j + 1, j);
                    else
                        continue;
                }
            }
        }

        #endregion

        #region InsertionSort

        [Sort]
        public static void InsertionSort<T>(List<T> list) where T : System.IComparable, System.IComparable<T>
        {
            for (int i = 0; i < list.Count; i++)
            {
                T temp = list[i];
                int j = i - 1;
                for (; j >= 0; j--)
                {
                    if (temp.CompareTo(list[j]) < 0)
                        list[j + 1] = list[j];
                    else
                        break;
                }
                list[j + 1] = temp;
            }
        }

        public static void InsertionSort<T>(ListSlice<T> list) where T : System.IComparable, System.IComparable<T>
        {
            for (int i = 0; i < list.Count; i++)
            {
                T temp = list[i];
                int j = i - 1;
                for (; j >= 0; j--)
                {
                    if (temp.CompareTo(list[j]) < 0)
                        list[j + 1] = list[j];
                    else
                        break;
                }
                list[j + 1] = temp;
            }
        }

        #endregion

        #region BubbleSort

        [Sort]
        public static void BubbleSort<T>(List<T> list) where T : System.IComparable<T> , System.IComparable
        {
            BubbleSort(new ListSlice<T>(list));
        }

        public static void BubbleSort<T>(ListSlice<T> list) where T : System.IComparable<T>, System.IComparable
        {
            for (int i = list.Count; i >= 1; i--)
            {
                for (int j = 1; j < i; j++)
                {
                    list.SwapIfGreater(j - 1, j);
                }
            }
        }

        #endregion

        #region CombSort

        [Sort]
        public static void CombSort<T>(List<T> list) where T : System.IComparable<T>, System.IComparable
        {
            CombSort(new ListSlice<T>(list));
        }

        public static void CombSort<T>(ListSlice<T> list) where T : System.IComparable<T>, System.IComparable
        {
            int gap = list.Count;
            bool sorted = false;

            while(!sorted)
            {
                if (gap < 1)
                {
                    sorted = true;
                    gap = 1;
                }

                for (int i = 0; i < list.Count - gap; i++)
                {
                    if(list[i].CompareTo(list[i + gap]) > 0)
                    {
                        list.Swap(i, i + gap);
                        sorted = false;
                    }
                }
                gap = (int)(gap / 1.3f);
            }

        }

        #endregion

        #region ShellSort

        [Sort]
        public static void ShellSort<T>(List<T> list) where T : System.IComparable, System.IComparable<T>
        {
            ShellSort(list, list.Count / 2);
        }

        public static void ShellSort<T>(List<T> list, int step) where T : System.IComparable, System.IComparable<T>
        {
            if (step > 0)
            {
                for (int i = 0; i < list.Count - step; i++)
                {
                    if (list[i].CompareTo(list[i + step]) > 0)
                        Swap(list, i, i + step);
                }
                ShellSort(list, step / 2);
            }
            else
                InsertionSort(list);
        }

        public static void ShellSort<T>(ListSlice<T> list, int step) where T : System.IComparable, System.IComparable<T>
        {
            if (step > 0)
            {
                for (int i = 0; i < list.Count - step; i++)
                {
                    if (list[i].CompareTo(list[i + step]) > 0)
                        list.Swap(i, i + step);
                }
                ShellSort(list, step / 2);
            }
            else
                InsertionSort(list);
        }

        #endregion

        #region HeapSort

        [Sort]
        public static void HeapSort<T>(List<T> list) where T : System.IComparable<T> , System.IComparable
        {
            HeapSort(new ListSlice<T>(list));
        }

        public static void HeapSort<T>(ListSlice<T> list) where T : System.IComparable<T> , System.IComparable
        {
            for (int i = list.Count / 2 - 1; i >= 0; i--)
                heapify(list, i);

            for (int i = list.Count - 1; i >= 0; i--)
            {
                list.Swap(0, i);

                heapify(list.GetSlice(0, i), 0);
            }
        }

        private static void heapify<T>(ListSlice<T> list, int i) where T : System.IComparable<T> , System.IComparable
        {
            int largest = i;
            int l = 2 * i + 1;
            int r = 2 * i + 2;

            if (l < list.Count && list[l].CompareTo(list[largest]) > 0)
                largest = l;

            if (r < list.Count && list[r].CompareTo(list[largest]) > 0)
                largest = r;

            if (largest != i)
            {
                list.Swap(i, largest);

                heapify(list, largest);
            }
        }

        #endregion

        #region MergeSort

        [Sort]
        public static void MergeSort<T>(List<T> list) where T : System.IComparable, System.IComparable<T>
        {
            MergeSort(new ListSlice<T>(list));
        }

        private static void MergeSort<T>(ListSlice<T> list) where T : System.IComparable , System.IComparable<T>
        {
            if(list.Count > 2)
            {
                int mid = list.Count/2;
                var left = list.GetSlice(0, mid);
                var right = list.GetSlice(mid, list.Count - mid);

                MergeSort(left);
                MergeSort(right);

                Merging(left, right);
            }
            else
            {
                list.SwapIfGreater(0, list.Count - 1);
            }
        }
        
        private static void Merging<T>(ListSlice<T> left , ListSlice<T> right) where T : System.IComparable , System.IComparable<T>
        {
            T[] tempArray = new T[left.Count + right.Count];
            int counterG = 0, counterLeft = 0, counterRight = 0;

            while (counterLeft < left.Count && counterRight < right.Count)
            {
                if (left[counterLeft].CompareTo(right[counterRight]) < 0)
                {
                    tempArray[counterG] = left[counterLeft];
                    counterLeft++;
                }
                else
                {
                    tempArray[counterG] = right[counterRight];
                    counterRight++;
                }
                counterG++;
            }

            while (counterLeft < left.Count)
            {
                tempArray[counterG] = left[counterLeft];
                counterLeft++;
                counterG++;
            }
            while (counterRight < right.Count)
            {
                tempArray[counterG] = right[counterRight];
                counterRight++;
                counterG++;
            }

            for (int i = 0; i < tempArray.Length; i++)
            {
                left[i] = tempArray[i];
            }
        }

        #endregion

        #region MergeInsertionSort

        [Sort]
        public static void MergeInsertionSort<T>(List<T> list) where T : System.IComparable<T>, System.IComparable
        {
            MergeInsertionSort(new ListSlice<T>(list));
        }

        private static void MergeInsertionSort<T>(ListSlice<T> list, int threshold = 100) where T : System.IComparable<T> , System.IComparable
        {
            if (list.Count > 10)
            {
                int mid = list.Count / 2;
                var left = list.GetSlice(0, mid);
                var right = list.GetSlice(mid, list.Count - mid);

                MergeInsertionSort(left);
                MergeInsertionSort(right);

                Merging(left, right);
            }
            else
            {
                InsertionSort(list);
            }
        }

        #endregion

        #region QuickSort

        [Sort]
        public static void QuickSort<T>(List<T> list) where T : System.IComparable<T>, System.IComparable
        {
            QuickSort(new ListSlice<T>(list));
        }

        private static void QuickSort<T>(ListSlice<T> list) where T : System.IComparable<T>, System.IComparable
        {
            if (list.Count == 3)
            {
                list.SwapIfGreater(0, 2);
                list.SwapIfGreater(1, 2);
                list.SwapIfGreater(0, 1);
            }
            else if (list.Count == 2)
            {
                list.SwapIfGreater(0, 1);
            }
            else if (list.Count <= 16)
            {
                InsertionSort(list);
            }
            else if (list.Count > 3)
            {
                int mid = Partation(list);

                QuickSort(list.GetSlice(0, mid));
                QuickSort(list.GetSlice(mid + 1, list.Count - mid - 1));
            }
        }

        private static int Partation<T>(ListSlice<T> list) where T : System.IComparable<T>, System.IComparable
        {
            int left = 0, right = list.Count - 2;
            T pivot = list[right + 1];

            while (left < right)
            {
                if (list[left].CompareTo(pivot) < 0)
                {
                    left++;
                    continue;
                }
                if (list[right].CompareTo(pivot) > 0)
                {
                    right--;
                    continue;
                }
                list.Swap(left, right);
                left++;
                right--;
            }

            int mid = list[right].CompareTo(pivot) > 0 ? right : right + 1;
            list.Swap(mid, list.Count - 1);

            return mid;
        }

        #endregion

        #region RadixSort

        #region LSB

        [Sort]
        public static void RadixLSBSort (List<int> list)
        {
            var radixList = ToRadix(list);
            RadixLSBSort(radixList);
            FromRadix(radixList, list);
        }

        [Sort]
        public static void RadixLSBSort(List<float> list)
        {
            var radixList = ToRadix(list);
            RadixLSBSort(radixList);
            FromRadix(radixList, list);
        }

        [Sort]
        public static void RadixLSBSort(List<uint> list)
        {
            uint maxDigit = RadixUtil.GetMaxBinaryDigit(GetMaxValue(list, uint.MinValue));

            uint[] copyArray = new uint[list.Count];
            int l, r, temp = 0;
            uint digit = 1;

            for (uint i = 0; i < maxDigit; i++)
            {
                l = 0; r = list.Count - 1;

                for (int j = 0; j < list.Count; j++)
                {
                    if ((list[j] & digit) == 0)
                        copyArray[l++] = list[j];
                    else
                        copyArray[r--] = list[j];
                }

                for (int j = 0; j <= r; j++)
                {
                    list[j] = copyArray[j];
                }
                temp = l;
                for (int j = list.Count - 1; j >= l; j--)
                {
                    list[temp] = copyArray[j];
                    temp++;
                }

                digit <<= 1;
            }
        }

        #endregion

        #region MSD

        [Sort]
        public static void RadixMSDSort(List<uint> list)
        {
            RadixMSDSort(list, 10);
        }

        [Sort]
        public static void RadixMSDSort(List<int> list)
        {
            var radixList = ToRadix(list);
            RadixMSDSort(radixList,10);
            FromRadix(radixList, list);
        }

        [Sort]
        public static void RadixMSDSort(List<float> list)
        {
            var radixList = ToRadix(list);
            RadixMSDSort(radixList, 10);
            FromRadix(radixList, list);
        }

        public static void RadixMSDSort(List<uint> list, int multi)
        {
            int digitCount = (int)System.Math.Log(GetMaxValue(list, uint.MinValue), multi);
            RadixMSDSort(list, 0, list.Count, digitCount, multi);
        }

        private static void RadixMSDSort(List<uint> list, int min, int max, int digitIndex, int multi)
        {
            if (digitIndex < 0 || max-min <= 1) return;

            int exp = (int)Mathf.Pow(multi, digitIndex);
            int[] digitsCount = new int[multi];
            uint[] tempArray = new uint[max - min];

            for (int i = min; i < max; i++)
                digitsCount[(list[i] / exp) % multi]++;

            for (int i = 1; i < digitsCount.Length; i++)
                digitsCount[i] += digitsCount[i - 1];

            for (int i = min; i < max; i++)
                tempArray[--digitsCount[(list[i] / exp) % multi]] = list[i];

            for (int i = 0; i < tempArray.Length; i++)
                list[i + min] = tempArray[i];

            digitIndex--;
            for (int i = 1; i < multi; i++)
            {
                RadixMSDSort(list, digitsCount[i - 1] + min, digitsCount[i] + min, digitIndex, multi);
            }
            RadixMSDSort(list, digitsCount[digitsCount.Length - 1] + min, max, digitIndex, multi);
        }

        #endregion

        #region MSDInPlaceSort

        [Sort]
        public static void InPlaceRadixSort(List<uint> list)
        {
            InPlaceRadixSort(list, 10);
        }

        [Sort]
        public static void InPlaceRadixSort(List<int> list)
        {
            var radixList = ToRadix(list);
            InPlaceRadixSort(radixList, 10);
            FromRadix(radixList, list);
        }

        [Sort]
        public static void InPlaceRadixSort(List<float> list)
        {
            var radixList = ToRadix(list);
            InPlaceRadixSort(radixList, 10);
            FromRadix(radixList, list);
        }

        public static void InPlaceRadixSort(List<uint> list , int multi = 10)
        {
            int digitCount = (int)System.Math.Log(GetMaxValue(list, uint.MinValue), multi) + 1;
            InPlaceRadixSort(list, 0, list.Count, digitCount, 0 , multi);
        }

        public static void InPlaceRadixSort(List<uint> list , int min ,int max, int k, int l, int multi)
        {
            if (max - min <= 1)
                return;

            int exp = (int)System.Math.Pow(multi, k - l - 1);
            var histogram = BuildHistogram(list , min , max, exp, multi);
            int[] heads = new int[multi];
            int[] tails = new int[multi];
            heads[0] = min;
            tails[0] = histogram[0] + min;

            for (int i = 1; i < multi; i++)
            {
                heads[i] = heads[i - 1] + histogram[i - 1];
                tails[i] = tails[i - 1] + histogram[i];
            }

            uint temp = 0;
            for (int i = 0; i < multi; i++)
            {
                while (heads[i] < tails[i])
                {
                    uint element = list[heads[i]];
                    int elementIndex = (int)((element / exp) % multi);
                    while (elementIndex != i)
                    {
                        temp = element;
                        element = list[heads[elementIndex]];
                        list[heads[elementIndex]] = temp;
                        heads[elementIndex]++;
                        elementIndex = (int)((element / exp) % multi);
                    }
                    list[heads[i]] = element;
                    heads[i]++;
                }
            }

            if (l < k - 1)
            {
                InPlaceRadixSort(list, min , tails[0], k, l + 1, multi);
                for (int i = 1; i < multi; i++)
                {
                    if (tails[i - 1] != tails[i])
                    {
                        InPlaceRadixSort(list ,tails[i - 1], tails[i] , k, l + 1, multi);
                    }
                }
            }
        }

        public static int[] BuildHistogram(List<uint> list , int min , int max, int exp, int multi)
        {
            int[] histogram = new int[multi];
            for (int i = min; i < max; i++)
            {
                histogram[(list[i] / exp) % multi]++;
            }
            return histogram;
        }

        #endregion

        #region Util

        private static List<uint> ToRadix(List<int> list)
        {
            var converted = new List<uint>(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                converted.Add(RadixUtil.ToRadix(list[i]));
            }
            return converted;
        }

        public static List<uint> ToRadix(List<float> list)
        {
            var converted = new List<uint>(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                converted.Add(RadixUtil.ToRadix(list[i]));
            }
            return converted;
        }

        private static void FromRadix(List<uint> source, List<int> destenation)
        {
            for (int i = 0; i < source.Count; i++)
            {
                destenation[i] = RadixUtil.FromRadix_int(source[i]);
            }
        }

        public static void FromRadix(List<uint> source, List<float> destenation)
        {
            for (int i = 0; i < source.Count; i++)
            {
                destenation[i] = RadixUtil.FromRadix_float(source[i]);
            }
        }

        #endregion

        #endregion

        private static T GetMaxValue<T> (List<T> list , T minValue) where T : System.IComparable<T> , System.IComparable
        {
            T max = minValue;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].CompareTo(max) > 0) max = list[i];
            }
            return max;
        }

        private static void Swap<T> (List<T> list , int i, int j)
        {
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        private static void SortWithCount(List<int> list, System.Action<List<int>, ValueWrapper<int>, ValueWrapper<int>> sortMethod)
        {
            ValueWrapper<int> compare = new ValueWrapper<int>(0);
            ValueWrapper<int> arrayAccess = new ValueWrapper<int>(0);
            sortMethod(list, compare, arrayAccess);
            Debug.Log("array access : " + arrayAccess.value);
            Debug.Log("compares : " + compare.value);
        }

    }
}