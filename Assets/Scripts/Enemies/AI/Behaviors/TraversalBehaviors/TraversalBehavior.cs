/*****************************************************************************
// File Name : TraversalBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 6/7/2026
// Last Modified : 6/7/2026
//
// Brief Description : Base class for any enemy behavior that controls the enemy's grounded movement, such as 
// walking/running up to the player.
*****************************************************************************/
using NaughtyAttributes;
using System;
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public abstract class TraversalBehavior : EnemyBehavior
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] protected bool stopAtEdge;
        [SerializeField, ShowIf(nameof(stopAtEdge)), AllowNesting] protected bool tryJump;

        /// <summary>
        /// Use Run as the wrapper function for setting and resetting speed so that any derived behaviors can use RunAI like normal.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            if (!enemy.TryGetComponent(out EnemyMovement movement))
            {
                throw new System.NullReferenceException($"Enemy {enemy} does not have a EnemyMovement component.");
            }

            float originalSpeed = movement.MoveSpeed;
            float originalAcceleration = movement.Acceleration;
            void CleanUp()
            {
                movement.SetDirection(0);
                movement.MoveSpeed = originalSpeed;
                movement.Acceleration = originalAcceleration;
            }

            movement.MoveSpeed = moveSpeed;
            movement.Acceleration = acceleration;
            try
            {
                await RunMovement(movement, enemy, ct);
                CleanUp();
            }
            catch (OperationCanceledException oce)
            {
                CleanUp();
                throw oce;
            }
    
        }

        /// <summary>
        /// Another level of RunAI that specifically controls the way the enemy moves.
        /// </summary>
        /// <remarks>
        /// Sorry this is so many levels deep.
        /// </remarks>
        /// <param name="movement"></param>
        /// <param name="enemy"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected abstract Awaitable RunMovement(EnemyMovement movement, EnemyController enemy, CancellationToken ct);

        /// <summary>
        /// Stops this enemy at the edge if they've reached an edge.
        /// </summary>
        /// <param name="movement">The movement component on the enemy.</param>
        /// <returns>True if th eenemy has reached an edge.  Use for breaking loops.</returns>
        protected bool StopWhenBlocked(EnemyMovement movement)
        {
            if (stopAtEdge && movement.IsDestinationObscured())
            {
                movement.StopHorizontalVelocty();
                return true;
            }
            return false;
        }

        protected async Awaitable<bool> CheckBlockedJump(EnemyMovement movement, CancellationToken ct)
        {
            if (tryJump)
            {
                if (stopAtEdge && await movement.IsObscuredTryJump(ct))
                {
                    movement.StopHorizontalVelocty();
                    return true;
                }
            }
            else
            {
                return StopWhenBlocked(movement);
            }
            
            return false;
        }
    }
}