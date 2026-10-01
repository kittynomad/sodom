/************************************************************************
// File Name : WorldManager.cs
// Author : Arcadia Koederitz
// Creation Date : 9/25/2026
// Last Modified : 9/25/2026
//
// Brief Description : 
*****************************************************************************/
using NaughtyAttributes;
using System;
using System.Threading;
using TFOOL.ManagersAndServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TFOOL.World
{
    public class WorldManager : Manager
    {
        [SerializeField, Scene] private string persistentObjectsScene;
        [SerializeField] private RoomData defaultRoom;

        private static WorldManager instance;

        private static CancellationTokenSource loadingCts;

        public override async Awaitable Initialize()
        {
            await base.Initialize();
            
            if (instance != null && instance != this)
            {
                instance = this;
            }
            else
            {
                Debug.LogError("Duplicate WorldManager Found");
                Destroy(gameObject);
                return;
            }
        }

        public override void DeInitialize()
        {
            loadingCts.Cancel();
        }

        /// <summary>
        /// Loads the main game world from the main menu.
        /// </summary>
        public static async void LoadWorld()
        {
            if (instance == null)
            {
                Debug.LogError("Cannot Load World until the WorldManager has initialized.");
                return;
            }
            try
            {
                // Queue any transitions.

                // Load the persistent data (Non-additively)
                AsyncOperation persistentOp = SceneManager.LoadSceneAsync(instance.persistentObjectsScene);
                // Wait until both scenes are loaded.
                await persistentOp;

                // Delegate to the LevelStreamingService.


            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }            
        }


        private static RoomData GetStartingRoom()
        {
            // This should query the SaveDataManager in the future.
            return instance.defaultRoom;
        }
    }
}