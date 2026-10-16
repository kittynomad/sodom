/*****************************************************************************
// File Name : AnimatorHelpers.cs
// Author : Arcadia Koederitz
// Creation Date : September 7, 2025
// Last Modified : September 7, 2025
//
// Brief Description : Set of static helper functions that deal with the animator
// that have common utility across multiple projects.
*****************************************************************************/
using System.Threading;
using UnityEngine;

namespace AriUtils
{
    public static class AnimatorUtil
    {
        /// <summary>
        /// Plays and then awaits the completion of an animation.
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="ct"></param>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        public static async Awaitable PlayAndAwaitAnimation(string animStateName, Animator animator, CancellationToken ct, int layerIndex = 0)
        {
            animator.Play(animStateName);
            await AwaitAnimation(animator, ct, layerIndex);
        }

        /// <summary>
        /// Waits until the current state of the animator completes.
        /// </summary>
        /// <param name="animator">The animator on the enemy to wait for.</param>
        /// <param name="ct">The cancellation token to cancel the await.</param>
        /// <param name="layerIndex">Optional layer index of the animator to await the state of.</param>
        /// <returns></returns>
        public static async Awaitable AwaitAnimation(Animator animator, CancellationToken ct, int layerIndex = 0)
        {
            await Awaitable.WaitForSecondsAsync(GetAnimationDuration(animator, layerIndex), ct);
        }

        /// <summary>
        /// Gets the duration of the current animation state.
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        public static float GetAnimationDuration(Animator animator, int layerIndex = 0)
        {
            animator.Update(0);
            AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(layerIndex);
            return animState.length / animState.speed;
        }
    }

}