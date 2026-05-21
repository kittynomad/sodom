using Sodom.Enemies.AI;
using Sodom.Enemies.AI.Tests;
using UnityEngine;

namespace Sodom.Enemies.AI.Tests
{
    [System.Serializable]
    public class TestEnemyDE : DecisionEngine
    {
        public override void OnSense(GameObject sensedObject, SenseType type, bool isSensed, EnemyController controller)
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
                    else if (controller.Target == sensedObject)
                    {
                        // Lose target when they exit the sense range.
                        controller.Target = null;
                    }

                    break;
                case SenseType.LoseAggro:

                    break;
            }
        }
        public override EnemyBehavior Decide(EnemyBehavior currentState, EnemyController enemy, EnemyAI ai)
        {
            switch (currentState)
            {
                case PatrolState:
                    // Move to combat if the enemy has a target.
                    if (enemy.Target != null)
                    {
                        Debug.Log("Sensed enemy.  Me Angy");
                        return ai.GetState<TestCombatState>();
                    }
                    break;
                case TestCombatState:
                    if (enemy.Target == null)
                    {
                        Debug.Log("Lost Enemy.");
                        return ai.GetState<PatrolState>();
                    }
                    break;
            }
            return null;
        }
    }

}