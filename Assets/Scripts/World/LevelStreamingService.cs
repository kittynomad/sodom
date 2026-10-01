/************************************************************************
// File Name : LevelStreamingService.cs
// Author : Arcadia Koederitz
// Creation Date : 9/25/2026
// Last Modified : 9/25/2026
//
// Brief Description : Controls loading and unloading levels based on player location.
*****************************************************************************/
using TFOOL.ManagersAndServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TFOOL.World
{
    public class LevelStreamingService : Service
    {
        private static RoomData currentRoom;
        private static bool canTransition;

        public override async Awaitable Initialize()
        {
            canTransition = true;
        }

        public static bool EnterNewRoom(RoomData fromRoom, RoomData toRoom, byte entryDoor)
        {
            if (fromRoom != currentRoom)
            {
                SetRoom(toRoom, entryDoor);
                canTransition = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void SetRoom(RoomData toRoom, byte entryDoor)
        {
            
        }

        public static void ResetTransitionability()
        {
            canTransition = true;
        }
    }

}