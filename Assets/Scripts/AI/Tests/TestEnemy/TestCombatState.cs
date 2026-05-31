/*****************************************************************************
// File Name : TestCombatState.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Test combat behavior to show intended functionality.
*****************************************************************************/
using CustomAttributes;
using System.Threading;
using UnityEngine;

namespace Sodom.Enemies.AI.Tests
{
    [System.Serializable]
    public class TestCombatState : EnemyBehavior
    {
        [SerializeField] private Color debugColor;
        [SerializeField] private float preActDelay;
        [SerializeField] private MoveToDistanceBehavior stayInRange;
        [Header("Backdash")]
        [SerializeField, Tooltip("Controls how close the player needs to be to the enemy to make them backdash.")]
        private float backdashThreshold;
        [SerializeField] private BackdashBehavior backdash;
        [SerializeField, Tooltip("How long the enemy should wait before performing it's next attack if it " +
            "doesn't backdash.")] 
        private float backdashFailDelay;
        [SerializeReference, ClassDropdown(typeof(AttackBehavior))] private AttackBehavior[] attacks;

        protected override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            if (enemy.TryGetComponent(out SpriteRenderer rend))
            {
                rend.color = debugColor;
            }
            await Awaitable.WaitForSecondsAsync(preActDelay, ct);
            while (!ct.IsCancellationRequested)
            {
                // Make the enemy dash back.
                if (enemy.ToTarget.magnitude < backdashThreshold)
                {
                    await backdash.Run(enemy, ct);
                }
                else
                {
                    await Awaitable.WaitForSecondsAsync(backdashFailDelay);
                }

                // Move the enemy to stay within aggro range of the player.
                await stayInRange.Run(enemy, ct);

                ct.ThrowIfCancellationRequested();

                // Point towards the target.
                enemy.PointTowardsTarget();

                // Chose an attack to perform based on distance from their ideal distance.
                int chosenAttackIndex = 0;
                float closestDistance = Mathf.Abs(attacks[0].IdealDistance - enemy.ToTarget.magnitude);
                for(int i = 1; i < attacks.Length; i++)
                {
                    float distanceToIdeal = Mathf.Abs(attacks[i].IdealDistance - enemy.ToTarget.magnitude);
                    // If the enemy is closer to this attack's ideal distance from it's target, this is now the 
                    // most ideal attack.
                    if (distanceToIdeal < closestDistance)
                    {
                        chosenAttackIndex = i;
                        closestDistance = distanceToIdeal;
                    }
                }
                await attacks[chosenAttackIndex].Run(enemy, ct);
            }
        }
    }

}