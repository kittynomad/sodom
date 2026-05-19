/*****************************************************************************
// File Name : DecisionEngine.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 5/19/2026
//
// Brief Description : Controls the ways a specific enemy transitions between it's different states based on external 
// stimuli.
// Remarks: NOT for deciding which attack to use.  This is purely for deciding what to do in reaction to player actions.
*****************************************************************************/
using UnityEngine;

namespace Sodom.Enemies.AI
{
    [System.Serializable]
    public abstract class DecisionEngine
    {
        /// <summary>
        /// Handles the enemy sensing objects and how it should react.
        /// </summary>
        /// <param name="sensedObject">The sensed object.</param>
        /// <param name="type">The type of sense the object triggered.</param>
        /// <param name="isSensed">Whether the enemy is currently sensed or is no longer sensed.</param>
        /// <param name="enemyController">The enemy that this</param>
        public abstract void OnSense(GameObject sensedObject, SenseType type, bool isSensed, EnemyController enemyController);

        /// <summary>
        /// Makes a decision on the next enemy state that the enemy should move to based on it's properties.
        /// </summary>
        /// <param name="currentState">the current state of the enemy.</param>
        /// <param name="enemyController">The enemy that is facing the decision.</param>
        /// <param name="ai">The AI controller for this enemy.</param>
        public abstract EnemyBehavior Decide(EnemyBehavior currentState, EnemyController enemyController, EnemyAI ai);
    }

}