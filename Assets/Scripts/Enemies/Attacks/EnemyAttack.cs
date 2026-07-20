/*****************************************************************************
// File Name : EnemyAttack.cs
// Author : Arcadia Koederitz
// Creation Date : 5/19/2026
// Last Modified : 5/21/2026
//
// Brief Description : Script that represents a specific attack an enemy can use.
*****************************************************************************/
using System.Threading;
using TFOOL.Enemies.AI;
using UnityEngine;

namespace TFOOL.Enemies
{
    [System.Serializable]
    public abstract class EnemyAttack
    {
        [SerializeField, Tooltip("Name that is used by the EnemyAI to perform this attack from an AttackBehavior.")] 
        private string attackName;

        public string Name => attackName;

        public abstract Awaitable PerformAttack(EnemyController enemy, GameObject target, EnemyAttacker attackerComp, CancellationToken ct);

        /// <summary>
        /// Custom condition check for specific attacks 
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="target"></param>
        /// <param name="attackerComp"></param>
        /// <returns></returns>
        public virtual bool CheckPerformable(EnemyController enemy, GameObject target, EnemyAttacker attackerComp)
        {
            return true;
        }

        /// <summary>
        /// Custom modifier to the weight of the attack based on specific conditions.
        /// </summary>
        /// <remarks>
        /// Would be used for things like making a move more probable in certain conditions.
        /// </remarks>
        /// <param name="baseWeight"></param>
        /// <param name="enemy"></param>
        /// <param name="target"></param>
        /// <param name="attackerComp"></param>
        /// <returns></returns>
        public virtual int ModifyWeight(int baseWeight, EnemyController enemy, GameObject target, EnemyAttacker attackerComp)
        {
            return baseWeight;
        }
    }
}