/*****************************************************************************
// File Name : EnemyOverlap.cs
// Author : Arcadia Koedderitz
// Creation Date : 6/20/2026
// Last Modified : 6/20/2026
//
// Brief Description : Detects when theh player is overlapping with an enemy.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;

namespace TFOOL
{
    public class EnemyOverlap : MonoBehaviour
    {
        private const string ENEMY_LAYER_NAME = "Enemy";

        [SerializeField] private SpriteRenderer debugSpriteRenderer;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Only collisions with the trigger on the enemy itself (not the hitbox or sensors) should trigger this.
            if (collision.gameObject.layer == LayerMask.NameToLayer(ENEMY_LAYER_NAME))
            {
                // Pierce add code here.
                Debug.Log("Touching enemy.");
                debugSpriteRenderer.color = Color.blue;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // Only collisions with the trigger on the enemy itself (not the hitbox or sensors) should trigger this.
            if (collision.gameObject.layer == LayerMask.NameToLayer(ENEMY_LAYER_NAME))
            {
                // Pierce add code here.
                Debug.Log("Not Touching enemy.");
                debugSpriteRenderer.color = Color.white;

            }
        }
    }
}