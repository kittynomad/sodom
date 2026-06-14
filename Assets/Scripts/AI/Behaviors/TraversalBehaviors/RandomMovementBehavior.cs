/*****************************************************************************
// File Name : RandomMovementBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 6/6/2026
// Last Modified : 6/6/2026
//
// Brief Description : Makes the enemy walk in a random direction for a certain amount of time.
*****************************************************************************/
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public class RandomMovementBehavior : TraversalBehavior
    {
        [SerializeField] private float wanderTime;

        /// <summary>
        /// Chooses a random direction and makes the enemy move in that direction for the specified wander time.
        /// </summary>
        /// <param name="movement"></param>
        /// <param name="enemy"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected override async Awaitable RunMovement(EnemyMovement movement, EnemyController enemy, CancellationToken ct)
        {
            int direction = (UnityEngine.Random.Range(0, 2) * 2) - 1;

            float timer = wanderTime;
            movement.SetDirection(direction);
            while (timer > 0)
            {
                ct.ThrowIfCancellationRequested();

                if (StopAtEdge(movement))
                {
                    break;
                }
                timer -= Time.fixedDeltaTime;
                await Awaitable.FixedUpdateAsync(ct);
            }
        }
    }

}