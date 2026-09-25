/*****************************************************************************
// File Name: Manager.cs
// Author: Pierce Nunnelley
// Contributors:
// Creation Date: 9/16/2026
//
// Brief Description:
// The base class which Managers inherit from.
*****************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using System;

namespace TFOOL.ManagersAndServices
{
    public class Manager : MonoBehaviour, IInitializable
    {
        public List<Service> ServiceReferences;
        public List<Service> ServiceInstances;
        public Action<Manager> PostInitiationActions;
        private bool hasFinishedInitializing = false;
        public bool HasFinishedInitializing { get => hasFinishedInitializing;}

        public virtual async Awaitable Initialize()
        {

            foreach (var service in ServiceReferences)
            {
                if (!destroyCancellationToken.IsCancellationRequested)
                {
                    var s = Instantiate(service, transform);

                    ServiceInstances.Add(s);

                    await s.Initialize();
                }

            }
            hasFinishedInitializing = true;
            Debug.Log(this.GetType().Name + " has finished initializing");
            PostInitiationActions?.Invoke(this);
        }

        public virtual void DeInitialize()
        {

        }
    }
}

