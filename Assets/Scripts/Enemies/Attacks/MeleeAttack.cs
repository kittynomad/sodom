/*****************************************************************************
// File Name : MeleeAttack.cs
// Author : Arcadia Koederitz
// Creation Date : 5/21/2026
// Last Modified : 8/20/2026
//
// Brief Description : Basic melee attack script for attacks that just use simple animations with no triggers.
*****************************************************************************/
using System;
using System.Threading;
using TFOOL.Enemies.AI;
using UnityEngine;

namespace TFOOL.Enemies
{
    [System.Serializable]
    public class MeleeAttack : EnemyAttack
    {
        [SerializeField, Tooltip("The name of the animation state that should be played when this attack is used.")] 
        private string animationStateName;
        [SerializeField, Tooltip("If true, the enemy will point towards the target before attacking.")]
        private bool pointTowardsTarget = true;

        /// <summary>
        /// Performs a basic spawn hitbox attack with a given timing.
        /// </summary>
        /// <param name="attackTime">The amount of time for the attack.</param>
        /// <param name="ct">The cancellation token for the enemy behavior.</param>
        /// <returns></returns>
        public override async Awaitable PerformAttack(EnemyController enemy, GameObject target, EnemyAttacker attackerComp, CancellationToken ct)
        {
            try
            {
                if (pointTowardsTarget)
                {
                    enemy.PointTowardsTarget();
                }
                // Play the attack animation.
                await AIUtilities.PlayAndAwaitAnimation(animationStateName, enemy.Animator, ct);
            }
            catch (OperationCanceledException oce)
            {
                throw oce;
            }
        }
    }
}