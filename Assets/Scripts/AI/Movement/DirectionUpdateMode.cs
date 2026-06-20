/*****************************************************************************
// File Name : DirectionUpdateMode.cs
// Author : Arcadia Koedderitz
// Creation Date : 6/20/2026
// Last Modified : 6/20/2026
//
// Brief Description : Enum that determines how enemy movement should update the facing direction of the enemy it's moving/
*****************************************************************************/
using UnityEngine;

namespace TFOOL.Enemies
{
    public enum DirectionUpdateMode
    {
        None,
        TargetDirection,
        Velocity,
        ToTarget
    }

}