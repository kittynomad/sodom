/************************************************************************
// File Name : SingletonUtility.cs
// Author : Arcadia Koederitz
// Creation Date : 10/16/2026
// Last Modified : 10/16/2026
//
// Brief Description : Set of utility functions for managing singletons.
*****************************************************************************/
using UnityEngine;

namespace TFOOL
{
    public static class SingletonUtility
    {
        public static bool SetSingleton<T>(ref T instanceVar, T obj) where T: MonoBehaviour
        {
            if (instanceVar != null && instanceVar != obj)
            {
                Debug.LogError($"Duplicate {obj.GetType().Name} found.  Deleting.");
                GameObject.Destroy(obj.gameObject);
                return false;
            }
            else
            {
                instanceVar = obj;
                return true;
            }
        }
    
    }
}
