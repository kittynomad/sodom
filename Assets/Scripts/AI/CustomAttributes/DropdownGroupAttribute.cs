/*****************************************************************************
// File Name : DropdownGroupAttribute.cs
// Author : Arcadia Koederitz
// Creation Date : 12/30/2025
// Last Modified : 12/30/2025
//
// Brief Description : Attribute for grouping classes together in the ClassDropdownAttribute.
*****************************************************************************/
using System;
using UnityEngine;

namespace CustomAttributes
{
    public class DropdownGroupAttribute : Attribute
    {
        public string GroupName { get; }

        public DropdownGroupAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }
}
