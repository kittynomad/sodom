using UnityEngine;

namespace Sodom.Enemies.AI.Tests
{
    [System.Serializable]
    public class TestEnemyDE : DecisionEngine
    {
        public override EnemyState OnSense(GameObject sensedObject, SenseType type, bool isSensed, EnemyController enemy, EnemyAI ai)
        {
            // By default, set the aggro target when we recieve an aggro sense.
            switch (type)
            {
                // Set the enemy's target when the player is detected.
                case SenseType.Aggro:
                    if (isSensed)
                    {
                        enemy.Target = sensedObject;
                    }
                    else if (enemy.Target == sensedObject)
                    {
                        // Lose target when they exit the sense range.
                        enemy.Target = null;
                    }

                    break;
            }
            return null;
        }
        public override EnemyState Decide(EnemyState currentState, EnemyController enemy, EnemyAI ai)
        {
            switch (currentState)
            {
                case PatrolState:
                    // Move to combat if the enemy has a target.
                    if (enemy.Target != null)
                    {
                        Debug.Log("Sensed enemy.  Me Angy");
                        return ai.GetState<EnterCombatState>();
                    }
                    break;
                case EnterCombatState:
                    if (enemy.Target != null)
                    {
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
                default:
                    if (enemy.Target == null)
                    {
                        return ai.GetState<PatrolState>();
                    }
                    else
                    {
                        return ai.GetState<TestCombatState>();
                    }
            }
            return null;
        }
    }

}