/*****************************************************************************
// File Name : ProjectileAttack.cs
// Author : Arcadia Koederitz
// Creation Date : 5/21/2026
// Last Modified : 5/21/2026
//
// Brief Description : Attack that fires a basic projectile
*****************************************************************************/
using CustomAttributes;
using System.Threading;
using UnityEditor.Search;
using UnityEngine;

namespace TFOOL.Enemies
{
    [System.Serializable]
    [DropdownGroup("Projectiles")]
    public class ProjectileAttack : EnemyAttack
    {
        [SerializeField] protected Transform shotPoint;
        [SerializeField] protected Rigidbody2D projectilePrefab;
        [SerializeField] protected float projectileSpeed;

        public override Awaitable PerformAttack(GameObject target, CancellationToken ct)
        {
            Vector2 toTarget = target.transform.position - shotPoint.transform.position;
            Rigidbody2D projInst = GameObject.Instantiate(projectilePrefab, 
                shotPoint.transform.position, Quaternion.identity);
            projInst.AddForce(toTarget.normalized * projectileSpeed, ForceMode2D.Impulse);
            return Awaitable.NextFrameAsync(ct);
        }
    }
}