/************************************************************************
// File Name : RoomData.cs
// Author : Arcadia Koederitz
// Creation Date : 9/25/2026
// Last Modified : 9/25/2026
//
// Brief Description : Stores data needed to load a room and unload it's corresponding rooms.
*****************************************************************************/
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace TFOOL.World
{
    [CreateAssetMenu(fileName = "RoomData", menuName = "Scriptable Objects/Room Data")]
    public class RoomData : ScriptableObject
    {
        [SerializeField, Scene] private string _thisScene;
        [SerializeField, Scene] private string[] _adjacentScenes;

        private string[] _allScenes;

        public string ThisScene => _thisScene;
        public string[] AdjacentScenes => _adjacentScenes;
        public string[] AllScenes
        {
            get
            {
                if (_allScenes == null || _allScenes.Length == 0)
                {
                    List<string> scenes = new() { _thisScene };
                    scenes.AddRange(_adjacentScenes);
                    _allScenes = scenes.ToArray();
                }
                return _allScenes;
            }
        }
    }

}