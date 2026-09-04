/*****************************************************************************
// File Name : RunningSliceAttack.cs
// Author : Arcadia Koederitz
// Creation Date : 6/20/2026
// Last Modified : 6/20/2026
//
// Brief Description : Controls the running slice attack of the red rooms Painter.
*****************************************************************************/
using CustomAttributes;
using System;
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    [DropdownGroup("Red Rooms/Painter")]
    public class RunningSliceAttack : EnemyAttack
    {
        //[SerializeField] private GameObject hitbox;
        //[SerializeField] private float attackTime;
        [SerializeField, Tooltip("Controls how close the enemy has to be to the player before it spawns the hitbox.")]
        private float attackRange;
        [Header("Charge")]
        [SerializeField, Tooltip("The amount of time the enemy charges before before stopping when passing the player.")]
        private float minChargeTime;
        [SerializeField, Tooltip("The max amount of time the enemy can charge before being forced to stop.")] 
        private float maxChargeTime;
        [SerializeField] private float chargeSpeed;
        [Header("Animation")]
        [SerializeField] private string windupAnimationState;
        [SerializeField] private string sliceAnimationState;
        [Header("Backdash On Hit")]
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

                // Play windup animation.
                enemy.PlayAnimation(windupAnimationState);
                await AIUtilities.AwaitAnimation(enemy.Animator, ct);

                // Immediately set the enemy to max speed after delay.
                movement.MoveSpeed = chargeSpeed;
                movement.SetMoveDirection(attackDirection);
                movement.Rigidbody.linearVelocityX = chargeSpeed * attackDirection;
                enemy.PointTowardsTarget();

                // Move until the player is passed.
                float timer = 0;
                while(!ct.IsCancellationRequested && (timer < minChargeTime 
                    || (enemy.DirectionToTarget == attackDirection && timer < maxChargeTime)))
                {
                    timer += Time.fixedDeltaTime;
                    await Awaitable.FixedUpdateAsync(ct);
                }

                ct.ThrowIfCancellationRequested();

                // Spawn the attack hitbox.
                attackerComp.OnHitEvent += HandleHit;
                //float attackTimer = attackTime;
                //if (enemy.ToTarget.magnitude <= attackRange)
                //{
                //    hitbox.SetActive(true);
                //    while(!ct.IsCancellationRequested && attackTimer > 0 && !hitTarget)
                //    {
                //        attackTimer -= Time.fixedDeltaTime;
                //        await Awaitable.FixedUpdateAsync(ct);
                //    }
                //    hitbox.SetActive(false);
                //}
                enemy.PlayAnimation(sliceAnimationState);
                await AIUtilities.AwaitAnimation(enemy.Animator, ct);

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