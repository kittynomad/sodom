/*****************************************************************************
// File Name : EnemySensor.cs
// Author : Arcadia Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Utilizes trigger colliders to detect the player and/or other targets.
*****************************************************************************/
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sodom.Enemies.AI
{
    [RequireComponent(typeof(Collider2D))]
    public class ObjectSensor : MonoBehaviour, IEnemySensor
    {
        #region CONSTS
        // Need to implement other forms of aggression later.
        private const string PLAYER_TAG = "Player";
        #endregion

        [SerializeField, Tooltip("What type of sense this is.  Used to differentiate between different sensors " +
            "in the DecisionEngine.")] 
        private SenseType senseType;
        [SerializeField, Tooltip("How long the target has to be undetectable by this sense before the enemy " +
            "loses track of it.")] 
        private float senseExpireTime;
        [SerializeField, Tooltip("The maximum height that the enemy can see.  Set 0 for no max height.")] 
        private float maxHeightDiff;
        [SerializeField, Tooltip("Whether the enemy requires line fo sight to detect an objecct with this sense.")] 
        private bool requireLOS;
        [SerializeField, ShowIf(nameof(requireLOS))] private bool debugLOS;
        [SerializeField] private LayerMask detectionMask;

        private readonly List<GameObject> monitoredObjects = new();
        private readonly List<GameObject> sensedObjects = new();
        private readonly Dictionary<GameObject, Coroutine> expiringSenses = new();

        private bool isMonitoring;

        #region Events
        public event Action<GameObject, SenseType, bool> EntitySenseEvent;
        #endregion

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsSensable(collision.gameObject))
            {
                if (requireLOS || maxHeightDiff > 0)
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
                    StopSensingObject(collision.gameObject);
                }
            }
        }

        private IEnumerator MonitorObjectsRoutine()
        {
            while (monitoredObjects.Count > 0)
            {
                for (int i = 0; i < monitoredObjects.Count; i++)
                {
                    bool breakSense = false;
                    
                    if (maxHeightDiff > 0)
                    {
                        // Check difference in height.
                        breakSense |= !CheckHeightDiff(i);
                    }

                    if (requireLOS)
                    {
                        // Raycast to the monitored object.
                        breakSense |= !CheckLOS(i);
                    }
                    

                    // If all sense factors return true, set the object as sensed.
                    if (!breakSense)
                    {
                        SenseObject(monitoredObjects[i]);
                    }
                    else
                    {
                        StopSensingObject(monitoredObjects[i]);
                    }
                }

                yield return new WaitForFixedUpdate();
            }
            isMonitoring = false;
        }

        private bool CheckLOS(int monitoredObjIndex)
        {
            Vector2 toObj = monitoredObjects[monitoredObjIndex].transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, toObj.normalized, Mathf.Infinity, detectionMask);
            // If the player was detected, mark it as a found entity.

            bool seesTarget = hit.collider != null
                && (hit.collider.gameObject == monitoredObjects[monitoredObjIndex] ||
                hit.collider.gameObject.transform.IsChildOf(monitoredObjects[monitoredObjIndex].transform));

            if (debugLOS)
            {
                if (seesTarget)
                {
                    Debug.DrawLine(transform.position, monitoredObjects[monitoredObjIndex].transform.position, Color.green);
                }
                else
                {
                    Debug.DrawLine(transform.position, monitoredObjects[monitoredObjIndex].transform.position, Color.red);
                    Debug.DrawLine(transform.position, hit.point, Color.green);
                }
                
            }

            return seesTarget;
        }

        private bool CheckHeightDiff(int monitoredObjIndex)
        {
            // Check the difference in max height.
            float heightDiff = monitoredObjects[monitoredObjIndex].transform.position.y - transform.position.y;
            return Mathf.Abs(heightDiff) < maxHeightDiff;
        }

        private void SenseObject(GameObject obj)
        {
            if (!sensedObjects.Contains(obj))
            {
                EntitySenseEvent?.Invoke(obj, senseType, true);
                sensedObjects.Add(obj);
            }
            // Refresh an expiring sense.
            else if (expiringSenses.ContainsKey(obj))
            {
                StopCoroutine(expiringSenses[obj]);
                expiringSenses.Remove(obj);
            }

        }

        private void StopSensingObject(GameObject obj)
        {
            // Starts a coroutine to delay losing the object
            if (!expiringSenses.ContainsKey(obj) && gameObject.activeInHierarchy)
            {
                expiringSenses.Add(obj, StartCoroutine(SenseExpireRoutine(obj, senseExpireTime)));
            }
        }

        private IEnumerator SenseExpireRoutine(GameObject obj, float time)
        {
            yield return new WaitForSeconds(time);
            LoseObject(obj);

            // Check if the enemy is still not sensed.
        }

        private void LoseObject(GameObject obj)
        {
            sensedObjects.Remove(obj);
            EntitySenseEvent?.Invoke(obj, senseType, false);
        }

        // Checks if a collided GO is sensable.
        private bool IsSensable(GameObject obj)
        {
            return obj.CompareTag(PLAYER_TAG);
        }
    }

}