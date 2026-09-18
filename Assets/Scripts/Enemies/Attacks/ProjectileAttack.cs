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
using Unity.VisualScripting;
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
        [Header("Animation")]
        [SerializeField, Tooltip("The name of the animation state for this attack.")] 
        private string animationStateName;
        [SerializeField, Tooltip("The string signal that is sent from an animation event when the projectile should be thrown.")] 
        private string shootSignal;

        public override async Awaitable PerformAttack(EnemyController enemy, GameObject target, EnemyAttacker attackerComp, CancellationToken ct)
        {
            enemy.PlayAnimation(animationStateName);

            // Play an animation, then await until the given signal in the animation to shoot the projectile.
            await AIUtility.AwaitSignal(shootSignal, enemy, ct);

            Vector2 toTarget = target.transform.position - shotPoint.transform.position;
            ShootProjectile(toTarget.normalized * projectileSpeed);
            await Awaitable.NextFrameAsync(ct);
        }

        protected void ShootProjectile(Vector2 launchVector)
        {
            EnemyProjectile projInst = GameObject.Instantiate(projectilePrefab,
                shotPoint.transform.position, Quaternion.identity);
            projInst.Launch(launchVector);
        }
    }
}