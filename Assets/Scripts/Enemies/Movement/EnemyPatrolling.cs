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
        public async Awaitable MoveToPatrolPoint(bool isLeft, CancellationToken ct)
        {

            // Get the position of our destination.
            Vector2 destination = GetDestination(isLeft);

            while (!ct.IsCancellationRequested)
            {
                Vector2 toDest = destination - (Vector2)transform.position;
                // Stop patrolling once the point has been passed.
                if (Mathf.Abs(toDest.y) < 0.5f && toDest.x * (isLeft ? -1 : 1) < 0)
                {
                    break;
                }
                else if (await movement.IsObscuredTryJump(ct))
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
                    movement.SetMoveDirection(isLeft ? -1 : 1);
                }
                await Awaitable.FixedUpdateAsync();
            }

            movement.SetMoveDirection(0);
            ct.ThrowIfCancellationRequested();
        }

        private Vector2 GetDestination(bool isLeft)
        {
            if (startPos == null) {  return Vector2.zero; }
            return (Vector2)startPos + (Vector2.right * patrolArea / 2) * (isLeft ? -1 : 1);
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