/*****************************************************************************
// File Name : ComponentReferenceAttribute.cs
// Author : Arcadia Koederitz
// Creation Date : 10/4/2026
// Last Modified : 10/4/2026
//
// Brief Description : Custom Attribute that automatically gets and assigns a component.
*****************************************************************************/
using System;
using UnityEngine;

namespace AriUtils
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ComponentReferenceAttribute : PropertyAttribute
    {
    
    }
}
