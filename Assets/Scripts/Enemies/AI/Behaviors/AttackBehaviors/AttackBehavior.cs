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
    public class AttackBehavior : EnemyBehavior, IWeighted
    {
        [SerializeField, Tooltip("Name of the attack on the enemy's EnemyAttacker component to use.")] 
        private string attackName;
        [SerializeField, Tooltip("Controls how likely the enemy is to choose this attack.  " +
            "Higher weight increases chance of this attack being chosen.")] 
        private int attackWeight;
        [SerializeField, Tooltip("The minimum distance that there must be between the enemy and it's target for " +
            "it to choose this attack.  Set to 0 for none.\n\nIe. If set to 4, then the enemy will only use " +
            "this attack if the player is more than 4 units away.")] 
        private float minDistance;
        [SerializeField, Tooltip("The maximum distance that there can be between the enemy and it's target for " +
    "it to choose this attack.  Set to 0 for none.\n\nIe. If set to 4, then the enemy will only use this " +
            "attack if the target is within 4 units.")]
        private float maxDistance;
        [SerializeField, Tooltip("How long to wait after performing the attack.")] protected float postAttackDelay;

        public float MinDistance => minDistance;
        public float MaxDistance => maxDistance;
        public int Weight => attackWeight;
        public string AttackName => attackName;

        public override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
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
            await attacker.PerformAttack(attackName, enemy, target, ct);
        }
    }
}