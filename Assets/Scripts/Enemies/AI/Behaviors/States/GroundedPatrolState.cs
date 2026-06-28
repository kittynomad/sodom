/*****************************************************************************
// File Name : PatrolState.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Controls the enemy while patrolling.
*****************************************************************************/
using System;
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public class GroundedPatrolState : EnemyState
    {
        [SerializeField, Tooltip("How long to wait at each end of the enemy's patrol range before turning around.")]
        private float patrolWait;

        public override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            await base.RunAI(enemy, ct);
            if (!enemy.TryGetComponent(out EnemyPatrolling patroller))
            {
                throw new System.NullReferenceException($"Enemy {enemy} has no EnemyPatroller component, but " +
                    $"it uses a PatrolBehaviour.");
            }
            
            void CleanUp()
            {

            }

            try
            {
                while (!ct.IsCancellationRequested)
                {
                    // Right
                    enemy.FacingDirection = 1;
                    await patroller.MoveToPatrolPoint(1, ct);
                    await Awaitable.WaitForSecondsAsync(patrolWait, ct);

                    // Left
                    enemy.FacingDirection = -1;
                    await patroller.MoveToPatrolPoint(-1, ct);
                    await Awaitable.WaitForSecondsAsync(patrolWait, ct);
                }
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