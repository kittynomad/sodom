/*****************************************************************************
// File Name : Helpers.cs
// Author : Arcadia Koederitz
// Creation Date : May 27, 2025
//
// Brief Description : Set of static helper functions that have common utility across multiple projects.
*****************************************************************************/
using UnityEngine;

namespace AriUtils
{
    public static class RandomUtil
    {
        /// <summary>
        /// Generates a random true/false outcome based on a given percentage chance.
        /// </summary>
        /// <param name="chance">The percentage chance of a true.  Int value between 0 and 100.</param>
        /// <returns>True if the roll was successful, false if not.</returns>
        public static bool PercentageChance(int chance)
        {
            return chance > Random.Range(0, 100);
        }

        /// <summary>
        /// Gets a random element from an array, taking into account the weight of each element.
        /// </summary>
        /// <param name="weightedElements">The weighted elements to get a random element from.</param>
        /// <returns>The index of the chosen element.</returns>
        public static int GetRandomIndexWeighted(IWeighted[] weightedElements)
        {
            int totalWeight = 0;
            foreach (IWeighted element in weightedElements)
            {
                totalWeight += element.Weight;
            }
            int randomWeight = Random.Range(0, totalWeight);

            for (int i = 0; i < weightedElements.Length; i++)
            {
                randomWeight -= weightedElements[i].Weight;
                if (randomWeight < 0)
                {
                    return i;
                }
            }
            // Failed to find a valid element.
            return -1;
        }
    }
}