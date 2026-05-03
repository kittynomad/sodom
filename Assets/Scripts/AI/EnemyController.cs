/*****************************************************************************
// File Name : EnemyController.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Main component that controls enemies based on given states and a decision engine that controls
transitions between states.
*****************************************************************************/
using CustomAttributes;
using System;
using System.Threading;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeReference, ClassDropdown(typeof(DecisionEngine))] private DecisionEngine decisionEngine;
    [SerializeReference, ClassDropdown(typeof(EnemyBehavior))] private EnemyBehavior[] stateMachine;

    private EnemyBehavior currentState;
    private CancellationTokenSource stateCanceller;

    // AI data that will probably be move to a blackboard later.
    public GameObject Target {  get; set; }

    #region properties
    public Vector2 ToTarget => Target.transform.position - transform.position;
    #endregion

    // Set tot he first state by default.
    private void Start()
    {
        SetState(stateMachine[0]);
    }

    private void OnDestroy()
    {
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
        EnemyBehavior nextState = decisionEngine.Decide(currentState, this);
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
        // Notify the decision engine that a sense has been triggered.
        decisionEngine.OnSense(sensedObject, type, isSensed, this);

        // Query the decision engine.
        QueryDecisionEngine();
    }
    #endregion

    #region State Handling
    /// <summary>
    /// Sets the current minotaur state to a state of type T.
    /// </summary>
    /// <typeparam name="T">The type of state to transition to</typeparam>
    /// <returns>The new state.</returns>
    public T GetState<T>() where T : EnemyBehavior
    {
        T state = (T)Array.Find(stateMachine, item => item.GetType() == typeof(T));
        return state;
    }

    /// <summary>
    /// Sets the current minotaur state.
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

    /// <summary>
    /// Wrapper function is here to ensure that no errors slip through because of async.
    /// </summary>
    private async void StateRoutine()
    {
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
        T state = GetState<T>();
        SetState(state);
        return state;
    }
    #endregion

    #region Misc
    public void PointTowardsTarget()
    {
        Vector2 toTarget = Target.transform.position - transform.position;
        SetRotation(toTarget.x < 0);
    }
    public void SetRotation(bool facingLeft)
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, facingLeft ? 180 : 0,
            transform.localEulerAngles.z);
    }
    #endregion
}
