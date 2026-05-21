/*****************************************************************************
// File Name : AttackBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 5/19/2026
//
// Brief Description : Base class for enemy attacks.
*****************************************************************************/
using UnityEngine;

namespace Sodom.Enemies.AI
{
    [System.Serializable]
    public abstract class AttackBehavior : EnemyBehavior
    {
        [SerializeField] protected float postAttackDelay;
    }
}