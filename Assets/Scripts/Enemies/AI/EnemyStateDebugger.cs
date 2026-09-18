/*****************************************************************************
// File Name : EnemyStateDebugger.cs
// Author : Arcadia Koederitz
// Creation Date : 9/18/2026
// Last Modified : 9/18/2026
//
// Brief Description : Changes the color tint on a given sprite renderer based on the enemy's state.
*****************************************************************************/
using CustomAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [RequireComponent(typeof(EnemyController))]
    public class EnemyStateDebugger : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer enemyRend;
        [SerializeField, ShowIfNull] private EnemyController enemy;

        private EnemyState lastState;

        private void Reset()
        {
            enemy = GetComponent<EnemyController>();
        }

        private void Update()
        {
            if (enemy.CurrentState != lastState)
            {
                lastState = enemy.CurrentState;
                enemyRend.color = enemy.CurrentState.DebugColor;
            }
        }
    }
}