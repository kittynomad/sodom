/************************************************************************
// File Name : PlayerManager.cs
// Author : Arcadia Koederitz
// Creation Date : 10/30/2026
// Last Modified : 10/30/2026
//
// Brief Description : Static functions for interacting with the player.
*****************************************************************************/
using UnityEngine;

namespace TFOOL.ManagersAndServices
{
    public class PlayerManager : Manager
    {
        internal static PlayerBehaviors playerInstance;

        public static bool IsSpawned => playerInstance != null;
        public static Vector2 PlayerPosition => playerInstance == null ? Vector2.zero : playerInstance.Rigidbody.position;

        public static void TogglePlayerActions(bool isActive)
        {
            if (playerInstance == null) { return; }
            
        }

        public static void MovePlayerTo(Vector2 position)
        {
            if (playerInstance == null) { return; }
            playerInstance.Rigidbody.MovePosition(position);
        }
    }
}