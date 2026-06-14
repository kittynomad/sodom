/*****************************************************************************
// File Name : CombatState.cs
// Author : Arcadia Koederitz
// Creation Date : 6/14/2026
// Last Modified : 6/14/2026
//
// Brief Description : Base combat state to allow for easier selection.
*****************************************************************************/
using CustomAttributes;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public abstract class CombatState : EnemyState
    {
        [SerializeReference, ClassDropdown(typeof(AttackBehavior))] protected AttackBehavior[] attacks;
    }
}