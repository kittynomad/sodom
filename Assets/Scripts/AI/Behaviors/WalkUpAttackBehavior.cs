/*****************************************************************************
// File Name : WalkUpAttackBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 5/21/2026
// Last Modified : 5/21/2026
//
// Brief Description : Makes the enemy walk up and attack an enemy.
*****************************************************************************/
using System.Threading;
using UnityEngine;

namespace Sodom.Enemies.AI
{
    public class WalkUpAttackBehavior : AttackBehavior
    {
        [SerializeField] private MoveToDistanceBehavior moveToDistance;

        protected override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            enemy.PointTowardsTarget();
            await moveToDistance.Run(enemy, ct);
            await base.RunAI(enemy, ct);
        }
    }

}