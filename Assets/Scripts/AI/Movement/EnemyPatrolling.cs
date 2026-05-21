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

namespace Sodom.Enemies
{
    [RequireComponent(typeof(EnemyMovement))]
    public class EnemyPatrolling : MonoBehaviour
    {
        [SerializeField] private float patrolArea;

        // Components
        [SerializeField, ShowIfNull] private EnemyMovement movement;

        private Vector2 startPos;

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
                // Worry about pathfinding later.
                // Stop patrolling once the point has been reached.
                if (Mathf.Abs(toDest.x) < 0.5f || CheckDestinationObscured(isLeft, movement.Edges))
                {
                    break;
                }
                else
                {
                    //movement.SetDirection((int)Mathf.Sign(toDest.x));
                    movement.SetDirection(isLeft ? -1 : 1);
                }
                await Awaitable.FixedUpdateAsync();
            }

            movement.SetDirection(0);
            ct.ThrowIfCancellationRequested();
        }

        private Vector2 GetDestination(bool isLeft)
        {
            return (Vector2)startPos + (Vector2.right * patrolArea / 2) * (isLeft ? -1 : 1);
        }

        private bool CheckDestinationObscured(bool isLeft, EnemyMovement.DetectedEdges edges)
        {
            return isLeft ? EnemyMovement.CheckLeft(edges) : EnemyMovement.CheckRight(edges);
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 patrolOffset = (Vector3.right * patrolArea / 2);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position - patrolOffset, transform.position + patrolOffset);
        }
    }

}