/************************************************************************
// File Name : RoomData.cs
// Author : Arcadia Koederitz
// Creation Date : 9/25/2026
// Last Modified : 9/25/2026
//
// Brief Description : Stores data needed to load a room and unload it's corresponding rooms.
*****************************************************************************/
using UnityEngine;

namespace TFOOL.World
{
    [System.Serializable]
    public struct RoomData
    {
        [field: SerializeField] public string Scene { get; private set; }
        [field: SerializeField] public short EntryIndex { get; private set;  }

        public RoomData(string scene, short entryIndex)
        {
            Scene = scene;
            EntryIndex = entryIndex;
        }
    }

}