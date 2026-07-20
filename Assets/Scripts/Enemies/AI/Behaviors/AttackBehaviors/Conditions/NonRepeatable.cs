/*****************************************************************************
// File Name : NonRepeatable.cs
// Author : Arcadia Koederitz
// Creation Date : 7/20/2026
// Last Modified : 7/20/2026
//
// Brief Description : Condition that prevents an attack from being selected twice.
*****************************************************************************/
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public class NonRepeatable : AttackCondition
    {
        public override bool CheckCondition(EnemyController enemy, AttackBehavior attackBehavior, EnemyAttacker attacker)
        {
            if (attacker.PreviousAttack == attackBehavior.AttackName)
            {
                Debug.Log(attacker.PreviousAttack);
                return false;
            }
            return true;
        }
    }
}