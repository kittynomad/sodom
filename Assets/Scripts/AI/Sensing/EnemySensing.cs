/*****************************************************************************
// File Name : EnemySensing.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Middleman script to handle routing senses to the enemy controller.
*****************************************************************************/
using UnityEngine;

public class EnemySensing : MonoBehaviour
{
    [SerializeField] private EnemyController controller;
    private EnemySensor[] sensors;

    private void Awake()
    {
        sensors = GetComponentsInChildren<EnemySensor>();
        foreach (EnemySensor sensor in sensors)
        {
            if (sensor != null)
            {
                sensor.EntitySensedEvent += OnSenseObject;
                sensor.EntityLostEvent += OnLoseObject;
            }
        }
    }

    private void OnDestroy()
    {
        foreach (EnemySensor sensor in sensors)
        {
            if (sensor != null)
            {
                sensor.EntitySensedEvent -= OnSenseObject;
                sensor.EntityLostEvent -= OnLoseObject;
            }
        }
    }

    /// <summary>
    /// Reroutes all child sensors through this central script.
    /// </summary>
    /// <param name="obj"></param>
    private void OnLoseObject(GameObject obj, SenseType sense)
    {
        controller.OnSense(obj, sense, false);
    }

    /// <summary>
    /// Reroutes all child sensors through this central script.
    /// </summary>
    /// <param name="obj"></param>
    private void OnSenseObject(GameObject obj, SenseType sense)
    {
        controller.OnSense(obj, sense, true);
    }
}
