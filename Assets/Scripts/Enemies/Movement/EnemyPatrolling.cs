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
        private const float PATROL_AREA_GIZMO_OFFSET = 0.1f;

        [SerializeField, Tooltip("The total width that the enemy patrols around it's starting positition.  " +
            "Shown by the blue line at the enemy's feet.")] 
        private float patrolArea;
        [SerializeField, Tooltip("If true, enemy lineary velocity is reset when it detects a ledge or wall.")] 
        private bool immediateBrake;

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
            movement.SetMoveDirection(direction);

            while (!ct.IsCancellationRequested)
            {
                Vector2 toDest = destination - (Vector2)transform.position;
                // Stop patrolling once the point has been passed.
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
            if (movement != null)
            {
                Vector3 patrolOffset = (Vector3.right * patrolArea / 2);
                Vector3 posOffset = new Vector2(0, -movement.EnemyBounds.min.y + movement.EnemyBounds.center.y 
                    - movement.EnemyBounds.size.y) + (Vector2.up * PATROL_AREA_GIZMO_OFFSET);
                Gizmos.color = Color.blue;
                if (startPos == null)
                {
                    Gizmos.DrawLine(transform.position + posOffset - patrolOffset, 
                        transform.position + posOffset + patrolOffset);
                }
                else
                {
                    Gizmos.DrawLine((Vector3)startPos + posOffset - patrolOffset, 
                        (Vector3)startPos + posOffset + patrolOffset);
                }
            }
        }
    }

}