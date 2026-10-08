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
        [SerializeField, Tooltip("Information about the room that this transition zone connects to.")] 
        private RoomData _destinationRoom;
        [SerializeField, Tooltip("What direction of the room this transition zone lies at.")]
        private TransitionDirection _direction;
        [SerializeField, Range(0, 10), Tooltip("The index of this transition zone.")] 
        private byte _doorIndex;
        [SerializeField, Range(0, 10), Tooltip("The index of the transition zone that the player should spawn at in the next scene.")] 
        private byte _destinationIndex;

        private bool currentLoadingZone;
        private static bool canTransition = true;

        public byte DoorIndex => _doorIndex;

        #region Nested
        private enum TransitionDirection
        {
            Right = 0,
            Top = 1,
            Left = 2,
            Bottom = 3
        }
        #endregion

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && canTransition && !LevelStreamingService.IsCurrent(_destinationRoom))
            {
                // Transition to the destination room if the player is currently in this room.
                currentLoadingZone = true;
                canTransition = false;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && currentLoadingZone)
            {
                // Only allow a room transition to happen again once the player has left the loading zone.
                currentLoadingZone = false;
                canTransition = true;

                // Check the direction the player exited the zone from.  if they exited in the direction of the next room, load the next room.
                if (CompareExitPosition(_direction, collision.GetComponent<Rigidbody2D>().linearVelocity))
                {
                    LevelStreamingService.EnterNewRoom(_destinationRoom, _destinationIndex);
                }

            }
        }

        private static bool CompareExitPosition(TransitionDirection direction, Vector2 playerVelocity)
        {
            float exitAngle = MathUtil.VectorToDegAngle(playerVelocity);
            int exitDirection = MathUtil.Mod(Mathf.RoundToInt(exitAngle / 90), 4);
            Debug.Log(exitDirection);
            return direction == (TransitionDirection)exitDirection;
            
        }
    }

}