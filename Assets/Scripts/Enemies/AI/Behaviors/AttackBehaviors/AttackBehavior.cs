/*****************************************************************************
// File Name : AttackBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 5/19/2026
//
// Brief Description : Base class triggering enemy attacks through AI.  NOT the attack itself, that is stored as
// EnemyAttack.  Divided like this for easier creation of things like boss phases with overlapping attacks.
*****************************************************************************/
using CustomAttributes;
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public class AttackBehavior : EnemyBehavior, IWeighted
    {
        [SerializeField, Tooltip("Name of the attack on the enemy's EnemyAttacker component to use.")] 
        private string attackName;
        [SerializeField, Tooltip("Controls how likely the enemy is to choose this attack.  " +
            "Higher weight increases chance of this attack being chosen.")] 
        private int attackWeight;
        [SerializeField, Tooltip("How long to wait after performing the attack.")] protected float postAttackDelay;
        [SerializeReference, ClassDropdown(typeof(AttackCondition)), Tooltip("List of conditions that must be met by " +
    "the enemy for this attack to be used.")]
        private AttackCondition[] conditions;
        public int Weight => attackWeight;
        public string AttackName => attackName;

        public override Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            return PerformAttack(enemy, GetAttacker(enemy), ct);
        }

        protected EnemyAttacker GetAttacker(EnemyController enemy)
        {
            if (!enemy.TryGetComponent(out EnemyAttacker attacker))
            {
                Debug.LogError($"Enemy {enemy} is missing an {nameof(EnemyAttacker)} component.");
            }
            return attacker;
        }

        public virtual async Awaitable PerformAttack(EnemyController enemy, EnemyAttacker attacker, CancellationToken ct)
        {
            enemy.PointTowardsTarget();
            await attacker.PerformAttack(attackName, enemy, enemy.Target, ct);
            await Awaitable.WaitForSecondsAsync(postAttackDelay, ct);
        }

        /// <summary>
        /// Checks if this attack behavior is valid, checking both conditions on the behavior itself and on the attack.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public bool IsValid(EnemyController enemy, EnemyAttacker attacker)
        {
            bool isInvalid = false;
            // Check all conditions on the behavior.
            foreach(AttackCondition condition in conditions)
            {
                isInvalid |= !condition.CheckCondition(enemy, this, attacker);
            }
            return !isInvalid;
        }
    }
}