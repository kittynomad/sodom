/*****************************************************************************
// File Name : FriendlyFireTags.cs
// Author : Arcadia Koederitz
// Creation Date : 9/18/2026
// Last Modified : 9/18/2026
//
// Brief Description : Set of tags that define what enemies can hit each other.
*****************************************************************************/
using System;
using UnityEngine;

[Flags]
public enum FriendlyFireTags
{
    None = 0,
    Paint = 1 << 0,
    TestTag = 1 << 1
}
