/*****************************************************************************
// File Name : RunningSliceAttack.cs
// Author : 
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;
using System.Threading;
using System;
using CustomAttributes;
using Unity.VisualScripting;

namespace TFOOL.Enemies.AI
{
    [System.Serializable]
    [DropdownGroup("Red Rooms/Stabby Guy")]
    public class RunningSliceAttack : EnemyAttack
    {
        [SerializeField] private GameObject hitbox;
        [SerializeField] private float attackTime;
        [SerializeField, Tooltip("Controls how close the enemy has to be to the player before it spawns the hitbox.")]
        private float attackRange;
        [Header("Movement")]
        [SerializeField] private float chargeSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float overshootTime;
        
        public override async Awaitable PerformAttack(EnemyController enemy, GameObject target, CancellationToken ct)
        {
            if (!enemy.TryGetComponent(out EnemyMovement movement))
            {
                throw new System.NullReferenceException($"Enemy {enemy} does not have a EnemyMovement component.");
            }

            ct.ThrowIfCancellationRequested();
            // Get Components

            float startingSpeed = movement.MoveSpeed;
            float startingAcceleration = movement.Acceleration;

            void CleanUp()
            {
                // Reset to defaults
                movement.MoveSpeed = startingSpeed;
                movement.Acceleration = startingAcceleration;
                movement.SetDirection(0);
                hitbox.SetActive(false);
            }
    
            try
            {
                // Behavior Logic.
                movement.MoveSpeed = chargeSpeed;
                movement.Acceleration = acceleration;

                // Move towards the player until within range.
                while(enemy.ToTarget.magnitude > attackRange)
                {
                    movement.SetDirection(enemy.DirectionToTarget);
                    await Awaitable.FixedUpdateAsync();
                }

                // Perform the attack.
                hitbox.SetActive(true);
                await Awaitable.WaitForSecondsAsync(attackTime, ct);
                hitbox.SetActive(false);

                // Enemy keeps moving.
                await Awaitable.WaitForSecondsAsync(overshootTime, ct);

                CleanUp();
            }
            catch (OperationCanceledException oce)
            {
                CleanUp();
                throw oce;
            }
    
        }
    }
}