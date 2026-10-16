/*****************************************************************************
// File Name : CameraHelpers.cs
// Author : Arcadia Koederitz
// Creation Date : September 7, 2025
// Last Modified : September 7, 2025
//
// Brief Description : Set of static helper functions that deal the camera and screen
// that have common utility across multiple projects.
*****************************************************************************/
using UnityEngine;

namespace AriUtils
{
    public static class CameraUtil
    {
        /// <summary>
        /// Checks if a given renderer is within the planes of a camera.
        /// </summary>
        /// <param name="cam">The camera to check if the object is within.</param>
        /// <param name="renderer">The renderer to check if it's in the camera's view.</param>
        /// <returns>True if the object is within the camera's view.</returns>
        public static bool CheckObjectInBounds(Camera cam, Renderer renderer)
        {
            return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(cam), renderer.bounds);
        }

        /// <summary>
        /// Checks if a given point in space is within the planes of a camera.
        /// </summary>
        /// <param name="cam">The camera to check if the object is within.</param>
        /// <param name="point">The point in space to check for.</param>
        /// <returns>True if the object is within the camera's view.</returns>
        public static bool CheckPointInBounds(Camera cam, Vector3 point)
        {
            return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(cam),
                new Bounds(point, Vector3.zero));
        }
    }
}