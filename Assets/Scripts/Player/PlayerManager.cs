/************************************************************************
// File Name : PlayerManager.cs
// Author : Arcadia Koederitz
// Creation Date : 10/30/2026
// Last Modified : 10/30/2026
//
// Brief Description : Static functions for interacting with the player.
*****************************************************************************/
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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
            playerInstance.GetComponent<PlayerInput>().enabled = isActive; // Temp Disable until we get a proper solution.
        }

        public static void ClearInput()
        {
            if (playerInstance == null) { return; }
            playerInstance.Controller.ResetInput();
        }

        public static void MovePlayerFeetTo(Vector2 position)
        {
            if (playerInstance == null) { return; }
            Collider2D[] playerColliders = new Collider2D[playerInstance.Rigidbody.attachedColliderCount];
            playerInstance.Rigidbody.GetAttachedColliders(playerColliders);
            float beraHeight = playerInstance.Rigidbody.position.y - playerColliders.OrderBy(x => x.bounds.min.y).FirstOrDefault().bounds.min.y;
            playerInstance.Rigidbody.position = position + Vector2.up * beraHeight;
            Debug.Log("Moved player to: " + position);
        }
    }
}