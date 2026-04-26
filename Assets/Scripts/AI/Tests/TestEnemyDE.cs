using UnityEngine;

[System.Serializable]
public class TestEnemyDE : DecisionEngine
{
    public override EnemyBehavior Decide(EnemyBehavior currentState, EnemyController enemy)
    {
        switch (currentState)
        {
            case PatrolState:
                // Move to combat if the enemy has a target.
                if (enemy.Target != null)
                {
                    Debug.Log("Sensed enemy.  Me Angy");
                    return enemy.GetState<TestCombatState>();
                }
                break;
        }
        return null;
    }
}
