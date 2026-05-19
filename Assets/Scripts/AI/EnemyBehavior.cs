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

namespace Sodom.Enemies.AI
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
        public async Awaitable Run(EnemyController enemy, CancellationToken ct)
        {
            OnAIStart(enemy);

            try
            {
                await RunAI(enemy, ct);
                OnAIEnd(enemy);
            }
            catch (OperationCanceledException)
            {
                OnAIEnd(enemy);
                throw new OperationCanceledException();
            }
        }

        protected virtual void OnAIStart(EnemyController enemy) { }

        protected virtual void OnAIEnd(EnemyController enemy) { }

        protected abstract Awaitable RunAI(EnemyController enemy, CancellationToken ct);

        #region Utilities
        /// <summary>
        /// Awaits a delay equal to the length of the animator's current animation.
        /// </summary>
        /// <param name="anim">The animator to wait for.</param>
        /// <returns></returns>
        protected static Awaitable AwaitAnimation(Animator anim, CancellationToken ct)
        {
            anim.Update(0);
            float animationDuration = anim.GetCurrentAnimatorStateInfo(0).length;
            return Awaitable.WaitForSecondsAsync(animationDuration, ct);
        }
        #endregion
    }
}
