/*****************************************************************************
// File Name : TeleportStabAttack.cs
// Author : 
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using System;
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public class TeleportStabAttack : EnemyAttack
    {
        [SerializeField] private GameObject hitbox;
        [SerializeField] private float attackTime;
        [SerializeField] private Vector2 teleportOffset;
        [SerializeField] private float postTeleportDelay;
        [SerializeField] private float stabbingLeapForce;
                
        public override async Awaitable PerformAttack(EnemyController enemy, GameObject target, CancellationToken ct)
        {
            if (!enemy.TryGetComponent(out EnemyMovement movement))
            {
                throw new System.NullReferenceException($"Enemy {enemy} does not have a EnemyMovement component.");
            }
            ct.ThrowIfCancellationRequested();
            // Get Components

            float originalGravity = movement.Rigidbody.gravityScale;
            void CleanUp()
            {
                // Reset to defaults
                movement.Rigidbody.gravityScale = originalGravity;
            }
    
            try
            {
                // Teleport behind the player.
                movement.Rigidbody.gravityScale = 0;
                movement.Rigidbody.position = (Vector2)target.transform.position + new Vector2(-target.transform.right.x * teleportOffset.x, teleportOffset.y);
                // Leap at the target and attack.
                enemy.PointTowardsTarget();
                await Awaitable.WaitForSecondsAsync(postTeleportDelay);

                // Leap at the target and attack.
                enemy.PointTowardsTarget();
                movement.Rigidbody.gravityScale = originalGravity;
                movement.Rigidbody.AddForce((target.transform.position - enemy.transform.position).normalized * stabbingLeapForce);
                hitbox.SetActive(true);
                await Awaitable.WaitForSecondsAsync(attackTime);
                hitbox.SetActive(false);

                // Wait until the enemy hits the ground.
                while(!movement.OnGround)
                {
                    await Awaitable.FixedUpdateAsync();
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