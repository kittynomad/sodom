/*****************************************************************************
// File Name : DrillheadDe.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Decided to try making the drillhead boss based on the concepts as a proof of concept for this AI system.
*****************************************************************************/
using UnityEngine;

namespace Sodom.Enemies.AI.Tests
{
    [System.Serializable]
    public class DrillheadDE : DecisionEngine
    {
        public override void OnSense(GameObject sensedObject, SenseType type, bool isSensed, EnemyController controller)
        {

        }
        public override EnemyBehavior Decide(EnemyBehavior currentState, EnemyController enemy, EnemyAI ai)
        {
            // Should always be in the combat state by default.
            return ai.GetState<TestCombatState>();
        }
    }

}