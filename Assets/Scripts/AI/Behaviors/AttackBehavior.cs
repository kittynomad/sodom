/*****************************************************************************
// File Name : AttackBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 5/19/2026
//
// Brief Description : Base class triggering enemy attacks through AI.  NOT the attack itself, that is stored as
// EnemyAttack.  Divided like this for easier creation of things like boss phases with overlapping attacks.
*****************************************************************************/
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public class AttackBehavior : EnemyBehavior
    {
        [SerializeField] private string attackName;
        [SerializeField] private float idealDistance; // Going to make this more modular later.
        [SerializeField] protected float postAttackDelay;

        public float IdealDistance => idealDistance;
        public string AttackName => attackName;

        protected override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            enemy.PointTowardsTarget();
            await PerformAttack(enemy, GetAttacker(enemy), enemy.Target, ct);
            await Awaitable.WaitForSecondsAsync(postAttackDelay, ct);
        }

        protected EnemyAttacker GetAttacker(EnemyController enemy)
        {
            if (!enemy.TryGetComponent(out EnemyAttacker attacker))
            {
                Debug.LogError($"Enemy {enemy} is missing an {nameof(EnemyAttacker)} component.");
            }
            return attacker;
        }

        protected async Awaitable PerformAttack(EnemyController enemy, EnemyAttacker attacker, GameObject target, CancellationToken ct)
        { 
            EnemyAttack toPerform = attacker.GetAttack(attackName);
            await toPerform.PerformAttack(enemy, target, ct);
        }
    }
}