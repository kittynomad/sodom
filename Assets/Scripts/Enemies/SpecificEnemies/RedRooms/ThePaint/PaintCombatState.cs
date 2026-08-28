/*****************************************************************************
// File Name : PaintCombatState.cs
// Author : Arcadia Koederitz
// Creation Date : 8/20/2026
// Last Modified : 8/20/2026
//
// Brief Description : 
*****************************************************************************/
using CustomAttributes;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    [DropdownGroup("Red Rooms/Paint")]
    public class PaintCombatState : CombatState
    {
        [SerializeField, Tooltip("The time in seconds between each of the enemy's attacks.")] private float attackTime;
        [SerializeField, Tooltip("If the enemy is less than this distance away, then attack time decreased " +
            "by acceleratedAttackTime each second instead of 1.")] 
        private float acceleratedAttackRange;
        [SerializeField, Tooltip("How much quicker attack time decreases when the player is within " +
            "acceleratedAttackRange of the enemy.")] 
        private float acceleratedAttackTime = 1;
        [SerializeField, Tooltip("Controls the range from the player the enemy tries to maintain while preparing to attack.")] 
        private MoveToDistanceBehavior moveInRange;
        [SerializeField, Tooltip("Controls the enemy's wandering movement when somewhat near the player.")]
        private RandomMovementBehavior wanderingMovement;
        [SerializeField, Tooltip("Randomized delay between enemy wanderings.")] 
        private Vector2 randomMovementDelay;

        public override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            await base.RunAI(enemy, ct);
            ct.ThrowIfCancellationRequested();

            // Get Components
            if (!enemy.TryGetComponent(out EnemyAttacker attacker))
            {
                Debug.LogError($"Enemy {enemy} is missing an {nameof(EnemyAttacker)} component.");
            }

            CancellationTokenSource movementCts = null;
            Awaitable movementSubroutine = null;
            void CleanUp()
            {
                // Reset back to defaults.
                movementCts?.Cancel();
            }
    
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    float attackTime = this.attackTime;
                    float wanderTime = UnityEngine.Random.Range(randomMovementDelay.x, randomMovementDelay.y);
                    while(attackTime > 0)
                    {
                        float distToPlayer = enemy.ToTarget.magnitude;
                        // If the enemy is too far away from the player, move back in range.
                        if (movementSubroutine == null || movementSubroutine.IsCompleted)
                        {
                            if (!moveInRange.IsWithinRange(distToPlayer))
                            {
                                movementCts = new CancellationTokenSource();
                                movementSubroutine = moveInRange.RunAI(enemy, movementCts.Token);
                            }
                            else
                            {
                                wanderTime -= Time.deltaTime;
                                if (wanderTime < 0)
                                {
                                    movementCts = new CancellationTokenSource();
                                    movementSubroutine = wanderingMovement.RunAI(enemy, movementCts.Token);
                                    wanderTime = UnityEngine.Random.Range(randomMovementDelay.x, randomMovementDelay.y);
                                }
                            }
                        }
                        

                        // Decrement AttackTime.
                        if (distToPlayer < acceleratedAttackRange)
                        {
                            attackTime -= Time.deltaTime * acceleratedAttackTime;
                        }
                        else
                        {
                            attackTime -= Time.deltaTime;
                        }
                        await Awaitable.NextFrameAsync(ct);
                    }
                    // Enemy continually wanders around the player until the player is close enough.

                    movementCts?.Cancel();
                    movementSubroutine = null;
                    await GetWeightedAttack(enemy, attacker).PerformAttack(enemy, attacker, ct);
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