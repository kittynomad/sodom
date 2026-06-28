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
using TFOOL.Enemies.AI;
using UnityEditor.Search;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TFOOL.Enemies
{
    [System.Serializable]
    [DropdownGroup("Projectiles")]
    public class ProjectileAttack : EnemyAttack
    {
        [SerializeField, Tooltip("Where the projectile should be spawned at and launched from.")] 
        protected Transform shotPoint;
        [SerializeField] protected EnemyProjectile projectilePrefab;
        [SerializeField] protected float projectileSpeed;

        public override Awaitable PerformAttack(EnemyController enemy, GameObject target, CancellationToken ct)
        {
            Vector2 toTarget = target.transform.position - shotPoint.transform.position;
            ShootProjectile(toTarget.normalized * projectileSpeed);
            return Awaitable.NextFrameAsync(ct);
        }

        protected void ShootProjectile(Vector2 launchVector)
        {
            EnemyProjectile projInst = GameObject.Instantiate(projectilePrefab,
                shotPoint.transform.position, Quaternion.identity);
            projInst.Launch(launchVector);
        }
    }
}