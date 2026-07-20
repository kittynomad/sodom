/*****************************************************************************
// File Name : RunningSliceAttack.cs
// Author : Arcadia Koederitz
// Creation Date : 6/20/2026
// Last Modified : 6/20/2026
//
// Brief Description : Controls the running slice attack of the red rooms stabby guy.
*****************************************************************************/
using CustomAttributes;
using System;
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    [DropdownGroup("Red Rooms/Stabby Guy")]
    public class RunningSliceAttack : EnemyAttack
    {
        [SerializeField] private GameObject hitbox;
        [SerializeField] private float attackTime;
        [SerializeField, Tooltip("Controls how close the enemy has to be to the player before it spawns the hitbox.")]
        private float attackRange;
        [Header("Charge")]
        [SerializeField] private float maxChargeTime;
        [SerializeField] private float chargeSpeed;
        [SerializeField] private float chargeWindupTime;
        [SerializeField] private BackdashBehavior hitBackdash;
        
        public override async Awaitable PerformAttack(EnemyController enemy, GameObject target, EnemyAttacker attackerComp, CancellationToken ct)
        {
            if (!enemy.TryGetComponent(out EnemyMovement movement))
            {
                throw new System.NullReferenceException($"Enemy {enemy} does not have a EnemyMovement component.");
            }

            ct.ThrowIfCancellationRequested();
            // Get Components

            float startingSpeed = movement.MoveSpeed;

            bool hitTarget = false;

            void CleanUp()
            {
                // Reset to defaults
                movement.MoveSpeed = startingSpeed;
                movement.SetMoveDirection(0);
                hitbox.SetActive(false);
                attackerComp.OnHitEvent -= HandleHit;
            }

            // When the enemy hits an enemy, flag it to backdash after the attack concludes.
            void HandleHit(IKillable hitObj, EnemyHitbox hitbox)
            {
                Debug.Log("Hit Player");
                hitTarget = true;
            }
    
            try
            {
                int attackDirection = enemy.DirectionToTarget;

                await Awaitable.WaitForSecondsAsync(chargeWindupTime, ct);

                // Immediately set the enemy to max speed after delay.
                movement.MoveSpeed = chargeSpeed;
                movement.SetMoveDirection(attackDirection);
                movement.Rigidbody.linearVelocityX = chargeSpeed * attackDirection;
                enemy.PointTowardsTarget();

                // Move until the player is passed.
                float timer = maxChargeTime;
                while(!ct.IsCancellationRequested 
                    && enemy.DirectionToTarget == attackDirection
                    && (maxChargeTime <= 0 || timer > 0))
                {
                    timer -= Time.fixedDeltaTime;
                    await Awaitable.FixedUpdateAsync();
                }

                attackerComp.OnHitEvent += HandleHit;
                ct.ThrowIfCancellationRequested();
                float attackTimer = attackTime;
                if (enemy.ToTarget.magnitude <= attackRange)
                {
                    // Perform the attack.
                    hitbox.SetActive(true);
                    while(!ct.IsCancellationRequested && attackTimer > 0 && !hitTarget)
                    {
                        attackTimer -= Time.fixedDeltaTime;
                        await Awaitable.FixedUpdateAsync(ct);
                    }
                    hitbox.SetActive(false);
                }
                attackerComp.OnHitEvent -= HandleHit;

                // Backdash if the enemy hit something.
                if (hitTarget)
                {
                    await hitBackdash.PerformBackdash(movement, -attackDirection, ct);
                }

                CleanUp();
            }
            catch (OperationCanceledException oce)
            {
                CleanUp();
                throw oce;
            }
    
        }
    }
}