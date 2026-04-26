/*****************************************************************************
// File Name : DecisionEngine.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Controls the ways a specific enemy transitions between it's different states.
*****************************************************************************/
using UnityEngine;

[System.Serializable]
public abstract class DecisionEngine
{
    /// <summary>
    /// Handles the enemy sensing objects and how it should react.
    /// </summary>
    /// <param name="sensedObject">The sensed object.</param>
    /// <param name="type">The type of sense the object triggered.</param>
    /// <param name="isSensed">Whether the enemy is currently sensed or is no longer sensed.</param>
    /// <param name="controller">The enemy this DecisionEngine is controlling.</param>
    public virtual void OnSense(GameObject sensedObject, SenseType type, bool isSensed, EnemyController controller)
    {
        // By default, set the aggro target when we recieve an aggro sense.
        switch (type)
        {
            // Set the enemy's target when the player is detected.
            case SenseType.Aggro:
                if (isSensed)
                {
                    controller.Target = sensedObject;
                }
                break;
        }
    }

    /// <summary>
    /// Makes a decision on the next enemy state that the enemy should move to based on it's properties.
    /// </summary>
    /// <param name="currentState">the current state of the enemy.</param>
    /// <param name="controller">The enemy controller script.</param>
    public abstract EnemyBehavior Decide(EnemyBehavior currentState, EnemyController controller);
}
