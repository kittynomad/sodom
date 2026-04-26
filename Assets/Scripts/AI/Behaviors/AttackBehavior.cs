/*****************************************************************************
// File Name : AttackBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Base class for attack based enemy behaviors.
*****************************************************************************/
using UnityEngine;

[System.Serializable]
public abstract class AttackBehavior : EnemyBehavior
{
    [SerializeField] protected float postAttackDelay;
}
