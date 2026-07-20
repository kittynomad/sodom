/*****************************************************************************
// File Name : CombatState.cs
// Author : Arcadia Koederitz
// Creation Date : 6/14/2026
// Last Modified : 6/14/2026
//
// Brief Description : Base combat state to allow for easier selection.
*****************************************************************************/
using CustomAttributes;
using System;
using System.Linq;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public abstract class CombatState : EnemyState
    {
        [SerializeReference, ClassDropdown(typeof(AttackBehavior))] protected AttackBehavior[] attacks;

        protected AttackBehavior GetRandomAttack(EnemyController enemy, EnemyAttacker attacker)
        {
            AttackBehavior[] validAttacks = attacks.Where(x => x.IsValid(enemy, attacker)).ToArray();

            int randomAttack = RandomUtility.GetRandomIndexWeighted(validAttacks);

            return validAttacks[randomAttack];
        }

        protected AttackBehavior GetAttackByName(string name)
        {
            return Array.Find(attacks, x => x.AttackName == name);
        }
    }
}