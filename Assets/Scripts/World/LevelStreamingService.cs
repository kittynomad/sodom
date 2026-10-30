/************************************************************************
// File Name : LevelStreamingService.cs
// Author : Arcadia Koederitz
// Creation Date : 9/25/2026
// Last Modified : 9/25/2026
//
// Brief Description : Controls loading and unloading levels based on player location.
*****************************************************************************/
using System.Linq;
using System.Threading;
using TFOOL.ManagersAndServices;
using TFOOL.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TFOOL.World
{
    public class LevelStreamingService : Service
    {
        [SerializeField] private Color fadeColor;

        private static string currentRoom;

        private static CancellationTokenSource _mainCts;

        private static CancellationToken mainCt => _mainCts.Token;
        private static LevelStreamingService instance;

        public override async Awaitable Initialize()
        {
            _mainCts = new CancellationTokenSource();
            SingletonUtility.SetSingleton(ref instance, this);

        }

        public override void DeInitialize()
        {
            _mainCts.Cancel();
        }
        
        /// <summary>
        /// Checks if a 
        /// </summary>
        /// <param name="toCompare"></param>
        /// <returns></returns>
        public static bool IsCurrent(string toCompare)
        {
            return toCompare == currentRoom;
        }

        /// <summary>
        /// Loads a new room from a given current room.
        /// </summary>
        /// <param name="fromRoom"></param>
        /// <param name="toRoom"></param>
        /// <param name="entryDoor"></param>
        /// <returns></returns>
        public static void EnterNewRoom(string toRoom, byte entryDoor)
        {
            if (toRoom != currentRoom)
            {
                EnterRoom(toRoom, entryDoor);
            }
        }

        private static async void EnterRoom(string toRoom, byte entryDoor)
        {
            await SetRoom(toRoom);
        }

        /// <summary>
        /// Sets the current room that is loaded.
        /// </summary>
        /// <remarks>Does NOT check the current room.</remarks>
        /// <param name="toRoom"></param>
        public static async Awaitable SetRoom(string toRoom)
        {
            if (instance != null)
            {
                // Fade the screen to black.
                await ScreenFadeService.FadeScreenTo(instance.fadeColor);
            }

            //Awaitable unloadScenesOp = null;
            //// Unload rooms asssociated with the previous room (Unless it is also listed in new room).
            //if (currentRoom != null)
            //{
            //    string[] validUnloadRooms = currentRoom.AllScenes.Except(toRoom.AllScenes).ToArray();
            //    unloadScenesOp = UnloadScenes(validUnloadRooms);
            //}

            //// Load rooms associated with the to room.
            //string[] validLoadRooms = currentRoom == null ? toRoom.AllScenes : toRoom.AllScenes.Except(currentRoom.AllScenes).ToArray();

            //// Await until all scenes are loaded.
            //await LoadScenes(validLoadRooms);
            //if (unloadScenesOp != null)
            //{
            //    await unloadScenesOp;
            //}
            //currentRoom = toRoom;

            // Start loading the new room.
            AsyncOperation loadScenesOp = SceneManager.LoadSceneAsync(toRoom, LoadSceneMode.Additive);

            // Unload the current room.
            if (currentRoom != null && currentRoom != "")
            {
                await SceneManager.UnloadSceneAsync(currentRoom);
            }

            // Wait until the scene is loaded.
            await loadScenesOp;

            currentRoom = toRoom;

            Debug.Log("Current room is now: " + currentRoom);

            if (instance != null)
            {
                // Fade back in.
                await ScreenFadeService.FadeScreenOut();
            }
        }

        private static async Awaitable UnloadScenes(string[] scenes)
        {
            foreach (string scene in scenes)
            {
                if (mainCt.IsCancellationRequested) { return; }
                for(int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Scene sceneStruct = SceneManager.GetSceneAt(i);
                    if (scene == sceneStruct.name)
                    {
                        await SceneManager.UnloadSceneAsync(sceneStruct);
                    }
                }
            }
        }

        private static async Awaitable LoadScenes(string[] scenes)
        {
            foreach (string scene in scenes)
            {
                if (mainCt.IsCancellationRequested) { return; }
                bool isLoaded = false;
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Scene sceneStruct = SceneManager.GetSceneAt(i);
                    if (scene == sceneStruct.name)
                    {
                        isLoaded = true;
                        return;
                    }
                }

                if (!isLoaded)
                {
                    await SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                }
            }
        }
    }

}