/*****************************************************************************
// File Name : AnimationUtility.cs
// Author : Arcadia Koederitz, Pierce
// Creation Date : 9/17/2026
// Last Modified : 9/17/2026
//
// Brief Description : Set of utility functions and for animations and animators.
*****************************************************************************/
using UnityEngine;
using System.Threading;

namespace TFOOL
{
    public static class AnimationUtility
    {
        /// <summary>
        /// Plays and then awaits the completion of an animation.
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="ct"></param>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        internal static async Awaitable PlayAndAwaitAnimation(string animStateName, Animator animator, CancellationToken ct, int layerIndex = 0)
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
        internal static async Awaitable AwaitAnimation(Animator animator, CancellationToken ct, int layerIndex = 0)
        {
            await Awaitable.WaitForSecondsAsync(GetCurrentAnimationDuration(animator, layerIndex), ct);
        }

        /// <summary>
        /// Gets the duration of the current animation state.
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        internal static float GetCurrentAnimationDuration(Animator animator, int layerIndex = 0)
        {
            animator.Update(0);
            AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(layerIndex);
            return animState.length / animState.speed;
        }
    }
}

