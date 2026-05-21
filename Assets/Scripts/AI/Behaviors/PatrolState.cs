/*****************************************************************************
// File Name : PatrolState.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Controls the enemy while patrolling.
*****************************************************************************/
using System.Threading;
using UnityEngine;

namespace Sodom.Enemies.AI
{
    [System.Serializable]
    public class PatrolState : EnemyBehavior
    {
        [SerializeField] private Color debugColor;
        [SerializeField] private float patrolWait;

        protected override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            if (enemy.TryGetComponent(out SpriteRenderer rend))
            {
                rend.color = debugColor;
            }
            if (!enemy.TryGetComponent(out EnemyPatrolling patroller))
            {
                throw new System.NullReferenceException($"Enemy {enemy} has no EnemyPatroller component, but " +
                    $"it uses a PatrolBehaviour.");
            }
            while (!ct.IsCancellationRequested)
            {
                // TODO: Update so it stops at edges.

                // Right
                enemy.SetRotation(false);
                await patroller.MoveToPatrolPoint(false, ct);
                await Awaitable.WaitForSecondsAsync(patrolWait, ct);

                // Left
                enemy.SetRotation(true);
                await patroller.MoveToPatrolPoint(true, ct);
                await Awaitable.WaitForSecondsAsync(patrolWait, ct);
            }
        }
    }

}