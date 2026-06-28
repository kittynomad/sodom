/*****************************************************************************
// File Name : EnemyAttack.cs
// Author : Arcadia Koederitz
// Creation Date : 5/19/2026
// Last Modified : 5/21/2026
//
// Brief Description : Script that represents a specific attack an enemy can use.
*****************************************************************************/
using System.Threading;
using TFOOL.Enemies.AI;
using UnityEngine;

namespace TFOOL.Enemies
{
    [System.Serializable]
    public abstract class EnemyAttack
    {
        [SerializeField, Tooltip("Name that is used by the EnemyAI to perform this attack from an AttackBehavior.")] 
        private string attackName;

        public string Name => attackName;

        public abstract Awaitable PerformAttack(EnemyController enemy, GameObject target, CancellationToken ct);
    }
}