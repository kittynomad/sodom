/*****************************************************************************
// File Name : EnemyBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Controls a specific behavior of an enemy.
*****************************************************************************/
using System.Threading;
using UnityEngine;

[System.Serializable]
public abstract class EnemyBehavior
{
    /// <summary>
    /// Main function that controls the enemy's behavior during this state.
    /// </summary>
    /// <remarks>Uses async because I need an easy way of handling cancellation.</remarks>
    /// <param name="controller">The enemy that is being controlled.</param>
    /// <param name="ct">The cancellation token for handling cancels on state change.</param>
    /// <returns></returns>
    public abstract Awaitable Run(EnemyController controller, CancellationToken ct);

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
