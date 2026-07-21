/*****************************************************************************
// File Name : EnemyBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 5/19/2026
//
// Brief Description : Controls a specific behavior of an enemy.  Can be either used as a state, or as a specific 
// sub-behaviour or action that the enemy takes as part of a larger state.
*****************************************************************************/
using System;
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    public abstract class EnemyBehavior
    {
        /// <summary>
        /// Main function that controls the enemy's actions while performing this behavior.
        /// </summary>
        /// <remarks>Uses async because I need an easy way of handling cancellation.</remarks>
        /// <param name="enemy">The enemy that is being controlled.</param>
        /// <param name="ct">The cancellation token for handling cancels on state change.</param>
        /// <returns></returns>
        public abstract Awaitable RunAI(EnemyController enemy, CancellationToken ct);

        #region Utilities
        /// <summary>
        /// Awaits a signal sent to the EnemyController.
        /// </summary>
        /// <param name="signalName">The name of the signal from the animation event.</param>
        /// <returns></returns>
        protected static async Awaitable AwaitSignal(string signalName, EnemyController enemy, CancellationToken ct)
        {
            while(enemy.AnimationSignal != signalName && !ct.IsCancellationRequested)
            {
                await Awaitable.NextFrameAsync(ct);
            }
        }
        #endregion
    }
}
