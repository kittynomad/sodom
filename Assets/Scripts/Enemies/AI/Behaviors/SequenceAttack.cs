/*****************************************************************************
// File Name : SequenceBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 6/20/2026
// Last Modified : 6/20/2026
//
// Brief Description : Performs an attack and a set of sub-behaviors.
*****************************************************************************/
using UnityEngine;
using System.Threading;
using System;
using CustomAttributes;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    [DropdownTag("Universal")]
    public class SequenceAttack : AttackBehavior
    {
        [SerializeReference, ClassDropdown(typeof(EnemyBehavior), ExcludedTypes = new Type[] { typeof(AttackBehavior), typeof(EnemyState) })] 
        private EnemyBehavior[] preBehaviors;
        [SerializeReference, ClassDropdown(typeof(EnemyBehavior), ExcludedTypes = new Type[] { typeof(AttackBehavior), typeof(EnemyState) })] 
        private EnemyBehavior[] postBehaviors;
        
        public override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            foreach (var behavior in preBehaviors)
            {
                await behavior.RunAI(enemy, ct);
            }

            ct.ThrowIfCancellationRequested();
            enemy.PointTowardsTarget();
            await PerformAttack(enemy, GetAttacker(enemy), enemy.Target, ct);

            foreach (var behavior in postBehaviors)
            {
                await behavior.RunAI(enemy, ct);
            }

            await Awaitable.WaitForSecondsAsync(postAttackDelay, ct);

        }
    }
}