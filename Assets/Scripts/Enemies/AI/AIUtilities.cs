/*****************************************************************************
// File Name : AIUtilities.cs
// Author : Arcadia Koederitz
// Creation Date : 8/20/2026
// Last Modified : 8/20/2026
//
// Brief Description : Set of utility functions used by multiple AI scripts.
*****************************************************************************/
using System.Threading;
using TFOOL.Enemies.AI;
using UnityEngine;

public static class AIUtilities
{
    /// <summary>
    /// Awaits a signal sent to the EnemyController.
    /// </summary>
    /// <param name="signalName">The name of the signal from the animation event.</param>
    /// <returns></returns>
    internal static async Awaitable AwaitSignal(string signalName, EnemyController enemy, CancellationToken ct)
    {
        while (enemy.AnimationSignal != signalName && !ct.IsCancellationRequested)
        {
            await Awaitable.NextFrameAsync(ct);
        }
    }

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
