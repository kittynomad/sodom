/*****************************************************************************
// File Name : GroundedCombatState.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 6/14/2026
//
// Brief Description : Base combat state logic for grounded enemies.
*****************************************************************************/
using CustomAttributes;
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    [DropdownGroup("Tests/TestGroundedEnemy")]
    public class TestEnemyCombatState : CombatState
    {
        [SerializeField] private MoveToDistanceBehavior moveInRange;
        [SerializeField] private RandomMovementBehavior randomMovement;
        [Header("Backdash")]
        [SerializeField, Tooltip("Controls how close the player needs to be to the enemy to make them backdash.")]
        private float backdashThreshold;
        [SerializeField] private BackdashBehavior backdash;

        public override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            await base.RunAI(enemy, ct);
            while (!ct.IsCancellationRequested)
            {
                // Make the enemy dash back.
                if (enemy.ToTarget.magnitude < backdashThreshold)
                {
                    await backdash.RunAI(enemy, ct);
                }
                else if (!moveInRange.IsWithinRange(enemy.ToTarget.magnitude))
                {
                    // Move the enemy to stay within aggro range of the player.
                    await moveInRange.RunAI(enemy, ct);
                }
                else
                {
                    await randomMovement.RunAI(enemy, ct);
                }

                ct.ThrowIfCancellationRequested();

                // Point towards the target.
                enemy.PointTowardsTarget();

                await GetAttackByDistance(enemy.ToTarget.magnitude).RunAI(enemy, ct);
            }
        }
    }

}