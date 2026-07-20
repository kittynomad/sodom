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
using System.Threading;
using TFOOL.Enemies.AI;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TFOOL.Enemies
{
    public class EnemyAttacker : MonoBehaviour
    {
        [SerializeReference, ClassDropdown(typeof(EnemyAttack))] private EnemyAttack[] attacks;

        private readonly Dictionary<string, EnemyAttack> attackDict = new Dictionary<string, EnemyAttack>();

        [SerializeField] private EnemyHitbox[] hitboxes;

        public event Action<IKillable, EnemyHitbox> OnHitEvent;

        private string lastUsedAttack;

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
            hitboxes = GetComponentsInChildren<EnemyHitbox>(true);
            foreach(EnemyHitbox hitbox in hitboxes)
            {
                hitbox.OnHitEvent += HandleHitboxHit;
            }
        }

        public void HandleHitboxHit(IKillable hitObj, EnemyHitbox hitbox)
        {
            Debug.Log("Hit Object from attacker.");
            OnHitEvent?.Invoke(hitObj, hitbox);
        }

        public EnemyAttack GetAttack(string name)
        {
            return attackDict[name];
        }

        public Awaitable PerformAttack(string attackName, EnemyController enemy, GameObject target, CancellationToken ct)
        {
            EnemyAttack toPerform = GetAttack(attackName);
            lastUsedAttack = toPerform.Name;
            return toPerform.PerformAttack(enemy, target, this, ct);
        }

        private void OnDestroy()
        {
            OnHitEvent = null;
        }
    }
}