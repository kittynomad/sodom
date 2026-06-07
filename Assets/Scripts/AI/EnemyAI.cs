/*****************************************************************************
// File Name : EnemyAI.cs
// Author : Arcadia Koederitz
// Creation Date : 5/19/2026
// Last Modified : 5/19/2026
//
// Brief Description : Object that controls all the settings of how an enemy acts.
// Done this way to allow for 1 object that controls all enemies of a certain type, instead of each enemy having it's
// own copy of the AI.
*****************************************************************************/
using CustomAttributes;
using System;
using UnityEngine;

namespace Sodom.Enemies.AI
{
    [CreateAssetMenu(fileName = "EnemyAI", menuName = "Scriptable Objects/Enemy AI")]
    public class EnemyAI : ScriptableObject
    {
        [SerializeReference, ClassDropdown(typeof(DecisionEngine))] private DecisionEngine decisionEngine;
        [SerializeReference, ClassDropdown(typeof(EnemyState))] private EnemyState[] stateMachine;

        /// <summary>
        /// Gets a state by type.
        /// </summary>
        /// <typeparam name="T">The type of state to get</typeparam>
        /// <returns>The found state from the state machine,.</returns>
        public T GetState<T>() where T : EnemyState
        {
            T state = (T)Array.Find(stateMachine, item => item.GetType() == typeof(T));
            return state;
        }
        /// <summary>
        /// Gets a state by index.
        /// </summary>
        /// <param name="index">The index of the state to get.</param>
        /// <returns>The found state from the state machine,.</returns>
        public EnemyState GetState(int index)
        {
            return stateMachine[index];
        }

        /// <summary>
        /// Queries the decision engine for a new state based on the status of the enemy.
        /// </summary>
        /// <param name="currentState">The current state the enemy is in.</param>
        /// <param name="enemy">The enemy to get a new state for.</param>
        /// <returns>The new state decided by the DecisionEngine.</returns>
        public EnemyState QueryDecisionEngine(EnemyState currentState, EnemyController enemy)
        {
            return decisionEngine.Decide(currentState, enemy, this);
        }

        /// <summary>
        /// Handles this enemy's reaction to a sense based on the decision engine.
        /// </summary>
        /// <param name="sensedObj">The object that was sensed.</param>
        /// <param name="type">The type of sense that was triggered.</param>
        /// <param name="isSensed">If the sense is active or lost.</param>
        /// <param name="enemy">The enemy that sensed the object.</param>
        public EnemyState OnSense(GameObject sensedObj, SenseType type, bool isSensed, EnemyController enemy)
        {
            // Notify the decision engine that a sense has been triggered.
            return decisionEngine.OnSense(sensedObj, type, isSensed, enemy, this);
        }
    }

}