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

namespace Sodom.Enemies.AI
{
    [System.Serializable]
    public class PatrolState : EnemyState
    {
        [SerializeField] private float patrolWait;

        protected override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
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
                    enemy.SetRotation(false);
                    await patroller.MoveToPatrolPoint(false, ct);
                    await Awaitable.WaitForSecondsAsync(patrolWait, ct);

                    // Left
                    enemy.SetRotation(true);
                    await patroller.MoveToPatrolPoint(true, ct);
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