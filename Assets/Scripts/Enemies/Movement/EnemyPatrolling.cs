/*****************************************************************************
// File Name : EnemyPatrolling.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Handles settings for an enemy's patrol route.
*****************************************************************************/
using CustomAttributes;
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies
{
    [RequireComponent(typeof(EnemyMovement))]
    public class EnemyPatrolling : MonoBehaviour
    {
        [SerializeField] private float patrolArea;
        [SerializeField] private bool immediateBrake;

        // Components
        [SerializeField, ShowIfNull] private EnemyMovement movement;

        private Vector2? startPos;

        private void Reset()
        {
            movement = GetComponent<EnemyMovement>();
        }

        private void Awake()
        {
            movement.OnGroundEvent += SetStartPos;
        }

        private void OnDestroy()
        {
            movement.OnGroundEvent -= SetStartPos;
        }

        /// <summary>
        /// Sets the start position of the enemy's patrol when they hit the ground for the first time.
        /// </summary>
        /// <param name="onGround"></param>
        private void SetStartPos(bool onGround)
        {
            startPos = transform.position;
            movement.OnGroundEvent -= SetStartPos;
        }

        /// <summary>
        /// Asyncronously moves the enemy to the next point in it's patrol route.
        /// </summary>
        /// <returns></returns>
        public async Awaitable MoveToPatrolPoint(int direction, CancellationToken ct)
        {
            // Wait until a start point has been set.
            while (startPos == null)
            {
                await Awaitable.FixedUpdateAsync(ct);
            }

            direction = Mathf.Clamp(direction, -1, 1);

            // Get the position of our destination.
            Vector2 destination = GetDestination(direction);

            while (!ct.IsCancellationRequested)
            {
                Vector2 toDest = destination - (Vector2)transform.position;
                // Stop patrolling once the point has been passed.
                Debug.Log(toDest.x);
                if (Mathf.Abs(toDest.y) < 0.5f && toDest.x * (direction) < 0)
                {
                    break;
                }
                else if (movement.IsDestinationObscured())
                {
                    if (immediateBrake)
                    {
                        movement.StopVelocity();
                    }
                    break;
                }
                else
                {
                    //movement.SetDirection((int)Mathf.Sign(toDest.x));
                    movement.SetMoveDirection(direction);
                }
                await Awaitable.FixedUpdateAsync();
            }

            movement.SetMoveDirection(0);
            ct.ThrowIfCancellationRequested();
        }

        private Vector2 GetDestination(int direction)
        {
            if (startPos == null) {  return Vector2.zero; }
            return (Vector2)startPos + (Vector2.right * patrolArea / 2) * direction;
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 patrolOffset = (Vector3.right * patrolArea / 2);
            Gizmos.color = Color.blue;
            if (startPos == null)
            {
                Gizmos.DrawLine(transform.position - patrolOffset, transform.position + patrolOffset);
            }
            else
            {
                Gizmos.DrawLine((Vector3)startPos - patrolOffset, (Vector3)startPos + patrolOffset);
            }
        }
    }

}