/*****************************************************************************
// File Name : EnterCombatState.cs
// Author : Arcadia Koederitz
// Creation Date : 6/7/2026
// Last Modified : 6/7/2026
//
// Brief Description : Plays an animation for the enemy entering combat when they first see the player.
*****************************************************************************/
using System;
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public class EnterCombatState : EnemyState
    {
        [SerializeField] private float enterCombatDelay;
        protected override async Awaitable RunAI(EnemyController enemy, CancellationToken ct)
        {
            await base.RunAI(enemy, ct);
            ct.ThrowIfCancellationRequested();

            // Get Components

            void CleanUp()
            {
                // Reset back to defaults.
            }

            try
            {
                await Awaitable.WaitForSecondsAsync(enterCombatDelay);
                CleanUp();
            }
            catch (OperationCanceledException oce)
            {
                CleanUp();
                throw oce;
            }
        }
    }
}