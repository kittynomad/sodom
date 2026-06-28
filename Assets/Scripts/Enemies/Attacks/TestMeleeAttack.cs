/*****************************************************************************
// File Name : TestMeleeAttack.cs
// Author : Arcadia Koederitz
// Creation Date : 5/21/2026
// Last Modified : 5/21/2026
//
// Brief Description : Test melee attack script that just spawns a hitbox for a certain duration.
*****************************************************************************/
using System;
using System.Threading;
using TFOOL.Enemies.AI;
using UnityEngine;

namespace TFOOL.Enemies
{
    [System.Serializable]
    public class TestMeleeAttack : EnemyAttack
    {
        [SerializeField] private GameObject hitbox;
        [SerializeField, Tooltip("Amount of tiime the hitbox remains active.")] 
        private float attackTime;

        /// <summary>
        /// Performs a basic spawn hitbox attack with a given timing.
        /// </summary>
        /// <param name="attackTime">The amount of time for the attack.</param>
        /// <param name="ct">The cancellation token for the enemy behavior.</param>
        /// <returns></returns>
        public override async Awaitable PerformAttack(EnemyController enemy, GameObject target, CancellationToken ct)
        {
            try
            {
                hitbox.SetActive(true);
                await Awaitable.WaitForSecondsAsync(attackTime, ct);
                hitbox.SetActive(false);
            }
            catch (OperationCanceledException oce)
            {
                hitbox.SetActive(false);
                throw oce;
            }
        }
    }
}