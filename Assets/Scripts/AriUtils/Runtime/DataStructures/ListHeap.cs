/*****************************************************************************
// File Name : ListHeap.cs
// Author : Arcadia Koederitz
// Creation Date : 5/26/2026
// Last Modified : 5/26/2026
//
// Brief Description : Generic implementation of a heap
*****************************************************************************/
using System;
using System.Collections.Generic;

namespace AriUtils
{
    public class ListHeap<T> where T : IComparable<T>
    {
        protected readonly List<T> list = new List<T>();

        // Implement heap formulas for finding parent/child relations.
        protected static int GetParentIndex(int index)
        {
            return (index - 1) / 2;
        }

        protected static int GetLeftChildIndex(int index)
        {
            return 2 * index + 1;
        }

        protected static int GetRightChildIndex(int index)
        {
            return 2 * index + 2;
        }

        // Parent/Child getters.
        protected T GetParent(int index)
        {
            return list[GetParentIndex(index)];
        }

        protected T GetLeftChild(int index)
        {
            int cIndex = GetLeftChildIndex(index);
            if (cIndex > list.Count)
            {
                return default;
            }
            return list[cIndex];
        }

        protected T GetRightChild(int index)
        {
            int rIndex = GetRightChildIndex(index);
            if (rIndex > list.Count)
            {
                return default;
            }
            return list[rIndex];
        }

        protected T[] GetLeaves()
        {
            int start = list.Count / 2;
            T[] leaves = new T[list.Count - start];
            int j = 0;
            for(int i = 0; i < list.Count; i++)
            {
                leaves[j] = list[i];
                j++;
            }
            return leaves;
        }

        public override string ToString()
        {
            string heapString = "";
            int level = 0;
            int i = 0;
            while(i <  list.Count)
            {
                heapString += (level + 1) + ". ";
                double loopCount = MathF.Pow(2, level);
                for(int j = 0; j  < loopCount && i < list.Count; j++)
                {
                    heapString += list[i] + " ";
                    i++;
                }
                level++;
                if (i < list.Count)
                {
                    heapString += "\n";
                }
            }
            return heapString;
        }
    }
}
