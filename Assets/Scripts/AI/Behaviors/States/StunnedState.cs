/*****************************************************************************
// File Name : StunnedState.cs
// Author : Arcadia Koederitz
// Creation Date : 6/7/2026
// Last Modified : 6/7/2026
//
// Brief Description : State for enemies being stunned by player attacks.
*****************************************************************************/
using UnityEngine;
using System.Threading;
using System;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public class StunnedState : EnemyState
    {
        [SerializeField] private float stunTime;

        public override bool IsCancellable => false;
        protected override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            await base.RunAI(enemy, ct);
            ct.ThrowIfCancellationRequested();

            // Get Components
            if (!enemy.TryGetComponent(out EnemyMovement movement))
            {
                throw new System.NullReferenceException($"Enemy {enemy} does not have a EnemyMovement component.");
            }

            void CleanUp()
            {
                // Reset back to defaults.
            }
    
            try
            {
                // Stop any enemy movement.
                movement.SetDirection(0);
                // Delay until stun expires.  The enemy shouldn't be doing anything during this time.
                await Awaitable.WaitForSecondsAsync(stunTime);
                CleanUp();
            }
            catch (OperationCanceledException oce)
            {
                CleanUp();
                throw oce;
            }
        }
    }
}