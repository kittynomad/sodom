/*****************************************************************************
// File Name : EnemyHitbox.cs
// Author : Arcadia Koederitz
// Creation Date : 6/23/2026
// Last Modified : 6/23/2026
//
// Brief Description : Base sscript for enemy hitboxes and damaging areas.
*****************************************************************************/
using System;
using UnityEngine;

namespace TFOOL.Enemies
{
    public class EnemyHitbox : MonoBehaviour
    {
        [SerializeField] private float damageAmount;
        [SerializeField, Tooltip("Allows this enemy to hit other enemies that have this string in their FriendlyFireTags.")] 
        private FriendlyFireTags damagingTags;
        public event Action<IKillable, EnemyHitbox> OnHitEvent;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (CheckDamageable(collision, out IKillable killable))
            {
                killable.OnDamage(damageAmount, gameObject);
                OnHitEvent?.Invoke(killable, this);
            }
        }

        private bool CheckDamageable(Collider2D collision, out IKillable killable)
        {
            killable = null;
            bool isDamageable = ((CollisionLayer)collision.gameObject.layer == CollisionLayer.Player || // Check for player.
                (CollisionLayer)collision.gameObject.layer == CollisionLayer.Enemy) // Check for enemy.
                && collision.TryGetComponent(out killable) && // Get the health component.
                ((CollisionLayer)collision.gameObject.layer == CollisionLayer.Player || 
                (killable is EnemyHealth enemy && HasAnyFlags(enemy.FriendlyFireTags))); // If enemy, check if it's elidgable for friendly fire.
            return isDamageable;
        }

        private bool HasAnyFlags(FriendlyFireTags enemyTags)
        {
            Debug.Log($"Tags:  Hit Enemy: {(FriendlyFireTags)enemyTags}.  This: {(FriendlyFireTags)damagingTags}.  Combined: " + (FriendlyFireTags)(enemyTags & damagingTags));
            return (enemyTags & damagingTags) != 0;
        }
    }
}