/*****************************************************************************
// File Name : CombatState.cs
// Author : Arcadia Koederitz
// Creation Date : 6/14/2026
// Last Modified : 6/14/2026
//
// Brief Description : Base combat state to allow for easier selection.
*****************************************************************************/
using CustomAttributes;
using Unity.VisualScripting;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public abstract class CombatState : EnemyState
    {
        [SerializeReference, ClassDropdown(typeof(AttackBehavior))] protected AttackBehavior[] attacks;

        protected AttackBehavior GetAttackByDistance(float distance)
        {
            int chosenAttackIndex = 0;
            float closestDistance = Mathf.Abs(attacks[0].IdealDistance - distance);
            for (int i = 1; i < attacks.Length; i++)
            {
                float distanceToIdeal = Mathf.Abs(attacks[i].IdealDistance - distance);
                // If the enemy is closer to this attack's ideal distance from it's target, this is now the 
                // most ideal attack.
                if (distanceToIdeal < closestDistance)
                {
                    chosenAttackIndex = i;
                    closestDistance = distanceToIdeal;
                }
            }

            return attacks[chosenAttackIndex];
        }
    }
}