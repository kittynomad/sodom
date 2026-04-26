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

[System.Serializable]
public class TestCombatState : EnemyBehavior
{
    [SerializeField] private MaintainDistanceBehavior maintainDistance;
    [SerializeReference, ClassDropdown(typeof(AttackBehavior))] private AttackBehavior[] attacks;

    public override async Awaitable Run(EnemyController enemy, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            // Continually maintain a distance from the player.
            await maintainDistance.Run(enemy, ct);

            // Point towards the target.
            enemy.PointTowardsTarget();

            // Chose a random attack to perform.
            AttackBehavior chosenAttack = attacks[Random.Range(0, attacks.Length)];
            if (chosenAttack != null)
            {
                await chosenAttack.Run(enemy, ct);
            }
        }
    }
}
