/*****************************************************************************
// File Name : RandomMovementBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 6/6/2026
// Last Modified : 6/6/2026
//
// Brief Description : Makes the enemy walk in a random direction for a certain amount of time.
*****************************************************************************/
using System;
using System.Threading;
using UnityEngine;

namespace Sodom.Enemies.AI
{
    [System.Serializable]
    public class RandomMovementBehavior : EnemyBehavior
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float wanderTime;

        protected override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            if (!enemy.TryGetComponent(out EnemyMovement movement))
            {
                throw new System.NullReferenceException($"Enemy {enemy} does not have a EnemyMovement component.");
            }
            ct.ThrowIfCancellationRequested();

            int direction = (UnityEngine.Random.Range(0, 2) * 2) - 1; 

            float timer = wanderTime;
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
                movement.SetDirection(direction);
                while (timer > 0)
                {
                    ct.ThrowIfCancellationRequested();
                    
                    if (EnemyMovement.CheckDestinationObscured(movement.TargetDirection < 0, movement.Edges))
                    {
                        // Immediately stop enemy velocity to stop.
                        movement.Rigidbody.linearVelocity = new Vector2(0, movement.Rigidbody.linearVelocityY);
                        break;
                    }
                    timer -= Time.fixedDeltaTime;
                    await Awaitable.FixedUpdateAsync(ct);
                }
                CleanUp();
            }
            catch (OperationCanceledException)
            {
                CleanUp();
                throw new OperationCanceledException();
            }
        }
    }

}