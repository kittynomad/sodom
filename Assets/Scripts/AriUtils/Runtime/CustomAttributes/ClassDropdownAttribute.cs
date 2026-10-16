/*****************************************************************************
// File Name : ClassDropdownAttribute.cs
// Author : Arcadia Koederitz
// Creation Date : 12/30/2025
// Last Modified : 12/30/2025
//
// Brief Description : Custom attribute that allows for selecting a modifiable child class from a dropdown menu.
*****************************************************************************/
using System;
using UnityEngine;

namespace AriUtils
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ClassDropdownAttribute : PropertyAttribute
    {
        public bool RestrictAssemblies { get; set; }
        
        public ClassDropdownAttribute(bool restrictAssemblies = true)
        {
            RestrictAssemblies = restrictAssemblies;
        }
    }
}
