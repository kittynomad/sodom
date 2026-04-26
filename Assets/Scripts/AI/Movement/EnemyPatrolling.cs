/*****************************************************************************
// File Name : EnemyPatrolling.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Handles settings for an enemy's patrol route.
*****************************************************************************/
using System.Threading;
using UnityEngine;

public class EnemyPatrolling : MonoBehaviour
{
    [SerializeField] private EnemyMovement movement;
    [SerializeField] private float patrolArea;

    private Vector2 startPos;

    private void Awake()
    {
        startPos = transform.position;
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
            if (Mathf.Abs(toDest.x) < 0.5f)
            {
                break;
            }
            else
            {
                movement.SetDirection((int)Mathf.Sign(toDest.x));
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

    private void OnDrawGizmosSelected()
    {
        Vector3 patrolOffset = (Vector3.right * patrolArea / 2);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position - patrolOffset, transform.position + patrolOffset);
    }
}
