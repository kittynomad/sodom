/*****************************************************************************
// File Name : AttackCondition.cs
// Author : Arcadia Koederitz
// Creation Date : 7/20/2026
// Last Modified : 7/20/2026
//
// Brief Description : Specialized condition that can be added to attack behaviors to define when they occur.
*****************************************************************************/
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public abstract class AttackCondition
    {
        /// <returns>True if this attack is valid, false otherwise.</returns>
        public abstract bool CheckCondition(EnemyController enemy, AttackBehavior attackBehavior, EnemyAttacker attacker);
    }
}
