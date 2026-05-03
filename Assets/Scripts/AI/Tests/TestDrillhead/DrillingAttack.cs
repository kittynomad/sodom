/*****************************************************************************
// File Name : DrillingAttack.cs
// Author : Arcadia Koederitz
// Creation Date : 5/3/2026
// Last Modified : 5/3/2026
//
// Brief Description : Test drilling underground attack for drillhead.
*****************************************************************************/
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class DrillingAttack : AttackBehavior
{
    [SerializeField] private float initialHopStrength;
    [SerializeField] private float drillTime;
    [SerializeField] private float drillSpeed;
    [SerializeField] private float drillTurnSpeed;
    [SerializeField] private float airTimeBeforeTransition;
    protected override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
    {
        // Get components
        if (!enemy.TryGetComponent(out EnemyMovement movement))
        {
            throw new System.NullReferenceException($"Enemy {enemy} does not have a EnemyMovement component.");
        }
        if (!enemy.TryGetComponent(out GroundDetector gDetector))
        {
            throw new System.NullReferenceException($"Enemy {enemy} does not have a GroundDetector component.");
        }
        if (!enemy.TryGetComponent(out CapsuleCollider2D collisionBox))
        {
            throw new System.NullReferenceException($"Enemy {enemy} does not have a CapsuleCollider component.");
        }
        Rigidbody2D rb = movement.RB;
        float baseGravity = rb.gravityScale;

        // Clean up function.
        void CleanUp()
        {
            collisionBox.isTrigger = false;
            movement.RB.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = baseGravity;
        }

        try
        {
            // Body
            collisionBox.isTrigger = true;

            rb.AddForce(Vector2.up * initialHopStrength, ForceMode2D.Impulse);
            rb.constraints = RigidbodyConstraints2D.None;

            float timer = drillTime;
            while(timer > 0 || gDetector.InGround)
            {
                ct.ThrowIfCancellationRequested();

                if (gDetector.InGround)
                {
                    float targetRotation = Mathf.Atan2(enemy.ToTarget.y, enemy.ToTarget.x) * Mathf.Rad2Deg;
                    float rotation = Mathf.MoveTowardsAngle(rb.rotation, targetRotation, drillTurnSpeed);

                    // Move velocity towards direction of player.
                    Vector2 direction = new Vector2(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad)).normalized;
                    rb.rotation = rotation;
                    rb.linearVelocity = direction * drillSpeed;
                    rb.gravityScale = 0;
                }
                else
                {
                    rb.gravityScale = baseGravity;
                    // Freefall while outside of ground.
                }

                // Rotate to face the direction of movement.
                rb.rotation = Mathf.Atan2(rb.linearVelocityX, rb.linearVelocityY) * Mathf.Rad2Deg;

                timer -= Time.fixedDeltaTime;
                await Awaitable.FixedUpdateAsync(ct);
            }

            await Awaitable.WaitForSecondsAsync(airTimeBeforeTransition, ct);

            // Wait until enemy hits ground in full game.

            await Awaitable.WaitForSecondsAsync(postAttackDelay, ct);

            CleanUp();
        }
        catch (OperationCanceledException e)
        {
            CleanUp();
            throw e;
        }
    }
}
