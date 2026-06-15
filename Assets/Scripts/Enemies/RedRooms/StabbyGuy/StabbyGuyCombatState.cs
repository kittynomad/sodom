/*****************************************************************************
// File Name : StabbyGuyCombatState.cs
// Author : Arcadia Koederitz
// Creation Date : 6/14/2026
// Last Modified : 6/14/2026
//
// Brief Description : Combat state that controls the behavior of the stabby guy from the red rooms.
*****************************************************************************/
using UnityEngine;
using System.Threading;
using System;
using CustomAttributes;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    [DropdownGroup("Red Rooms/Stabby Guy")]
    public class StabbyGuyCombatState : CombatState
    {
        [SerializeField] private RandomMovementBehavior randomMovement;

        [SerializeField, Tooltip("Controls how close the player needs to be to the enemy to make them backdash.")]
        private float backdashThreshold;
        [SerializeField] private BackdashBehavior backdash;
        [SerializeField] private string throwAttackName;

        protected override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            await base.RunAI(enemy, ct);
            ct.ThrowIfCancellationRequested();
            
            // Get Components
    
            void CleanUp()
            {
                // Reset back to defaults.
            }
    
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    // Make the enemy dash back.
                    if (enemy.ToTarget.magnitude < backdashThreshold)
                    {
                        await backdash.Run(enemy, ct);
                        // If the enemy backdashes, always use the throw attack.
                        ct.ThrowIfCancellationRequested();
                        enemy.PointTowardsTarget();
                        await GetAttackByName(throwAttackName).Run(enemy, ct);
                        continue;
                    }
                    else
                    {
                        await randomMovement.Run(enemy, ct);
                        
                    }

                    ct.ThrowIfCancellationRequested();

                    // Point towards the target.
                    enemy.PointTowardsTarget();

                    await GetAttackByDistance(enemy.ToTarget.magnitude).Run(enemy, ct);
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