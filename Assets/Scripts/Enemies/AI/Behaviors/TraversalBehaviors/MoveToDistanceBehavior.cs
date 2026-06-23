/*****************************************************************************
// File Name : MoveToDistanceBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Moves the enemy to be within a certain distance range of it's target.
*****************************************************************************/
using CustomAttributes;
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public class MoveToDistanceBehavior : TraversalBehavior
    {
        [SerializeField, VectorLabels("Min", "Max")] private Vector2 distanceRange;
        //[SerializeField] private bool hasMaxTime;
        //[field: SerializeField] public float MaxTime { get; set; }

        protected override async Awaitable RunMovement(EnemyMovement movement, EnemyController enemy, CancellationToken ct)
        {
            Vector2 toTarget = enemy.Target.transform.position - enemy.transform.position;
            while (!IsWithinRange(toTarget.magnitude))
            //&& (timer > 0 || !hasMaxTime))
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

                // Stopping at edge handling.
                if (await CheckBlockedJump(movement, ct))
                {
                    break;
                }

                //timer -= Time.fixedDeltaTime;
                await Awaitable.FixedUpdateAsync(ct);
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