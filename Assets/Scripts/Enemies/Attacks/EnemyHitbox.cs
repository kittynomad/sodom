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
        public event Action<IKillable, EnemyHitbox> OnHitEvent;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((CollisionLayer)collision.gameObject.layer == CollisionLayer.Player 
                && collision.TryGetComponent(out IKillable killable))
            {
                killable.OnDamage(damageAmount, gameObject);
                OnHitEvent?.Invoke(killable, this);
            }
        }
    }
}