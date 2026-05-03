/*****************************************************************************
// File Name : ChargeAttackBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 5/3/2026
// Last Modified : 5/3/2026
//
// Brief Description : Test chjarging attack.
*****************************************************************************/
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class ChargeAttackBehavior : AttackBehavior
{
    [SerializeField] private float chargeForce;
    [SerializeField] private float preChargeDelay;
    [SerializeField] private float followThroughDelay;
    protected override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
    {
        // Get components
        if (!enemy.TryGetComponent(out EnemyMovement movement))
        {
            throw new System.NullReferenceException($"Enemy {enemy} does not have a EnemyMovement component.");
        }

        // Clean up function.
        void CleanUp()
        {
            
        }

        try
        {
            // Body
            enemy.PointTowardsTarget();
            await Awaitable.WaitForSecondsAsync(preChargeDelay, ct);

            // Show the hitbox.
            Transform hitbox = enemy.transform.GetChild(1);
            hitbox.gameObject.SetActive(true);

            enemy.PointTowardsTarget();
            // Continually chage while the direction to the target is the same.
            int direction = (int)Mathf.Sign(enemy.ToTarget.x);
            while (direction == (int)Mathf.Sign(enemy.ToTarget.x))
            {
                movement.RB.AddForce(direction * chargeForce * Vector2.right);
                await Awaitable.FixedUpdateAsync(ct);
            }

            float timer = followThroughDelay;
            while(timer > 0)
            {
                timer -= Time.fixedDeltaTime;
                await Awaitable.FixedUpdateAsync(ct);
            }

            hitbox.gameObject.SetActive(false);

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
