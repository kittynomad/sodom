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

namespace Sodom.Enemies.AI
{
    public class EnemyController : MonoBehaviour
    {
        // Serialized Fields
        [SerializeField] private EnemyAI ai;

        private EnemyBehavior currentState;
        private CancellationTokenSource stateCanceller;
        private EnemySensor[] sensors;

        // AI Fields
        public GameObject Target { get; set; }

        #region Properties
        public Vector2 ToTarget => Target.transform.position - transform.position;
        #endregion

        private void Awake()
        {
            sensors = GetComponentsInChildren<EnemySensor>();
            foreach (EnemySensor sensor in sensors)
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
            foreach (EnemySensor sensor in sensors)
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
        public void QueryDecisionEngine()
        {
            EnemyBehavior nextState = ai.QueryDecisionEngine(currentState, this);
            if (nextState != null)
            {
                SetState(nextState);
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
            ai.OnSense(sensedObject, type, isSensed, this);

            // Query the decision engine.
            QueryDecisionEngine();
        }
        #endregion

        #region State Handling
        /// <summary>
        /// Sets the current state.
        /// </summary>
        /// <param name="state">The state to set.</param>
        public void SetState(EnemyBehavior state)
        {
            if (state == currentState) { return; }
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
        }

        private async void StateRoutine()
        {
            // Wrapper to catch async errors.
            try
            {
                await currentState.Run(this, stateCanceller.Token);
                if (!stateCanceller.Token.IsCancellationRequested)
                {
                    QueryDecisionEngine();
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
        public T SetState<T>() where T : EnemyBehavior
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