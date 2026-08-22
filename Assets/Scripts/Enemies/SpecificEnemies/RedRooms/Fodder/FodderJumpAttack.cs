/*****************************************************************************
// File Name : FodderJumpAttack.cs
// Author : Arcadia Koederitz
// Creation Date : 8/22/2026
// Last Modified : 8/22/2026
//
// Brief Description : Jump attack for the red rooms fodder enemy.
*****************************************************************************/
using CustomAttributes;
using System;
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    [DropdownGroup("Red Rooms/Fodder")]
    public class FodderJumpAttack : EnemyAttack
    {
        [SerializeField, Tooltip("The hitbox to use for the jump attack.")] 
        private GameObject hitbox;
        [SerializeField, Tooltip("Name of the animation state that plays this attack's animation.")] 
        private string animationStateName;
        [SerializeField, Tooltip("Name of the animation signal for when the enemy performs the jump.")]
        private string jumpSignal;
        [SerializeField, Tooltip("How high the enemy can jump.")]
        private float jumpStrength;
        [SerializeField, Tooltip("How much damage this enemy does to itself when landing from the jump.")] 
        private int selfDamage;

        public override async Awaitable PerformAttack(EnemyController enemy, GameObject target, EnemyAttacker attackerComp, CancellationToken ct)
        {
            if (!enemy.TryGetComponent(out EnemyMovement movement))
            {
                throw new System.NullReferenceException($"Enemy {enemy} does not have a EnemyMovement component.");
            }
            ct.ThrowIfCancellationRequested();
            // Get Components
    
            void CleanUp()
            {
                // Reset to defaults
                hitbox.SetActive(false);
            }
    
            try
            {
                // Behavior Logic.
                enemy.PlayAnimation(animationStateName);

                await AIUtilities.AwaitSignal(jumpSignal, enemy, ct);
                hitbox.SetActive(true);

                // Apply Jump Velocity.
                movement.Rigidbody.linearVelocity = 
                    GetJumpVelocity(jumpStrength, target.transform.position, movement.GetFeetPosition(), movement.Rigidbody.gravityScale);

                // Wait until the enemy hits the ground.
                await Awaitable.WaitForSecondsAsync(0.1f); // Buffer to prevent instant landing.
                while(!movement.OnGround)
                {
                    await Awaitable.FixedUpdateAsync(ct);
                }

                hitbox.SetActive(false);

                // Do stuff when the enemy lands, like self damage.
                if (enemy.TryGetComponent(out EnemyHealth health))
                {
                    health.OnDamage(selfDamage);
                }

                CleanUp();
            }
            catch (OperationCanceledException oce)
            {
                CleanUp();
                throw oce;
            }
    
        }

        /// <summary>
        /// Utilizes the formula for finding the angle of a projectile based on initial speed and position.
        /// </summary>
        /// <param name="verticalSpeed">The vertical speed of the projectile..</param>
        /// <param name="targetPosition">The target position.</param>
        /// <param name="shotPosition">The position from which the projectile will be shot.</param>
        /// <param name="gravityScale">The gravity scale of the projectile.</param>
        /// <returns>The vector that the projectiles hould be shot at to hit the target.</returns>
        private static Vector2 GetJumpVelocity(float verticalSpeed, Vector2 targetPosition, Vector2 shotPosition, float gravityScale)
        {
            Vector2 deltaPosition = targetPosition - shotPosition;
            float gravity = gravityScale * Physics2D.gravity.y;

            float travelTime = (verticalSpeed + Mathf.Sqrt(Mathf.Abs(Mathf.Pow(verticalSpeed, 2) + 2 * gravity * deltaPosition.y))) / gravity;
            float horizontalSpeed = deltaPosition.x / travelTime;

            return new Vector2(-horizontalSpeed, verticalSpeed);
        }
    }
}