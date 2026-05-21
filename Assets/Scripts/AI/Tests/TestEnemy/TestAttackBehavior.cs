/*****************************************************************************
// File Name : TestAttackBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Test attack.
*****************************************************************************/
using System.Threading;
using UnityEngine;

namespace Sodom.Enemies.AI.Tests
{
    [System.Serializable]
    public class TestAttackBehavior : AttackBehavior
    {
        [SerializeField] private float attackTime;
        [SerializeField] private MoveToDistanceBehavior moveToDistance;

        protected override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            if(!enemy.TryGetComponent(out EnemyAttacker attacker))
            {
                Debug.LogError("The enemy does not have an EnemyAttacker component.");
            }

            enemy.PointTowardsTarget();
            await moveToDistance.Run(enemy, ct);

            // Perform the attack.
            enemy.PointTowardsTarget();
            await attacker.PerformAttack(attackTime, ct);
            
            await Awaitable.WaitForSecondsAsync(postAttackDelay, ct);
        }
    }

}