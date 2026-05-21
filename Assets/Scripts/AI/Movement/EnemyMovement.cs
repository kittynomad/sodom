/*****************************************************************************
// File Name : EnemyMovement.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Middleman script to handle routing senses to the enemy controller.
*****************************************************************************/
using CustomAttributes;
using System;
using UnityEngine;

namespace Sodom.Enemies
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class EnemyMovement : MonoBehaviour
    {
        private const float WALL_CHECK_BOTTOM_MARGIN = 0.1f;
        private const float WALL_CHECK_TOP_MARGIN = -0.1f;
        private const float WALL_CHECK_LENGTH = 0.5f;
        private const float GROUND_CHECK_HEIGHT = 0.1f;
        private const float GROUND_CHECK_LENGTH = 0.5f;

        private const DetectedEdges LEFT_EDGES = DetectedEdges.LeftEdge | DetectedEdges.LeftWall;
        private const DetectedEdges RIGHT_EDGES = DetectedEdges.RightEdge | DetectedEdges.RightWall;

        [SerializeField, Tooltip("The collider used to determine the location of wall, edge, and ground checks.")]
        private Collider2D physicsCollider;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float acceleration;

        // Components
        [SerializeField, ShowIfNull] private Rigidbody2D rb;

        private int targetDirection;

        [SerializeField, ReadOnly] private bool onGround;
        [SerializeField, ReadOnly] private DetectedEdges edges;

        public event Action<DetectedEdges> DetectEdgeEvent;
        public event Action<bool> OnGroundEvent;

        #region Properties
        public Rigidbody2D Rigidbody => rb;
        public float MoveSpeed { get => walkSpeed; set => walkSpeed = value; }
        public float Acceleration { get => acceleration; set => acceleration = value; }
        public DetectedEdges Edges => edges;
        private Bounds enemyBounds => physicsCollider.bounds;

        private LayerMask GroundMask => LayerMask.GetMask("Ground");
        public bool OnGround
        {
            get => onGround;
            set
            {
                if (onGround != value)
                {
                    onGround = value;
                    OnGroundEvent?.Invoke(onGround);
                }
            }
        }
        #endregion

        #region Nested
        [Flags]
        public enum DetectedEdges
        {
            None = 0,
            LeftWall = 1 << 0,
            RightWall = 1 << 1,
            LeftEdge = 1 << 2,
            RightEdge = 1 << 3
        }
        #endregion

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

            CheckEdges();
        }

        /// <summary>
        /// Gets a vector point at the outer edge of the enemy's bounds.
        /// </summary>
        /// <param name="sign">The side of the bounds to get.</param>
        /// <param name="height">The height from the bottom of the enemy's bounds to get.</param>
        /// <returns></returns>
        private Vector2 GetEdgeVectorMin(int sign, float height)
        {
            return new Vector2(enemyBounds.center.x + (enemyBounds.size.x / 2 * sign), enemyBounds.min.y + height);
        }

        private Vector2 GetEdgeVector(int sign)
        {
            return new Vector2(enemyBounds.center.x + (enemyBounds.size.x / 2 * sign), enemyBounds.center.y);
        }

        #region Ground Checking
        private DetectedEdges CheckGroundAndEdge()
        {
            bool onGround = false;
            DetectedEdges edges = 0;
            for (int i = -1; i <= 1; i += 2)
            {
                Vector2 wallCheckVector = GetEdgeVectorMin(i, GROUND_CHECK_HEIGHT);
                RaycastHit2D hit = Physics2D.Raycast(wallCheckVector, Vector2.down, GROUND_CHECK_LENGTH, GroundMask);
                if (!hit)
                {
                    DetectedEdges addedEdge = i < 0 ? DetectedEdges.LeftEdge : DetectedEdges.RightEdge;
                    edges |= addedEdge;
                }
                onGround |= hit;
            }

            // Use the edge checks for on ground.  If one of them hits, then the enemy is on ground.
            OnGround = onGround;
            if(!onGround)
            {
                // If the enemy is not on ground, then they should detect no edges.
                return 0;
            }

            return edges;
            
        }
        #endregion

        #region Wall Detection
        private Bounds GetWallCheckBounds(int direction)
        {
            Vector2 center = GetEdgeVector(direction) + new Vector2(direction * WALL_CHECK_LENGTH / 2, 
                WALL_CHECK_BOTTOM_MARGIN / 2 - WALL_CHECK_TOP_MARGIN / 2);
            Vector2 size = new Vector2(WALL_CHECK_LENGTH, enemyBounds.size.y - WALL_CHECK_BOTTOM_MARGIN - WALL_CHECK_TOP_MARGIN);
            return new Bounds(center, size);
        }

        private DetectedEdges CheckWall()
        {
            DetectedEdges edges = 0;
            for (int i = -1; i <= 1; i += 2)
            {
                Bounds wallCheckBox = GetWallCheckBounds(i);
                RaycastHit2D hit = Physics2D.BoxCast(wallCheckBox.center, wallCheckBox.size, 0, Vector2.right * i, 0, GroundMask);
                if (hit)
                {
                    DetectedEdges addedEdge = i < 0 ? DetectedEdges.LeftWall : DetectedEdges.RightWall;
                    edges |= addedEdge;
                }
            }
            return edges;
        }
        #endregion
        #region Edge Detection

        private void CheckEdges()
        {
            DetectedEdges newEdges = 0;

            newEdges |= CheckWall();
            newEdges |= CheckGroundAndEdge();

            if (newEdges != edges)
            {
                edges = newEdges;
                DetectEdgeEvent?.Invoke(edges);
            }
        }

        /// <summary>
        /// Checks if the enemy has detected an edge on their left side.
        /// </summary>
        /// <param name="edges">The information about this eneny's detected edges.</param>
        /// <returns>True if the enemy has detected an edge on the left side.</returns>
        public static bool CheckLeft(DetectedEdges edges)
        {
            // If no left edges are present in the edges flag, then the value will be 0.
            return (edges & LEFT_EDGES) != 0;
        }

        /// <summary>
        /// Checks if the enemy has detected an edge on their right side.
        /// </summary>
        /// <param name="edges">The information about this eneny's detected edges.</param>
        /// <returns>True if the enemy has detected an edge on the right side.</returns>
        public static bool CheckRight(DetectedEdges edges)
        {
            // If no right edges are present in the edges flag, then the value will be 0.
            return (edges & RIGHT_EDGES) != 0;
        }
        #endregion

        /// <summary>
        /// Draw gizmos for visualizing wall and edge/ground checks.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            // Draw Wall Check.
            for(int i = -1; i <= 1; i += 2)
            {
                Bounds wallCheckBounds = GetWallCheckBounds(i);
                Gizmos.DrawWireCube(wallCheckBounds.center, wallCheckBounds.size);
            }
            // Draw Ground/Edge Check.
            for (int i = -1; i <= 1; i += 2)
            {
                Vector2 groundCheckVector = GetEdgeVectorMin(i, GROUND_CHECK_HEIGHT);
                Gizmos.DrawLine(groundCheckVector, groundCheckVector + Vector2.down * GROUND_CHECK_LENGTH);
            }
        }
    }

}