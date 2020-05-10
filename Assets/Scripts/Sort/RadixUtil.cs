using UnityEngine;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;

public static unsafe class RadixUtil 
{
    public static uint GetMaxBinaryDigit(uint num)
    {
        uint maxDigit = 1u << 31;
        for (uint i = 32; i > 0; i--)
        {
            if ((num & maxDigit) == 0)
            {
                maxDigit >>= 1;
                continue;
            }
            else
                return i;
        }
        return 0;
    }

    public unsafe static int GetDigit(uint* array, int index, int exp, int multi)
    {
        return (int)((UnsafeUtility.ReadArrayElement<uint>(array, index) / exp) % multi);
    }

    public unsafe static void IncrementArrayElement(int* array, int index)
    {
        UnsafeUtility.WriteArrayElement(array, index, UnsafeUtility.ReadArrayElement<int>(array, index) + 1);
    }

    public unsafe static uint ToRadix(float num)
    {
        uint rdxble = *((uint*)&num);
        if ((rdxble >> 31) == 1)
        {
            rdxble = ~rdxble;
            rdxble ^= (1u << 31);
        }
        rdxble ^= (1u << 31);
        return rdxble;
    }

    public unsafe static float FromRadix_float(uint rdxble)
    {
        rdxble ^= (1u << 31);
        if ((rdxble >> 31) == 1)
        {
            rdxble ^= (1u << 31);
            rdxble = ~rdxble;
        }
        return *((float*)&rdxble);
    }

    public unsafe static uint ToRadix(int num)
    {
        return (uint)num ^ (1u << 31);
    }

    public unsafe static int FromRadix_int(uint num)
    {
        return (int)(num ^ (1u << 31));
    }
}
