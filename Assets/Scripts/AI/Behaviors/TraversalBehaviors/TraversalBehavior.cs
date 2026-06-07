/*****************************************************************************
// File Name : TraversalBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 6/7/2026
// Last Modified : 6/7/2026
//
// Brief Description : Base class for any enemy behavior that controls the enemy's grounded movement, such as 
// walking/running up to the player.
*****************************************************************************/
using UnityEngine;
using System.Threading;
using System;

namespace Sodom.Enemies.AI
{
    [System.Serializable]
    public abstract class TraversalBehavior : EnemyBehavior
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] protected bool stopAtEdge;

        /// <summary>
        /// Use Run as the wrapper function for setting and resetting speed so that any derived behaviors can use RunAI like normal.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException"></exception>
        protected override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
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
        protected bool StopAtEdge(EnemyMovement movement)
        {
            if (stopAtEdge && EnemyMovement.CheckDestinationObscured(movement.TargetDirection < 0, movement.Edges))
            {
                // Immediately stop enemy velocity to stop.
                movement.Rigidbody.linearVelocity = new Vector2(0, movement.Rigidbody.linearVelocityY);
                return true;
            }
            return false;
        }
    }
}