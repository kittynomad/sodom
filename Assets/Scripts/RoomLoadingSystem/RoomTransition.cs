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

public class RoomTransition : MonoBehaviour
{
    [SerializeField] private RoomData _destinationRoom;
    [SerializeField, Range(0, 10)] private byte _doorIndex;
    [SerializeField, Range(0, 10)] private byte _destinationIndex;
}
