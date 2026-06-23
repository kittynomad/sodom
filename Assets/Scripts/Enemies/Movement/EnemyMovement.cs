/*****************************************************************************
// File Name : EnemyMovement.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Middleman script to handle routing senses to the enemy controller.
*****************************************************************************/
using CustomAttributes;
using NaughtyAttributes;
using System;
using System.Threading;
using TFOOL.Enemies.AI;
using UnityEngine;

namespace TFOOL.Enemies
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(EnemyController))]
    public class EnemyMovement : MonoBehaviour
    {
        #region CONSTS
        private const float WALL_CHECK_BOTTOM_MARGIN = 0.1f;
        private const float WALL_CHECK_TOP_MARGIN = -0.1f;
        private const float WALL_CHECK_LENGTH = 0.8f;
        private const float GROUND_CHECK_HEIGHT = 0.1f;
        private const float GROUND_CHECK_LENGTH = 0.15f;

        private const DetectedBlockers EDGES = DetectedBlockers.LeftEdge | DetectedBlockers.RightEdge;
        private const DetectedBlockers WALLS = DetectedBlockers.RightWall | DetectedBlockers.LeftWall;

        private const DetectedBlockers LEFT_EDGES = DetectedBlockers.LeftEdge | DetectedBlockers.LeftWall;
        private const DetectedBlockers RIGHT_EDGES = DetectedBlockers.RightEdge | DetectedBlockers.RightWall;

        private const float JUMP_LEEWAY = 1f;
        #endregion

        [SerializeField, Tooltip("The collider used to determine the location of wall, edge, and ground checks.")]
        private Collider2D physicsCollider;
        [SerializeField] private DirectionUpdateMode directionUpdateMode;
        [Header("Movement")]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float groundAcceleration;
        [SerializeField] private float airAcceleration;
        [Header("Jumping")]
        [SerializeField] private bool jumpOverGaps;
        [SerializeField] private bool jumpUpLedges;
        [SerializeField] private int maxJumpDistance;
        [SerializeField] private int maxJumpHeight;
        [SerializeField] private float jumpWindupTime;
        [SerializeField] private float jumpLagTime;

        // Components
        [SerializeField, ShowIfNull] private Rigidbody2D rb;
        [SerializeField, ShowIfNull] private EnemyController controller;

        private int targetDirection;
        private bool pauseGroundCheck;

        [ShowNonSerializedField, NaughtyAttributes.ReadOnly] private bool onGround;
        [ShowNonSerializedField, NaughtyAttributes.ReadOnly] private DetectedBlockers blockers;

        public event Action<DetectedBlockers> DetectEdgeEvent;
        public event Action<bool> OnGroundEvent;

        #region Properties
        #region Settings
        public float MoveSpeed { get => walkSpeed; set => walkSpeed = value; }
        public float Acceleration { get => groundAcceleration; set => groundAcceleration = value; }
        public DetectedBlockers Blockers => blockers;
        private DirectionUpdateMode DirectionMode => directionUpdateMode;
        public bool JumpOverGaps { get => jumpOverGaps; set => jumpOverGaps = value; }
        public int MaxJumpDistance { get => maxJumpDistance; set => maxJumpDistance = value; }
        public int MaxJumpHeight { get => maxJumpHeight; set => maxJumpHeight = value; }
        #endregion

        public Rigidbody2D Rigidbody => rb;
        private Bounds EnemyBounds => physicsCollider.bounds;
        public int TargetDirection => targetDirection;

        private LayerMask GroundMask => 1 << (int)CollisionLayer.Ground; // Bit shift for layer mask.
        public bool OnGround
        {
            get => onGround;
            set
            {
                if (onGround != value)
                {
                    onGround = value;
                    OnGroundEvent?.Invoke(onGround);
                    if (onGround == false)
                    {
                        pauseGroundCheck = true;
                    }
                }
            }
        }
        #endregion

        #region Nested
        [Flags]
        public enum DetectedBlockers
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
            controller = GetComponent<EnemyController>();
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
                if (controller != null && DirectionMode == DirectionUpdateMode.TargetDirection)
                {
                    controller.FacingDirection = direction;
                }
            }
        }

        private void FixedUpdate()
        {
            // Move towards the desired velocity.
            float horizontalSpeed = rb.linearVelocity.x;
            float acceleration = onGround ? groundAcceleration : airAcceleration;
            horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, walkSpeed * targetDirection, acceleration * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector2(horizontalSpeed, rb.linearVelocity.y);

            if (DirectionMode == DirectionUpdateMode.Velocity)
            {
                controller.FacingDirection = (int)Mathf.Sign(horizontalSpeed);
            }
            else if (DirectionMode == DirectionUpdateMode.ToTarget)
            {
                controller.PointTowardsTarget();
            }

            CheckBlockers();
        }

        public void StopVelocity()
        {
            Rigidbody.linearVelocity = Vector2.zero;
        }

        public void StopHorizontalVelocty()
        {
            Rigidbody.linearVelocity = new Vector2(0, Rigidbody.linearVelocityY);
        }

        #region Ground Checking
        private DetectedBlockers CheckGroundAndEdge()
        {
            bool onGround = false;
            DetectedBlockers edges = 0;
            for (int i = -1; i <= 1; i += 2)
            {
                Vector2 wallCheckVector = GetEdgeVectorMin(i, GROUND_CHECK_HEIGHT);
                RaycastHit2D hit = Physics2D.Raycast(wallCheckVector, Vector2.down, GROUND_CHECK_LENGTH, GroundMask);
                if (!hit)
                {
                    DetectedBlockers addedEdge = i < 0 ? DetectedBlockers.LeftEdge : DetectedBlockers.RightEdge;
                    edges |= addedEdge;
                }
                onGround |= hit;
            }

            // Use the edge checks for on ground.  If one of them hits, then the enemy is on ground.
            if (!pauseGroundCheck)
            {
                OnGround = onGround;
                pauseGroundCheck = false;
            }
            if(!onGround)
            {
                pauseGroundCheck = false;
                // If the enemy is not on ground, then they should detect no edges.
                return 0;
            }

            return edges;
            
        }
        #endregion

        #region Position Getting
        /// <summary>
        /// Gets a vector point at the outer edge of the enemy's bounds.
        /// </summary>
        /// <param name="sign">The side of the bounds to get.</param>
        /// <param name="height">The height from the bottom of the enemy's bounds to get.</param>
        /// <returns></returns>
        private Vector2 GetEdgeVectorMin(int sign, float height)
        {
            return new Vector2(EnemyBounds.center.x + (EnemyBounds.size.x / 2 * sign), EnemyBounds.min.y + height);
        }

        private Vector2 GetEdgeVector(int sign)
        {
            return new Vector2(EnemyBounds.center.x + (EnemyBounds.size.x / 2 * sign), EnemyBounds.center.y);
        }

        private Vector2 GetFeetPosition()
        {
            return Rigidbody.position + new Vector2(0, EnemyBounds.min.y);
        }
        #endregion

        #region Blocker Detection

        private void CheckBlockers()
        {
            DetectedBlockers newEdges = 0;

            newEdges |= CheckWall();
            newEdges |= CheckGroundAndEdge();

            if (newEdges != blockers)
            {
                blockers = newEdges;
                DetectEdgeEvent?.Invoke(blockers);
                //// If the enemy is set to jump, check for a valid jump spot then begin the jump state.
                //if (OnGround)
                //{
                //    if (ContainsEdges(blockers))
                //    {
                        
                //        if (jumpOverGaps)
                //        {
                //            Jump(TargetDirection);
                //        }
                //    }
                //    else if (jumpUpLedges && ContainsWall(blockers))
                //    {
                //        Jump(TargetDirection);
                //    }
                //}
            }
        }

        #region Wall Detection
        private Bounds GetWallCheckBounds(int direction)
        {
            Vector2 center = GetEdgeVector(direction) + new Vector2(direction * WALL_CHECK_LENGTH / 2,
                WALL_CHECK_BOTTOM_MARGIN / 2 - WALL_CHECK_TOP_MARGIN / 2);
            Vector2 size = new Vector2(WALL_CHECK_LENGTH, EnemyBounds.size.y - WALL_CHECK_BOTTOM_MARGIN - WALL_CHECK_TOP_MARGIN);
            return new Bounds(center, size);
        }

        private DetectedBlockers CheckWall()
        {
            DetectedBlockers edges = 0;
            for (int i = -1; i <= 1; i += 2)
            {
                Bounds wallCheckBox = GetWallCheckBounds(i);
                RaycastHit2D hit = Physics2D.BoxCast(wallCheckBox.center, wallCheckBox.size, 0, Vector2.right * i, 0, GroundMask);
                if (hit && OnGround)
                {
                    DetectedBlockers addedEdge = i < 0 ? DetectedBlockers.LeftWall : DetectedBlockers.RightWall;
                    edges |= addedEdge;
                }
            }
            return edges;
        }
        #endregion

        #region Checks
        private static bool ContainsEdges(DetectedBlockers blockers)
        {
            return (blockers & EDGES) != 0;
        }

        private static bool ContainsWall(DetectedBlockers edges)
        {
            return (edges & WALLS) != 0;
        }

        /// <summary>
        /// Checks if the enemy has detected an edge on their left side.
        /// </summary>
        /// <param name="edges">The information about this eneny's detected edges.</param>
        /// <returns>True if the enemy has detected an edge on the left side.</returns>
        public static bool CheckLeft(DetectedBlockers edges)
        {
            // If no left edges are present in the edges flag, then the value will be 0.
            return (edges & LEFT_EDGES) != 0;
        }

        /// <summary>
        /// Checks if the enemy has detected an edge on their right side.
        /// </summary>
        /// <param name="edges">The information about this eneny's detected edges.</param>
        /// <returns>True if the enemy has detected an edge on the right side.</returns>
        public static bool CheckRight(DetectedBlockers edges)
        {
            // If no right edges are present in the edges flag, then the value will be 0.
            return (edges & RIGHT_EDGES) != 0;
        }

        /// <summary>
        /// Checks if the enemy ran into an edge in a specific direction.
        /// </summary>
        /// <param name="isLeft">Whether to check for left or right.</param>
        /// <param name="edges">The edges the enemy has encountered.</param>
        /// <returns></returns>
        public static bool CheckDestinationObscured(bool isLeft, EnemyMovement.DetectedBlockers edges)
        {
            return isLeft ? EnemyMovement.CheckLeft(edges) : EnemyMovement.CheckRight(edges);
        }

        public bool IsDestinationObscured()
        {
            return CheckDestinationObscured(TargetDirection < 0, Blockers);
        }
        #endregion
        #endregion

        #region Jumping
        public async Awaitable<bool> IsObscuredTryJump(CancellationToken ct)
        {
            return await IsObscuredTryJump(targetDirection, ct);
        }
        /// <summary>
        /// Checks if the way to the enemy's destination is obscured, and automatically try jumping if it is.
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="direction">The direction that the enemy should try jumping int.</param>
        /// <returns>True if the enemy cannot reach their destination, false if their destination is not obscured or their jump succeeded.</returns>
        public async Awaitable<bool> IsObscuredTryJump(int direction, CancellationToken ct)
        {
            if (IsDestinationObscured())
            {
                return !await TryJump(direction, ct);
            }
            return false;
        }

        /// <summary>
        /// Async jump function that can be called from enemy AI states.
        /// </summary>
        /// <returns></returns>
        public async Awaitable<bool> TryJump(CancellationToken ct)
        {
            return await TryJump(targetDirection, ct);
        }

        public async Awaitable<bool> TryJump(int jumpDirection, CancellationToken ct)
        {
            // Check to see if jumping is valid.
            if (GetJumpPoint(jumpDirection, out Vector2 jumpPoint))
            {
                try
                {
                    SetDirection(0);

                    // If the enemy is hugging a wall, make them back up a bit.
                    if (ContainsWall(blockers))
                    {
                        Vector2 backUpPos = Rigidbody.position - new Vector2(jumpDirection, 0);
                        SetDirection(-jumpDirection);
                        while(Vector2.Distance(backUpPos, Rigidbody.position) > 0.1f)
                        {
                            Debug.Log(Vector2.Distance(backUpPos, Rigidbody.position));
                            await Awaitable.FixedUpdateAsync(ct);
                        }
                        SetDirection(0);
                    }

                    await Awaitable.WaitForSecondsAsync(jumpWindupTime, ct);

                    // While jumping, the enemy is not blocked.
                    OnGround = false;
                    blockers = 0;
                    // Calculate the correct velocity to jump at to reach the jump point.
                    Vector2 jumpVelocity = GetJumpVelocity(GetFeetPosition(), jumpPoint);
                    Rigidbody.linearVelocity = jumpVelocity;

                    // Wait until the enemy has hit the ground again.
                    while (!OnGround)
                    {
                        await Awaitable.FixedUpdateAsync(ct);
                    }

                    await Awaitable.WaitForSecondsAsync(jumpLagTime, ct);

                    SetDirection(jumpDirection);

                    return true;
                }
                catch (OperationCanceledException oce)
                {
                    // Do any needed cleanup here.
                    throw oce;
                } 
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Perform a sequence of raycasts to check for a valid jump point.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="jumpPoint"></param>
        /// <returns></returns>
        private bool GetJumpPoint(int direction, out Vector2 jumpPoint)
        {
            Vector2 raycastPoint = GetEdgeVector(direction) + new Vector2(WALL_CHECK_LENGTH * direction, 0);
            for(int i = 0; i < maxJumpDistance; i++)
            {
                Vector2 origin = raycastPoint + (direction * i * Vector2.right) + (Vector2.up * maxJumpHeight);
                RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, maxJumpHeight * 2, GroundMask);
                
                if (hit)
                {
                    // If valid ground was hit, return true and set the hit point as the jump point/
                    Debug.DrawRay(origin, Vector2.down * maxJumpHeight * 2, Color.red, 5);
                    Debug.DrawLine(origin, hit.point, Color.green, 5);
                    jumpPoint = hit.point;
                    return true;
                }
                else
                {
                    Debug.DrawRay(origin, Vector2.down * maxJumpHeight * 2, Color.green, 5);
                }
            }
            jumpPoint = Vector2.zero;
            return false;
        }

        /// <summary>
        /// Uses projectile motion to calculatae the velocity needed for the enemy to reach the set jump point.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="jumpPoint"></param>
        /// <returns></returns>
        private Vector2 GetJumpVelocity(Vector2 position, Vector2 jumpPoint)
        {
            Vector2 deltaPosition = jumpPoint - position;
            float gravity = Rigidbody.gravityScale * Physics2D.gravity.y;

            float verticalVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * (Mathf.Max(deltaPosition.y + JUMP_LEEWAY, JUMP_LEEWAY)));
            // Calc horizontal velocity using the formula vx = dx / ((-vy / g) + sqrt(2l/g))
            float horizontalVelocity = deltaPosition.x / ((-verticalVelocity / gravity) + Mathf.Sqrt(2 * JUMP_LEEWAY / Mathf.Abs(gravity)));
            Debug.Log(deltaPosition.y);
            return new Vector2(horizontalVelocity, verticalVelocity);

        }
        #endregion

        /// <summary>
        /// Draw gizmos for visualizing wall and edge/ground checks.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (physicsCollider != null)
            {
                Gizmos.color = Color.green;
                // Draw Wall Check.
                for (int i = -1; i <= 1; i += 2)
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

}