/*****************************************************************************
// File Name : ArcedProjectileAttack.cs
// Author : Arcadia Koederitz
// Creation Date : 5/21/2026
// Last Modified : 5/21/2026
//
// Brief Description : Attack that fires a gravity-affected projectile at an arc.
*****************************************************************************/
using CustomAttributes;
using System.Threading;
using UnityEngine;

namespace Sodom.Enemies
{
    [System.Serializable]
    [DropdownGroup("Projectiles")]
    public class ArcedProjectileAttack : ProjectileAttack
    {
        public override Awaitable PerformAttack(GameObject target, CancellationToken ct)
        {


            return Awaitable.NextFrameAsync(ct);
        }
    }

}