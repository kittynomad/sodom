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
using System.Collections.Generic;
using UnityEngine;

namespace TFOOL.Enemies
{
    public class EnemyAttacker : MonoBehaviour
    {
        [SerializeReference, ClassDropdown(typeof(EnemyAttack))] private EnemyAttack[] attacks;

        private readonly Dictionary<string, EnemyAttack> attackDict = new Dictionary<string, EnemyAttack>();

        private EnemyHitbox[] hitboxes;

        public event Action<EnemyHitbox, IKillable> OnHitEvent;

        /// <summary>
        /// Setup a dictionary at runtime for quicker attack access.  Stupid lack of serialized dictionaries.
        /// </summary>
        private void Awake()
        {
            foreach(EnemyAttack attack in attacks)
            {
                attackDict.Add(attack.Name, attack);
            }

            // Initialize hitboxes.
            hitboxes = GetComponentsInChildren<EnemyHitbox>();
            foreach(EnemyAttack attack in attacks)
            {

            }
        }

        public void HandleHitboxHit(IKillable hitObj, EnemyHitbox hitbox)
        {

        }

        public EnemyAttack GetAttack(string name)
        {
            return attackDict[name];
        }

        private void OnDestroy()
        {
            OnHitEvent = null;
        }
    }
}