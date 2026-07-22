/*****************************************************************************
// File Name : RandomUtility.cs
// Author : Arcadia Koederitz
// Creation Date : 6/29/2026
// Last Modified : 6/29/2026
//
// Brief Description : Set of utility functions for randomness.
*****************************************************************************/
using TFOOL.Enemies.AI;
using UnityEngine;

public static class RandomUtility
{
    /// <summary>
    /// Gets a random element from an array, taking into account the weight of each element.
    /// </summary>
    /// <param name="weightedElements">The weighted elements to get a random element from.</param>
    /// <returns>The index of the chosen element.</returns>
    public static int GetRandomIndexWeighted(IWeighted[] weightedElements, float param = 0)
    {
        int totalWeight = 0;
        foreach (IWeighted element in weightedElements)
        {
            totalWeight += element.GetWeight(param);
        }
        int randomWeight = Random.Range(0, totalWeight);

        for(int i = 0; i < weightedElements.Length; i++)
        {
            randomWeight -= weightedElements[i].GetWeight(param);
            if (randomWeight < 0)
            {
                return i;
            }
        }
        // Failed to find a valid element.
        return -1;
    }
}
