/*****************************************************************************
// File Name : EnemyMovement.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Middleman script to handle routing senses to the enemy controller.
*****************************************************************************/
using CustomAttributes;
using UnityEngine;

namespace Sodom.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float walkSpeed;
        [SerializeField] private float acceleration;

        // Components
        [SerializeField, ShowIfNull] private Rigidbody2D rb;

        private int targetDirection;

        public Rigidbody2D Rigidbody => rb;
        public float MoveSpeed { get => walkSpeed; set => walkSpeed = value; }
        public float Acceleration { get => acceleration; set => acceleration = value; }

        private void Reset()
        {
            // Get components automatically on reset.
            rb = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Sets the enemy's direction.
        /// </summary>
        /// <param name="direction"></param>
        public void SetDirection(int direction)
        {
            direction = Mathf.Clamp(direction, -1, 1);
            if (direction != targetDirection)
            {
                targetDirection = direction;
                // Invert speed so that the enemy is moving the other direction.
                //rb.linearVelocity = new Vector2(-rb.linearVelocity.x, rb.linearVelocity.y);
            }
        }

        private void FixedUpdate()
        {
            // Move towards the desired velocity.
            //float horizontalSpeed = rb.linearVelocity.x;
            //horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, walkSpeed * targetDirection, acceleration * Time.fixedDeltaTime);
            //rb.linearVelocity = new Vector2(horizontalSpeed, rb.linearVelocity.y);

            // Using the same code as player for consistency.
            if (Mathf.Abs(rb.linearVelocityX) < walkSpeed)
                rb.AddForce(new Vector2(targetDirection * acceleration, 0f));

            // TODO: Prevent walking off the edge.

            // TODO: Jump when a wall is hit.
        }
    }

}