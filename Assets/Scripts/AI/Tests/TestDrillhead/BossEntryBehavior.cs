/*****************************************************************************
// File Name : BossEntryBehavior.cs
// Author : Arcadia Koederitz
// Creation Date : 5/3/2026
// Last Modified : 5/3/2026
//
// Brief Description : Automatically get the player as a target when boss enters..
*****************************************************************************/
using System.Threading;
using UnityEngine;

[System.Serializable]
public class BossEntryBehavior : EnemyBehavior
{
    protected override Awaitable RunAI(EnemyController controller, CancellationToken ct)
    {
        controller.Target = GameObject.FindGameObjectWithTag("Player");
        return Awaitable.NextFrameAsync();
    }
}
