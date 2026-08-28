/*****************************************************************************
// File Name : MoveTowardsTarget.cs
// Author : 
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;
using System.Threading;
using System;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public class MoveTowardsTarget : TraversalBehavior
    {
        [SerializeField, Tooltip("How long the enemy moves towards the target for.")] private float moveTime;
        /// <summary>
        /// Moves the enemy towards the player
        /// </summary>
        /// <param name="movement"></param>
        /// <param name="enemy"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected override async Awaitable RunMovement(EnemyMovement movement, EnemyController enemy, CancellationToken ct)
        {
            float timer = moveTime;
            while (timer > 0)
            //&& (timer > 0 || !hasMaxTime))
            {
                ct.ThrowIfCancellationRequested();
                enemy.PointTowardsTarget();
                movement.SetMoveDirection((int)Mathf.Sign(enemy.ToTarget.x));

                // Stopping at edge handling.
                if (await CheckBlockedJump(movement, ct))
                {
                    break;
                }

                timer -= Time.fixedDeltaTime;
                await Awaitable.FixedUpdateAsync(ct);
            }
        }
    }
}