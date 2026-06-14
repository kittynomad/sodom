/*****************************************************************************
// File Name : EnemyController.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 5/19/2026
//
// Brief Description : Main component that controls enemies based on the settings of an EnemyAI Object.
*****************************************************************************/
using System;
using System.Threading;
using UnityEngine;

namespace TFOOL.Enemies.AI
{
    public class EnemyController : MonoBehaviour
    {
        // Serialized Fields
        [SerializeField] private EnemyAI ai;

        private EnemyState currentState;
        private CancellationTokenSource stateCanceller;
        private IEnemySensor[] sensors;

        // AI Fields
        public GameObject Target { get; set; }

        #region Properties
        public Vector2 ToTarget => Target.transform.position - transform.position;
        #endregion

        private void Awake()
        {
            sensors = GetComponentsInChildren<IEnemySensor>();
            foreach (IEnemySensor sensor in sensors)
            {
                if (sensor != null)
                {
                    sensor.EntitySenseEvent += OnSense;
                }
            }
        }

        // Set to the first state by default.
        private void Start()
        {
            SetState(ai.GetState(0));
        }

        private void OnDestroy()
        {
            foreach (IEnemySensor sensor in sensors)
            {
                if (sensor != null)
                {
                    sensor.EntitySenseEvent -= OnSense;
                }
            }

            if (currentState != null)
            {
                if (stateCanceller != null)
                {
                    stateCanceller.Cancel();
                    stateCanceller = null;
                }
            }
        }

        #region Decision Engine

        /// <summary>
        /// Updates the enemy state based on the results of the decision engine.
        /// </summary>
        public void QueryDecisionEngine(bool isCancel)
        {
            EnemyState nextState = ai.QueryDecisionEngine(currentState, this);
            if (nextState != null)
            {
                SetStateInternal(nextState, isCancel);
            }
        }

        /// <summary>
        /// Reroutes all senses to the decision engine.
        /// </summary>
        /// <param name="sensedObject"></param>
        /// <param name="type"></param>
        /// <param name="isSensed"></param>
        public void OnSense(GameObject sensedObject, SenseType type, bool isSensed)
        {
            EnemyState newState = ai.OnSense(sensedObject, type, isSensed, this);
            // If the sense did not prompt a new state, then query the decision engine for any changes to state.
            if (newState != null)
            {
                SetState(newState);
            }
            else
            {
                // Query the decision engine.
                QueryDecisionEngine(true);
            }

            
        }
        #endregion

        #region State Handling
        /// <summary>
        /// Sets the current state.
        /// </summary>
        /// <param name="state">The state to set.</param>
        public bool SetState(EnemyState state)
        {
            return SetStateInternal(state, true);
        }

        private bool SetStateInternal(EnemyState state, bool isCancel)
        {
            if (state == currentState) { return false; }
            // Prevents uncancellable states (like stagger) from being interrupted.
            if (currentState != null && !currentState.IsCancellable && isCancel) { return false; }
            if (currentState != null)
            {
                if (stateCanceller != null)
                {
                    stateCanceller.Cancel();
                    stateCanceller = null;
                }
            }

            currentState = state;
            if (currentState != null)
            {
                stateCanceller = new CancellationTokenSource();
                StateRoutine();
            }
            return true;
        }

        private async void StateRoutine()
        {
            // Wrapper to catch async errors.
            try
            {
                await currentState.Run(this, stateCanceller.Token);
                if (!stateCanceller.Token.IsCancellationRequested)
                {
                    // The only non-cancel query or state set is if the state finishes naturally.
                    QueryDecisionEngine(false);
                }
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException)
                {
                    Debug.Log("Enemy operation cancelled");
                }
                else
                {
                    Debug.LogException(e);
                }
            }
        }

        /// <summary>
        /// Sets the current state by type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T SetState<T>() where T : EnemyState
        {
            T state = ai.GetState<T>();
            SetState(state);
            return state;
        }
        #endregion

        #region Misc
        /// <summary>
        /// Forces the enemy to point towards it's target, if any.
        /// </summary>
        public void PointTowardsTarget()
        {
            if (Target == null) { return; }
            Vector2 toTarget = Target.transform.position - transform.position;
            SetRotation(toTarget.x < 0);
        }

        /// <summary>
        /// Sets the rotation of the enemy.
        /// </summary>
        /// <param name="facingLeft">True if the enemy is facing left, false if otherwise.</param>
        public void SetRotation(bool facingLeft)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, facingLeft ? 180 : 0,
                transform.localEulerAngles.z);
        }
        #endregion
    }

}