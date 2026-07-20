/*****************************************************************************
// File Name : BackdashBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 5/19/2026
// Last Modified : 5/19/2026
//
// Brief Description : Behavior for enemies to backdash away from the player.
*****************************************************************************/
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [Serializable]
    public class BackdashBehavior : EnemyBehavior
    {
        [SerializeField, Tooltip("Controls the horizontal speed of the enemy during the backdash.")] 
        private float dashForce;
        [SerializeField, Tooltip("Controls how high the enemy jumps during the backdash.")] 
        private float dashJump;
        [SerializeField, Tooltip("How long to wait after backdashing before continuing.")] 
        private float postDashDelay;

        public override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            if (!enemy.TryGetComponent(out EnemyMovement movement))
            {
                throw new System.NullReferenceException($"Enemy {enemy} does not have a EnemyMovement component.");
            }

            void CleanUp()
            {
                
            }

            try
            {
                enemy.PointTowardsTarget();

                await PerformBackdash(movement, -Mathf.Sign(enemy.FacingDirection), ct);
                CleanUp();
            }
            catch (OperationCanceledException oce)
            {
                CleanUp();
                throw oce;
            }

        }

        public async Awaitable PerformBackdash(EnemyMovement movement, float direction, CancellationToken ct)
        {
            Vector2 jumpBackForce = new Vector2(direction * dashForce, dashJump);

            // TODO: Switch this to linearVelocity
            movement.Rigidbody.linearVelocity = jumpBackForce;

            await Awaitable.WaitForSecondsAsync(postDashDelay, ct);
        }
    }

}