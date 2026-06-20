/*****************************************************************************
// File Name : EnemyProjectile.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 6/14/2026
//
// Brief Description : Base class for enemy projectiles.
*****************************************************************************/
using CustomAttributes;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

namespace TFOOL.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyProjectile : MonoBehaviour
    {
        private const string PLAYER_HITBOX_LAYER = "PlayerHitbox";

        [SerializeField] private float maxLifetime;
        [SerializeField, Tooltip("The amount of time the projectile will fly straight before gravity takes effect. " +
            " Set to 0 to ignore.")] 
        private float falloffTime;

        [SerializeField, ShowIfNull] private Rigidbody2D rb;

        public Rigidbody2D Rigidbody => rb;

        private void Reset()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Collision Handling.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // If colliding with a player hitbox, destroy this projectile.
            if (collision.gameObject.layer == LayerMask.NameToLayer(PLAYER_HITBOX_LAYER))
            {
                DestroyProjectile();
            }
        }


        /// <summary>
        /// Launches this projectile with a given starting speed.
        /// </summary>
        /// <param name="launchVector"></param>
        public void Launch(Vector2 launchVector)
        {
            rb.AddForce(launchVector, ForceMode2D.Impulse);
            StartCoroutine(LifetimeRoutine());
        }

        private IEnumerator LifetimeRoutine()
        {
            float baseGravityScale = -1;
            if (falloffTime > 0)
            {
                baseGravityScale = rb.gravityScale;
                rb.gravityScale = 0;
            }

            float timer = 0;
            while(timer < maxLifetime)
            {
                if (timer > falloffTime && baseGravityScale > 0)
                {
                    rb.gravityScale = baseGravityScale;
                    baseGravityScale = -1;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            DestroyProjectile();
        }

        /// <summary>
        /// Handles correctly destroying this projectile.  Might need to do object pooling & such later.
        /// </summary>
        private void DestroyProjectile()
        {
            Destroy(gameObject);
        }
    }

}