/************************************************************************
// File Name :RoomTransition.cs
// Author : Arcadia Koederitz
// Creation Date : 9/25/2026
// Last Modified : 9/25/2026
//
// Brief Description : Triggers transitions between rooms and prompts the LevelStreamingService to load the 
// appropriate levels.
*****************************************************************************/
using AriUtils;
using NaughtyAttributes;
using UnityEngine;

namespace TFOOL.World
{
    public class TransitionZone : MonoBehaviour
    {
        private const short MAX_DOOR_INDICIES = 10;

        [SerializeField, Tooltip("Information about the room that this transition zone connects to.")] 
        private SceneField _destinationRoom;
        [SerializeField, Tooltip("What direction of the room this transition zone lies at.")]
        private TransitionDirection _direction;
        [SerializeField, Range(0, MAX_DOOR_INDICIES - 1), Tooltip("The index of this transition zone.")] 
        private short _doorIndex;

        private bool currentLoadingZone;
        private static bool canTransition = true;

        public short DoorIndex => _doorIndex;

        public static Vector3?[] DoorPositions { get; private set; } = new Vector3?[MAX_DOOR_INDICIES];

        #region Nested
        private enum TransitionDirection
        {
            Right = 0,
            Top = 1,
            Left = 2,
            Bottom = 3
        }
        #endregion

        // Make this door's position statically available to the LevelStreamingService.
        private void Awake()
        {
            DoorPositions[_doorIndex] = transform.position;
        }
        private void OnDestroy()
        {
            DoorPositions[_doorIndex] = null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (canTransition && !LevelStreamingService.IsCurrent(_destinationRoom) && 
                collision.gameObject.TryGetComponent(out PlayerController pc))
            {
                // Transition to the destination room if the player is currently in this room.
                currentLoadingZone = true;
                canTransition = false;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (currentLoadingZone && collision.gameObject.TryGetComponent(out PlayerController pc))
            {
                // Only allow a room transition to happen again once the player has left the loading zone.
                currentLoadingZone = false;
                canTransition = true;

                // Check the direction the player exited the zone from.  if they exited in the direction of the next room, load the next room.
                if (CompareExitPosition(_direction, collision.GetComponent<Rigidbody2D>().linearVelocity))
                {
                    LevelStreamingService.EnterNewRoom(new RoomData(_destinationRoom, _doorIndex));
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