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

[System.Serializable]
public class PatrolState : EnemyBehavior
{
    [SerializeField] private float patrolWait;

    public override async Awaitable Run(EnemyController enemy, CancellationToken ct)
    {
        if (!enemy.TryGetComponent(out EnemyPatrolling patroller))
        {
            throw new System.NullReferenceException($"Enemy {enemy} has no EnemyPatroller component, but " +
                $"it uses a PatrolBehaviour.");
        }
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
    }
}
