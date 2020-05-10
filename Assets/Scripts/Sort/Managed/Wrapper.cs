using UnityEngine;
using System.Collections;

namespace ManagedSort
{
    public class ValueWrapper<T> where T : struct
    {
        public T value;

        public ValueWrapper(T value)
        {
            this.value = value;
        }
    }

}
