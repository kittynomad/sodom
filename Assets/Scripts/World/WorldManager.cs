/************************************************************************
// File Name : WorldManager.cs
// Author : Arcadia Koederitz
// Creation Date : 9/25/2026
// Last Modified : 9/25/2026
//
// Brief Description : 
*****************************************************************************/
using AriUtils;
using System;
using System.Threading;
using TFOOL.ManagersAndServices;
using TFOOL.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TFOOL.World
{
    public class WorldManager : Manager
    {
        [SerializeField] private SceneField persistentObjectsScene;
        [SerializeField] private SceneField defaultRoom;
        [SerializeField] private Color loadFadeColor;

        private static WorldManager instance;

        private static CancellationTokenSource loadingCts;

        public override async Awaitable Initialize()
        {
            if (SingletonUtility.SetSingleton(ref instance, this))
            {
                DontDestroyOnLoad(this);
                await base.Initialize();
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
                // Fade to Black.
                await ScreenFadeService.FadeScreenTo(instance.loadFadeColor);

                // Load the persistent data (Non-additively)
                await SceneManager.LoadSceneAsync(instance.persistentObjectsScene);
                // Disable the player.

                // Delegate to the LevelStreamingService to load the starting room.
                await LevelStreamingService.SetRoom(GetStartingRoom());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }            
        }

        /// <summary>
        /// gets a service of a given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T GetService<T>() where T : Service
        {
            return ServiceInstances.Find(x => x is T) as T;
        }


        private static RoomData GetStartingRoom()
        {
            // This should query the SaveDataManager in the future.
            return new RoomData(instance.defaultRoom, -1);
        }
    }
}