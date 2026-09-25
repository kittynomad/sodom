/*****************************************************************************
// File Name: Service.cs
// Author: Pierce Nunnelley
// Contributors:
// Creation Date: 9/16/2026
//
// Brief Description:
// The base class which Services inherit from.
*****************************************************************************/
using UnityEngine;


namespace TFOOL.ManagersAndServices
{
    public class Service : MonoBehaviour, IInitializable
    {
        private bool hasFinishedInitializing = false;
        public bool HasFinishedInitializing { get => hasFinishedInitializing; }

        public virtual async Awaitable Initialize()
        {
            await Awaitable.WaitForSecondsAsync(0.1f);
            Debug.Log(this.GetType().Name + " has finished initializing");
            hasFinishedInitializing = true;
            return;
        }

        public virtual void DeInitialize()
        {

        }
    }
}


