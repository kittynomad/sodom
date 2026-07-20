/*****************************************************************************
// File Name : RequiredDistance.cs
// Author : Arcadia Koederitz
// Creation Date : 7/20/2026
// Last Modified : 7/20/2026
//
// Brief Description : Condition that requires the enemy be within a specific distance range from the target.
*****************************************************************************/
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public class RequiredDistance : AttackCondition
    {
        [SerializeField, Tooltip("The minimum distance that there must be between the enemy and it's target for " +
        "it to choose this attack.  Set to 0 for none.\n\nIe. If set to 4, then the enemy will only use " +
        "this attack if the player is more than 4 units away.")]
        private float minDistance;
        [SerializeField, Tooltip("The maximum distance that there can be between the enemy and it's target for " +
    "it to choose this attack.  Set to 0 for none.\n\nIe. If set to 4, then the enemy will only use this " +
            "attack if the target is within 4 units.")]
        private float maxDistance;

        public override bool CheckCondition(EnemyController enemy, AttackBehavior attackBehavior, EnemyAttacker attacker)
        {
            return (minDistance <= 0 || enemy.ToTarget.x > minDistance) && (maxDistance >= 0 || enemy.ToTarget.x < maxDistance); 
        }
    }

}