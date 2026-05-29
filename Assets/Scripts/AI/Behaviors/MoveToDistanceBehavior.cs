/*****************************************************************************
// File Name : MoveToDistanceBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Moves the enemy to be within a certain distance range of it's target.
*****************************************************************************/
using CustomAttributes;
using System;
using System.Threading;
using UnityEngine;

namespace Sodom.Enemies.AI
{
    [System.Serializable]
    public class MoveToDistanceBehavior : EnemyBehavior
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float acceleration;
        [SerializeField, VectorLabels("Min", "Max")] private Vector2 distanceRange;
        [SerializeField] private bool stopAtEdge;
        [SerializeField] private bool hasMaxTime;
        [field: SerializeField] public float MaxTime { get; set; }

        protected override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            if (!enemy.TryGetComponent(out EnemyMovement movement))
            {
                throw new System.NullReferenceException($"Enemy {enemy} does not have a EnemyMovement component.");
            }

            Vector2 toTarget = enemy.Target.transform.position - enemy.transform.position;
            float timer = MaxTime;
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
            // Continually move to keep the ideal distance until the next update.
            try
            {
                while (!IsWithinRange(toTarget.magnitude) 
                    && (timer > 0 || !hasMaxTime))
                {
                    ct.ThrowIfCancellationRequested();
                    toTarget = enemy.Target.transform.position - enemy.transform.position;
                    // Enemy too close
                    if (toTarget.magnitude < distanceRange.x)
                    {
                        movement.SetDirection(-(int)Mathf.Sign(toTarget.x));
                    }
                    // Enemy too far.
                    else
                    {
                        movement.SetDirection((int)Mathf.Sign(toTarget.x));
                    }

                    if (stopAtEdge && EnemyMovement.CheckDestinationObscured(movement.TargetDirection < 0, movement.Edges))
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

        /// <summary>
        /// Checks if the distance from from the target is within the acceptable range.
        /// </summary>
        /// <param name="distance">The distance from the target.</param>
        /// <returns>True if the distance is within range, false if otherwise.</returns>
        public bool IsWithinRange(float distance)
        {
            return distance < distanceRange.y && distance > distanceRange.x;
        }
    }

}