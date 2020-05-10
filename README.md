# Native-Sort-Test

testing different sorting algorithms using unity native container and compare to sorting in the managed side

# Included sorting algorithms
- Selection Sort
- Bubble Sort
- Insertion Sort 
- Comb Sort
- Heap Sort
- Merge Sort
- Merge Insertion Sort
- Quick Sort
- Radix LSD Sort
- Radix LSB Sort
- Radix MSD Sort

both soring algorithm implmented to work with List<> and NativeArray<>

# Conclusion
- by building with IL2CPP you will see performance increase with NativeArray compare to the same sorting method running on the List.
- when sorting List<int> with Radix MSD it's performance will be the highest compare to the other sorting methods but when doing the same test on the NativeArray
