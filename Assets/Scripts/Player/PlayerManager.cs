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
    public class PlayerManager : MonoBehaviour
    {
        internal static PlayerBehaviors playerInstance;

        public void TogglePlayer(bool isActive)
        {
            playerInstance.gameObject.SetActive(isActive);
        }
    }
}