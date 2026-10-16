/*****************************************************************************
// File Name : MathHelpers.cs
// Author : Arcadia Koederitz
// Creation Date : September 7, 2025
// Last Modified : September 7, 2025
//
// Brief Description : Set of static helper functions containing commonly used math equasions
// that have common utility across multiple projects.
*****************************************************************************/
using UnityEngine;

namespace AriUtils
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts a bool to an int value, either 1 or 0.
        /// </summary>
        /// <remarks>
        /// Useful for saving boolean values if bool values are not supported (like with PlayerPrefs)
        /// </remarks>
        /// <param name="value">The boolean value to convert to an integer.</param>
        /// <returns>1 if true, 0 if false.</returns>
        public static int ToInt(this bool value)
        {
            return value ? 1 : 0;
        }

        /// <summary>
        /// Sets the alpha value of a color.
        /// </summary>
        /// <param name="color">The color to modify.</param>
        /// <param name="alpha">The alpha value to set.</param>
        public static Color SetAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}