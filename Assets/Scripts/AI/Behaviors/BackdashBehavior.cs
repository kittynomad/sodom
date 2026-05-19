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

namespace Sodom.Enemies.AI
{
    [Serializable]
    public class BackdashBehavior : EnemyBehavior
    {
        [SerializeField] private float dashForce;
        [SerializeField] private float dashJump;
        [SerializeField] private float postDashDelay;

        protected override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
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

                Vector2 jumpBackForce = new Vector2(-Mathf.Sign(enemy.ToTarget.x) * dashForce, dashJump);
                movement.Rigidbody.AddForce(jumpBackForce, ForceMode2D.Impulse);

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