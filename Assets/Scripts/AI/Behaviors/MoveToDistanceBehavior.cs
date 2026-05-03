/*****************************************************************************
// File Name : MoveToDistanceBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Moves the enemy to be within a certain distance range of it's target.
*****************************************************************************/
using System;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class MoveToDistanceBehavior : EnemyBehavior
{
    [SerializeField, Tooltip("X is min, Y is max.")] private Vector2 distanceRange;
    [SerializeField] private bool hasMaxTime;
    [field: SerializeField] public float MaxTime { get; set; }

    protected override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
    {
        if (!enemy.TryGetComponent(out EnemyMovement movement))
        {
            throw new System.NullReferenceException($"Enemy {enemy} does not have a EnemyMovement component.");
        }

        void CleanUp()
        {
            movement.SetDirection(0);
        }

        Vector2 toTarget = enemy.Target.transform.position - enemy.transform.position;
        float timer = MaxTime;
        // Continually move to keep the ideal distance until the next update.
        try
        {
            while (!IsWithinRange(toTarget.magnitude) && (timer > 0 || !hasMaxTime))
            {
                ct.ThrowIfCancellationRequested();
                toTarget = enemy.Target.transform.position - enemy.transform.position;
                // Enemy too close
                if (toTarget.magnitude < distanceRange.x)
                {
                    //Debug.Log("Too Close");
                    movement.SetDirection(-(int)Mathf.Sign(toTarget.x));
                }
                // Enemy too far.
                else
                {
                    //Debug.Log("Too Far");
                    movement.SetDirection((int)Mathf.Sign(toTarget.x));
                }
                timer -= Time.fixedDeltaTime;
                await Awaitable.FixedUpdateAsync(ct);
            }
        }
        catch (OperationCanceledException)
        {
            CleanUp();
            throw new OperationCanceledException();
        }

        CleanUp();
    }

    public bool IsWithinRange(float distance)
    {
        return distance < distanceRange.y && distance > distanceRange.x;
    }
}
