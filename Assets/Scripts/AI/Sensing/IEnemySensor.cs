/*****************************************************************************
// File Name : EnemySensor.cs
// Author : Arcadia Koederitz
// Creation Date : 6/7/2026
// Last Modified : 6/7/2026
//
// Brief Description : Interface for any script that acts as a sense for the enemy.
*****************************************************************************/
using Sodom.Enemies.AI;
using System;
using UnityEngine;

public interface IEnemySensor
{
    event Action<GameObject, SenseType, bool> EntitySenseEvent;
}
