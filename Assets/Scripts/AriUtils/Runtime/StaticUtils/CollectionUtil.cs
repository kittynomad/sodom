/*****************************************************************************
// File Name : CollectionHelpers.cs
// Author : Arcadia Koederitz
// Creation Date : September 7, 2025
// Last Modified : September 7, 2025
//
// Brief Description : Set of static helper functions that deal with collections 
// that have common utility across multiple projects.
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AriUtils
{
    public static class CollectionUtil
    {
        /// <summary>
        /// Returns all items in a list that are a specific child class of this list.
        /// </summary>
        /// <typeparam name="T"> The type of the child objects to return a list of. </typeparam>
        /// <typeparam name="E"> The type of this list. </typeparam>
        /// <param name="inputList"> This list to filter child classes for. </param>
        /// <returns> A list of child classes from the passed in list. </returns>
        [Obsolete("FilterToChildList is obsolete.  Use Linq.Where() instead.")]
        public static List<T> FilterToChildList<T, E>(List<E> inputList) where T : E
        {
            List<T> outputList = new List<T>();
            for (int i = 0; i < inputList.Count; i++)
            {
                if (inputList[i] is T item)
                {
                    outputList.Add(item);
                }
            }
            return outputList;
        }

        /// <summary>
        /// Loops an index around if it exceeds the bounds of a collection.
        /// </summary>
        /// <typeparam name="T">The type stored in the collection.</typeparam>
        /// <param name="collection">THe collection to loop the index within.</param>
        /// <param name="index">The current value of the index.</param>
        /// <returns> The looped index value.</returns>
        public static bool LoopIndex<T>(IEnumerable<T> collection, ref int index)
        {
            bool didLoop = false;
            if (index >= collection.Count())
            {
                didLoop = true;
                index -= collection.Count();
            }
            else if (index < 0)
            {
                didLoop = true;
                index += collection.Count();
            }
            return didLoop;
        }

        /// <summary>
        /// Checks if a given index is insid ethe bounds of a collection.
        /// </summary>
        /// <typeparam name="T">The type of the collection.</typeparam>
        /// <param name="collection">The collection to check the index of.</param>
        /// <param name="index">The index to check.</param>
        /// <returns>True if the index is within the bounds of the collection.</returns>
        public static bool IndexInRange<T>(IEnumerable<T> collection, int index)
        {
            return index > 0 && index < collection.Count() - 1;
        }

        /// <summary>
        /// Prints a collection of values to the console.
        /// </summary>
        /// <typeparam name="T">The type of the collection.</typeparam>
        /// <param name="collection">The collection to print.</param>
        /// <param name="collectionName">A specific name that can be used to identify the collection.</param>
        public static void LogCollection<T>(IEnumerable<T> collection, string collectionName = "")
        {
            T[] array = collection.ToArray();
            if (collectionName == "")
            {
                collectionName = collection.ToString();
            }
            for (int i = 0; i < array.Length; i++)
            {
                Debug.Log($"Collection {collectionName} contains item {array[i]} at index {i}.");
            }
        }
    }

}