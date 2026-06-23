/*****************************************************************************
// File Name : DropdownTagAttribute.cs
// Author : Arcadia Koederitz
// Creation Date : 12/30/2025
// Last Modified : 12/30/2025
//
// Brief Description : Attribute for filtering certain classes with the ClassSelector
*****************************************************************************/
using System;
using UnityEngine;

namespace CustomAttributes
{
    public class DropdownTagAttribute : Attribute
    {
        public string Tag { get; }

        public DropdownTagAttribute(string groupName)
        {
            Tag = groupName;
        }
    }

}