/*****************************************************************************
// File Name : ApplicationStarter.cs
// Author : Pierce Nunnelley
// Creation Date : February 7, 2026
//
// Brief Description : This initializes game's managers, starting game logic.
*****************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using TFOOL.ManagersAndServices;

public class ApplicationStarter : MonoBehaviour
{
    public List<Manager> ManagerReferences;
    public List<Manager> ManagerInstances;

    async void Start()
    {
        try
        {
            await InitManagers();
        }
        catch
        {
            Debug.Log("Manager initializaton CANCELLED!!!");
        }
        
        print("start has finished");
    }

    async Awaitable InitManagers()
    {
            for (int i = 0; i < ManagerReferences.Count; i++)
            {
                if (destroyCancellationToken.IsCancellationRequested) return;
                var m = Instantiate(ManagerReferences[i]);

                ManagerInstances.Add(m);

                await m.Initialize();
                
            }

            await Task.CompletedTask;
            return;
        
    }
}
