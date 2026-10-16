/*****************************************************************************
// File Name : ListMinHeap.cs
// Author : Arcadia Koederitz
// Creation Date : 5/26/2026
// Last Modified : 5/26/2026
//
// Brief Description : Heap with minimum rule.
*****************************************************************************/
using System;
using System.Diagnostics;

namespace AriUtils
{
    public class ListMinHeap<T> : ListHeap<T> where T : IComparable<T>
    {
        public ListMinHeap() { }

        public bool IsEmpty => list.Count == 0;

        /// <summary>
        /// Adds an element to this heap, maintaining the min heap rule.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(T item)
        {
            int insertIndex = list.Count;
            list.Add(item);

            // Heapify Up.
            HeapifyUp(insertIndex);
        }
        
        /// <summary>
        /// Removes the minimum root element from this heap, maintaining the heap rule.
        /// </summary>
        /// <returns>The removed item.</returns>
        public T RemoveMin()
        {
            return Remove(0);
        }

        /// <summary>
        /// Removes an element from the heap.
        /// </summary>
        /// <param name="index">The index of the item to remove.</param>
        /// <returns>The removed element.</returns>
        public T Remove(int index)
        {
            // Swap the element to remove and the last index, then remove at the end.
            T returnVal = list[index];
            Swap(index, list.Count - 1);
            list.RemoveAt(list.Count - 1);
            if (list.Count == 0)
            {
                return returnVal;
            }


            if (list[index].CompareTo(list[GetParentIndex(index)]) < 0)
            {
                HeapifyUp(index);
            }
            else
            {
                HeapifyDown(index);
            }
            return returnVal;
        }

        private void HeapifyUp(int index)
        {
            int parentIndex = GetParentIndex(index);
            if (index > 0 && list[index].CompareTo(list[parentIndex]) < 0)
            {
                Swap(index, parentIndex);
                HeapifyUp(parentIndex);
            }
        }

        private void HeapifyDown(int index)
        {
            // Find the index of the smallest child.
            int smallestIndex = index;
            int leftChildIndex = GetLeftChildIndex(index);
            int rightChildIndex = GetRightChildIndex(index);

            if (leftChildIndex < list.Count && list[leftChildIndex].CompareTo(list[smallestIndex]) < 0)
            {
                smallestIndex = leftChildIndex;
            }
            if (rightChildIndex < list.Count && list[rightChildIndex].CompareTo(list[smallestIndex]) < 0)
            {
                smallestIndex = rightChildIndex;
            }

            if (smallestIndex != index)
            {
                Swap(smallestIndex, index);
                HeapifyDown(smallestIndex);
            }
        }

        private void Swap(int index1, int index2)
        {
            (list[index1], list[index2]) = (list[index2], list[index1]);
        }
    }
}
