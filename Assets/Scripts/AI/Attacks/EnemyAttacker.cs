/*****************************************************************************
// File Name : EnemyAttacker.cs
// Author : Arcadia Koederitz
// Creation Date : 5/19/2026
// Last Modified : 5/19/2026
//
// Brief Description : Component script that controls enemy attacks.
*****************************************************************************/
using System.Threading;
using UnityEngine;

public class EnemyAttacker : MonoBehaviour
{
    [SerializeField] private GameObject hitbox;

    /// <summary>
    /// Performs a basic spawn hitbox attack with a given timing.
    /// </summary>
    /// <param name="attackTime">The amount of time for the attack.</param>
    /// <param name="ct">The cancellation token for the enemy behavior.</param>
    /// <returns></returns>
    public async Awaitable PerformAttack(float attackTime, CancellationToken ct)
    {
        hitbox.SetActive(true);
        await Awaitable.WaitForSecondsAsync(attackTime, ct);
        hitbox.SetActive(false);
    }
}
