using System;
using System.Collections.Generic;

namespace ManagedSort
{
    public struct ListSlice<T> where T : IComparable , IComparable<T> 
    {
        public int Count;

        List<T> list;
        public int start;

        public ListSlice(List<T> list, int start, int count)
        {
            this.list = list;
            this.start = start;
            this.Count = count;
            if (Count > list.Count) Count = list.Count;
        }

        public ListSlice(List<T> list)
        {
            this.list = list;
            this.start = 0;
            this.Count = list.Count;
        }

        public ListSlice<T> GetSlice(int start, int count)
        {
            return new ListSlice<T>(list, this.start + start, count);
        }

        public List<T> ToList()
        {
            return list;
        }

        public void SwapIfGreater (int i , int j)
        {
            if (this[i].CompareTo(this[j]) > 0)
                Swap(i, j);
        }

        public T this[int i]
        {
            get
            {
                return list[i + start];
            }
            set
            {
                list[i + start] = value;
            }
        }

        public void Swap (int i , int j)
        {
            T num = this[i];
            this[i] = this[j];
            this[j] = num;
        }

    }
}