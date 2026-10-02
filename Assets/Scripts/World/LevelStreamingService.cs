/************************************************************************
// File Name : LevelStreamingService.cs
// Author : Arcadia Koederitz
// Creation Date : 9/25/2026
// Last Modified : 9/25/2026
//
// Brief Description : Controls loading and unloading levels based on player location.
*****************************************************************************/
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TFOOL.ManagersAndServices;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TFOOL.World
{
    public class LevelStreamingService : Service
    {
        private static RoomData currentRoom;

        private static CancellationTokenSource _mainCts;

        private static CancellationToken mainCt => _mainCts.Token;

        public override async Awaitable Initialize()
        {
            _mainCts = new CancellationTokenSource();
        }

        public override void DeInitialize()
        {
            _mainCts.Cancel();
        }

        /// <summary>
        /// Loads a new room from a given current room.
        /// </summary>
        /// <param name="fromRoom"></param>
        /// <param name="toRoom"></param>
        /// <param name="entryDoor"></param>
        /// <returns></returns>
        public static bool EnterNewRoom(RoomData toRoom, byte entryDoor)
        {
            if (toRoom != currentRoom)
            {
                EnterRoom(toRoom, entryDoor);
                // Move the player to the given entry door.
                return true;
            }
            else
            {
                return false;
            }
        }

        private static async void EnterRoom(RoomData toRoom, byte entryDoor)
        {
            await SetRoom(toRoom);
        }

        /// <summary>
        /// Sets the current room that is loaded.
        /// </summary>
        /// <remarks>Does NOT check the current room.</remarks>
        /// <param name="toRoom"></param>
        public static async Awaitable SetRoom(RoomData toRoom)
        {
            Awaitable unloadScenesOp = null;
            // Unload rooms asssociated with the previous room (Unless it is also listed in new room).
            if (currentRoom != null)
            {
                string[] validUnloadRooms = currentRoom.AllScenes.Except(toRoom.AllScenes).ToArray();
                unloadScenesOp = UnloadScenes(validUnloadRooms);
            }

            // Load rooms associated with the to room.
            string[] validLoadRooms = currentRoom == null ? toRoom.AllScenes : toRoom.AllScenes.Except(currentRoom.AllScenes).ToArray();

            // Await until all scenes are loaded.
            await LoadScenes(validLoadRooms);
            if (unloadScenesOp != null)
            {
                await unloadScenesOp;
            }
            currentRoom = toRoom;
            Debug.Log("Current room is now: " + currentRoom);
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