/************************************************************************
// File Name :RoomTransition.cs
// Author : Arcadia Koederitz
// Creation Date : 9/25/2026
// Last Modified : 9/25/2026
//
// Brief Description : Triggers transitions between rooms and prompts the LevelStreamingService to load the 
// appropriate levels.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;

namespace TFOOL.World
{
    public class TransitionZone : MonoBehaviour
    {
        [SerializeField, Tooltip("The room this transition zone is in.")]
        private RoomData _currentRoom;
        [SerializeField, Tooltip("Information about the room that this transition zone connects to.")] 
        private RoomData _destinationRoom;
        [SerializeField, Range(0, 10), Tooltip("The index of this transition zone.")] 
        private byte _doorIndex;
        [SerializeField, Range(0, 10), Tooltip("The index of the transition zone that the player should spawn at in the next scene.")] 
        private byte _destinationIndex;

        private bool currentLoadingZone;

        public byte DoorIndex => _doorIndex;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // Transition to the destination room if the player is currently in this room.
                currentLoadingZone = LevelStreamingService.EnterNewRoom(_currentRoom, _destinationRoom, _destinationIndex);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && currentLoadingZone)
            {
                // Only allow a room transition to happen again once the player has left the loading zone.
                LevelStreamingService.ResetTransitionability();
            }
        }
    }

}