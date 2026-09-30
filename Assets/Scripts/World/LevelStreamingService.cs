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
        public override async Awaitable Initialize()
        {
            
        }

        public static void SetCurrentRoom(RoomData room)
        {

        }
    }

}