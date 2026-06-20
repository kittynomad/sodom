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
        [SerializeField] private float dashForce;
        [SerializeField] private float dashJump;
        [SerializeField] private float postDashDelay;
        [SerializeField] private bool pointTowardsTarget;

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
                if (pointTowardsTarget)
                {
                    enemy.PointTowardsTarget();
                }

                Vector2 jumpBackForce = new Vector2(-Mathf.Sign(enemy.FacingDirection) * dashForce, dashJump);

                // TODO: Switch this to linearVelocity
                movement.Rigidbody.linearVelocity = jumpBackForce;

                await Awaitable.WaitForSecondsAsync(postDashDelay, ct);
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