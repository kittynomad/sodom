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

[System.Serializable]
public class TestAttackBehavior : AttackBehavior
{
    [SerializeField] private float attackTime;
    [SerializeField] private MoveToDistanceBehavior moveToDistance;

    protected override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
    {
        enemy.PointTowardsTarget();
        await moveToDistance.Run(enemy, ct);

        // Perform the attack.
        enemy.PointTowardsTarget();
        Transform hitbox = enemy.transform.GetChild(1);
        hitbox.gameObject.SetActive(true);
        await Awaitable.WaitForSecondsAsync(attackTime, ct);
        hitbox.gameObject.SetActive(false);


        await Awaitable.WaitForSecondsAsync(postAttackDelay, ct);
    }
}
