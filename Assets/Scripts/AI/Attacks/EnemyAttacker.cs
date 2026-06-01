/*****************************************************************************
// File Name : EnemyAttacker.cs
// Author : Arcadia Koederitz
// Creation Date : 5/19/2026
// Last Modified : 5/19/2026
//
// Brief Description : Component script that controls all the attacks an enemy can use and any parameters/references
// they have.
*****************************************************************************/
using CustomAttributes;
using System;
using System.Threading;
using UnityEngine;

namespace Sodom.Enemies
{
    public class EnemyAttacker : MonoBehaviour
    {
        [SerializeReference, ClassDropdown(typeof(EnemyAttack))] private EnemyAttack[] attacks;

        public EnemyAttack GetAttack(string name)
        {
            return Array.Find(attacks, x => x.Name == name);
        }
    }
}