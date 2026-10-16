/*****************************************************************************
// File Name : EditableSO.cs
// Author : Arcadia Koederitz
// Creation Date : 10/25/2025
// Last Modified : 10/25/2025
//
// Brief Description : Allows modiftying a scriptable object reference's values from an object reference field.
*****************************************************************************/
using System;
using UnityEngine;

namespace AriUtils
{
    [AttributeUsage(AttributeTargets.Field)]
    [Obsolete("ShowNestedEditor is obsolete.  Use NaughtyAttributes.Expandable instead.")]
    public class ShowNestedEditorAttribute : PropertyAttribute
    {
        
    }
}
