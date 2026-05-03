/*****************************************************************************
// File Name : DrillheadDe.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Decided to try making the drillhead boss based on the concepts as a proof of concept for this AI system.
*****************************************************************************/
using UnityEngine;

[System.Serializable]
public class DrillheadDE : DecisionEngine
{
    public override EnemyBehavior Decide(EnemyBehavior currentState, EnemyController enemy)
    {
        // Should always be in the combat state by default.
        return enemy.GetState<TestCombatState>();
    }
}
