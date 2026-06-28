/*****************************************************************************
// File Name : TeleportStabAttack.cs
// Author : Arcadia Koederitz
// Creation Date : 6/20/2026
// Last Modified : 6/20/2026
//
// Brief Description : Teleport attack for the red rooms stabby guy.
*****************************************************************************/
using CustomAttributes;
using NaughtyAttributes;
using System;
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    [DropdownGroup("Red Rooms/Stabby Guy")]
    public class TeleportStabAttack : EnemyAttack
    {
        [SerializeField] private GameObject hitbox;
        [SerializeField] private float hitboxOffset;
        [SerializeField] private float attackTime;
        [SerializeField] private float stabbingLeapForce;
        [SerializeField] private float stabYVelocity;
        [Header("Timing")]
        [SerializeField] private float decideDirectionDelay;
        [SerializeField] private float leapDelay;
        [Header("Teleporting")]
        [SerializeField] private Vector2 teleportOffset;
        [SerializeField, Tooltip("The set of points around the scene that the enemy should teleport to.  " +
    "If none are set, it will teleport behind the player.")]
        private Transform[] manualTeleportPoints;
                
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
                hitbox.SetActive(false);
            }
    
            try
            {
                // Teleport behind the player.
                movement.Rigidbody.gravityScale = 0;
                movement.Rigidbody.position = GetTeleportPosition(target);
                movement.Rigidbody.linearVelocity = Vector2.zero;

                // Leap at the target and attack.
                await Awaitable.FixedUpdateAsync(ct);
                enemy.PointTowardsTarget();

                await Awaitable.WaitForSecondsAsync(decideDirectionDelay, ct);

                Vector2 stabVector = GetStabDirection(enemy.transform.position, target.transform.position);
                Debug.DrawLine(movement.transform.position, movement.transform.position + (Vector3)stabVector, Color.yellow, 5f);

                await Awaitable.WaitForSecondsAsync(leapDelay, ct);

                // Leap at the target and attack.
                enemy.PointTowardsTarget();
                hitbox.transform.position = enemy.transform.position + (Vector3)(stabVector * hitboxOffset);
                movement.Rigidbody.linearVelocity = stabVector * stabbingLeapForce;
                hitbox.SetActive(true);
                await Awaitable.WaitForSecondsAsync(attackTime, ct);
                hitbox.SetActive(false);
                movement.Rigidbody.gravityScale = originalGravity;

                // Wait until the enemy hits the ground.
                while (!movement.OnGround)
                {
                    ct.ThrowIfCancellationRequested();
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

        private Vector2 GetTeleportPosition(GameObject target)
        {
            if (manualTeleportPoints != null && manualTeleportPoints.Length > 0)
            {
                // Gets the closest teleport point to the target.
                float distToTarget = Vector3.Distance(manualTeleportPoints[0].position, target.transform.position);
                int targetIndex = 0;
                for (int i = 1; i < manualTeleportPoints.Length; i++)
                {
                    if (manualTeleportPoints[i] == null) { continue; }
                    float dist = Vector3.Distance(manualTeleportPoints[i].position, target.transform.position);
                    if (dist <  distToTarget)
                    {
                        distToTarget = dist;
                        targetIndex = i;
                    }
                }
                return manualTeleportPoints[targetIndex].position + (Vector3)teleportOffset;
            }
            return (Vector2)target.transform.position + new Vector2(-GetTargetFacingDirection(target) * teleportOffset.x, teleportOffset.y);
        }

        /// <summary>
        /// Gets a direction to stab in constrained to cardinal directions + diagonal.
        /// </summary>
        /// <param name="enemyPosition"></param>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        private Vector3 GetStabDirection(Vector3 enemyPosition, Vector3 targetPosition)
        {
            Vector2 direction = (targetPosition - enemyPosition).normalized;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            float restrictedAngle = Mathf.Round(angle / 45) * 45;
            Vector2 restrictedDirection = new Vector2(Mathf.Sin(restrictedAngle * Mathf.Deg2Rad), Mathf.Cos(restrictedAngle * Mathf.Deg2Rad)).normalized;
            return  restrictedDirection;
        }

        /// <summary>
        /// This needs to get replaced at a later date when the player/enemies have proper direction tracking.
        /// </summary>
        /// <param name="playerobj"></param>
        /// <returns></returns>
        private static int GetTargetFacingDirection(GameObject playerobj)
        {
            SpriteRenderer[] spriteRends = playerobj.GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer spriter in spriteRends)
            {
                if (spriter.gameObject.name == "PlayerSprite")
                {
                    return spriter.flipX ? -1 : 1;
                }
            }
            return 1;
        }
    }
}