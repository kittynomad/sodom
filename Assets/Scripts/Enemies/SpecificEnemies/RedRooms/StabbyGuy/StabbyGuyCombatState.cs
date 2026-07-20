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
        [Header("Custom AI Values")]
        [SerializeField, Tooltip("Enemy moves randomly a little between each attack to prevent enemies from " +
            "stacking on top of each other.  Adjust random movements here.")] 
        private RandomMovementBehavior randomMovement;

        [SerializeField, Tooltip("Controls how close the player needs to be to the enemy to make them backdash.")]
        private float backdashThreshold;
        [SerializeField, Tooltip("Controls the backdash the enemy performs if the target is too close.")] private BackdashBehavior backdash;
        [SerializeField, Tooltip("Name of the knife throw attack.  Used to find the knife throw and force the " +
            "enemy to use it after backdashing.")] 
        private string throwAttackName;

        public override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            await base.RunAI(enemy, ct);
            ct.ThrowIfCancellationRequested();

            // Get Components
            if (!enemy.TryGetComponent(out EnemyAttacker attacker))
            {
                Debug.LogError($"Enemy {enemy} is missing an {nameof(EnemyAttacker)} component.");
            }

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
                        await backdash.RunAI(enemy, ct);
                        // If the enemy backdashes, always use the throw attack.
                        ct.ThrowIfCancellationRequested();
                        enemy.PointTowardsTarget();
                        await GetAttackByName(throwAttackName).RunAI(enemy, ct);
                        continue;
                    }
                    else
                    {
                        await randomMovement.RunAI(enemy, ct);
                        
                    }

                    ct.ThrowIfCancellationRequested();

                    // Point towards the target.
                    enemy.PointTowardsTarget();

                    await GetRandomAttack(enemy, attacker).PerformAttack(enemy, attacker, ct);
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