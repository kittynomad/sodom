/*****************************************************************************
// File Name : MaintainDistanceBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Makes the enemy attempt to continually maintain a certain distance from the target.
*****************************************************************************/
using System;
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public class MaintainDistanceBehavior : EnemyBehavior
    {
        [SerializeField] private MoveToDistanceBehavior moveToDistance; // Sub-behavior so that they can be reused.
        [SerializeField] private float maintainTime;
        [SerializeField] private float updatePeriod;

        public override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            int numUpdates = (int)(maintainTime / updatePeriod);

            CancellationTokenSource subCts = new CancellationTokenSource();

            void CleanUp()
            {
                subCts.Cancel();
            }

            Awaitable moveAwaitable = null;
            try
            {
                for (int i = 0; i < numUpdates && !ct.IsCancellationRequested; i++)
                {
                    if (enemy.Target == null) { continue; }
                    Vector2 toTarget = enemy.Target.transform.position - enemy.transform.position;

                    // Update rotation.
                    enemy.PointTowardsTarget();

                    // Control movement.
                    if (!moveToDistance.IsWithinRange(toTarget.magnitude) &&
                        (moveAwaitable == null || moveAwaitable.IsCompleted))
                    {
                        moveAwaitable = moveToDistance.RunAI(enemy, subCts.Token);
                    }

                    await Awaitable.WaitForSecondsAsync(updatePeriod, ct);
                }
            }
            catch (OperationCanceledException)
            {
                CleanUp();
                throw new OperationCanceledException();
            }
            CleanUp();
        }

        /// <summary>
        /// Checks if the enemy is currently within range of their target.
        /// </summary>
        /// <param name="distance"></param>
        public bool IsWithinRange(float distance)
        {
            return moveToDistance.IsWithinRange(distance);
        }
    }

}