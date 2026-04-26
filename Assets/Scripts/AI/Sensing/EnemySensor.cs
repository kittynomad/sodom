/*****************************************************************************
// File Name : EnemySensor.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Utilizes trigger colliders to detect the player and/or other targets.
*****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class EnemySensor : MonoBehaviour
{
    #region CONSTS
    // Need to implement other forms of aggression later.
    private const string PLAYER_TAG = "Player";
    #endregion

    [SerializeField] private SenseType senseType;
    [SerializeField] private bool requireLOS;
    [SerializeField] private LayerMask detectionMask;

    private readonly List<GameObject> monitoredObjects = new();
    private readonly List<GameObject> sensedObjects = new();

    private bool isMonitoring;

    #region Events
    public event Action<GameObject, SenseType> EntitySensedEvent;
    public event Action<GameObject, SenseType> EntityLostEvent;
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsSensable(collision.gameObject))
        {
            if (requireLOS)
            {
                monitoredObjects.Add(collision.gameObject);
                if (monitoredObjects.Count == 1 && !isMonitoring)
                {
                    isMonitoring = true;
                    StartCoroutine(MonitorObjectsRoutine());
                }
            }
            else
            {
                SenseObject(collision.gameObject);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsSensable(collision.gameObject))
        {
            monitoredObjects.Remove(collision.gameObject);
            if (sensedObjects.Contains(collision.gameObject))
            {
                LoseObject(collision.gameObject);
            }
        }
    }

    private IEnumerator MonitorObjectsRoutine()
    {
        while (monitoredObjects.Count > 0)
        {
            for (int i = 0; i < monitoredObjects.Count; i++)
            {
                
                // Raycast to the monitored object.
                Vector2 toObj = monitoredObjects[i].transform.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toObj.normalized, Mathf.Infinity, detectionMask);
                // If the player was detected, mark it as a found entity.
                if (hit.collider != null
                    && hit.collider.gameObject == monitoredObjects[i])
                {
                    if (!sensedObjects.Contains(monitoredObjects[i]))
                    {
                        SenseObject(monitoredObjects[i]);
                    }
                }
                else
                {
                    LoseObject(monitoredObjects[i]);
                }
            }

            yield return new WaitForFixedUpdate();
        }
        isMonitoring = false;
    }

    private void SenseObject(GameObject obj)
    {
        EntitySensedEvent?.Invoke(obj, senseType);
        sensedObjects.Add(obj);
    }

    private void LoseObject(GameObject obj)
    {
        sensedObjects.Remove(obj);
        EntityLostEvent?.Invoke(obj, senseType);
    }

    // Checks if a collided GO is sensable.
    private bool IsSensable(GameObject obj)
    {
        return obj.CompareTag(PLAYER_TAG);
    }

    // Gizmo for visualizing what entities the enemy can see.
    private void OnDrawGizmos()
    {
        Vector3[] points = new Vector3[sensedObjects.Count * 2];
        for (int i = 0; i < sensedObjects.Count; i++)
        {
            points[i * 2] = transform.position;
            points[(i * 2) + 1] = sensedObjects[i].transform.position;
        }

        foreach (GameObject obj in sensedObjects)
        {
            Gizmos.DrawLineList(points);
        }
    }
}
