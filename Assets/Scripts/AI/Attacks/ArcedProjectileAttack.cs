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
using TFOOL.Enemies.AI;
using UnityEngine;

namespace TFOOL.Enemies
{
    [System.Serializable]
    [DropdownGroup("Projectiles")]
    public class ArcedProjectileAttack : ProjectileAttack
    {
        public override Awaitable PerformAttack(EnemyController enemy, GameObject target, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            Vector2 toTarget = target.transform.position - shotPoint.transform.position;

            Rigidbody2D projInst = GameObject.Instantiate(projectilePrefab,
                shotPoint.transform.position, Quaternion.identity);

            projInst.AddForce(GetArcedShotVector2(projectileSpeed, 
                target.transform.position, shotPoint.transform.position, projInst.gravityScale), ForceMode2D.Impulse);

            return Awaitable.NextFrameAsync(ct);
        }

        /// <summary>
        /// Utilizes the formula for finding the angle of a projectile based on initial speed and position.
        /// </summary>
        /// <param name="speed">The speed that the projectile will be shot at.</param>
        /// <param name="targetPosition">The target position.</param>
        /// <param name="shotPosition">The position from which the projectile will be shot.</param>
        /// <param name="gravityScale">The gravity scale of the projectile.</param>
        /// <returns>The vector that the projectiles hould be shot at to hit the target.</returns>
        private Vector2 GetArcedShotVector(float speed, Vector2 targetPosition, Vector2 shotPosition, float gravityScale)
        {
            Vector2 deltaPosition = targetPosition - shotPosition;
            float gravity = gravityScale * Physics2D.gravity.y;
            float phaseAngle = Mathf.Atan(deltaPosition.x / deltaPosition.y);

            float angle = (Mathf.Acos((deltaPosition.y + (gravity * Mathf.Pow(deltaPosition.x, 2) / Mathf.Pow(speed, 2))) 
                / deltaPosition.magnitude) + phaseAngle) / 2;

            if (float.IsNaN(angle))
            {
                // NaN means that the target is out of range, in which case just use the ideal angle.
                Debug.Log("Out of range");
                angle = 45f;
            }
            Debug.Log(angle * Mathf.Rad2Deg);

            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
        }

        /// <summary>
        /// Utilizes the formula for finding the angle of a projectile based on initial speed and position.
        /// </summary>
        /// <param name="verticalSpeed">The vertical speed of the projectile..</param>
        /// <param name="targetPosition">The target position.</param>
        /// <param name="shotPosition">The position from which the projectile will be shot.</param>
        /// <param name="gravityScale">The gravity scale of the projectile.</param>
        /// <returns>The vector that the projectiles hould be shot at to hit the target.</returns>
        private Vector2 GetArcedShotVector2(float verticalSpeed, Vector2 targetPosition, Vector2 shotPosition, float gravityScale)
        {
            Vector2 deltaPosition = targetPosition - shotPosition;
            float gravity = gravityScale * Physics2D.gravity.y;

            float travelTime = (verticalSpeed + Mathf.Sqrt(Mathf.Abs(Mathf.Pow(verticalSpeed, 2) + 2 * gravity * deltaPosition.y))) / gravity;
            float horizontalSpeed = deltaPosition.x / travelTime;

            return new Vector2(-horizontalSpeed, verticalSpeed);
        }
    }

}