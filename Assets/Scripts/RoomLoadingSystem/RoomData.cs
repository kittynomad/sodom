/************************************************************************
// File Name : RoomData.cs
// Author : Arcadia Koederitz
// Creation Date : 9/25/2026
// Last Modified : 9/25/2026
//
// Brief Description : Stores data needed to load a room and unload it's corresponding rooms.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "Scriptable Objects/Room Data")]
public class RoomData : ScriptableObject
{
    [SerializeField, Scene] private int thisScene;
    [SerializeField, Scene] private int[] adjacentScenes;
}
