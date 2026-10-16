/************************************************************************
// File Name : RoomData.cs
// Author : Arcadia Koederitz
// Creation Date : 9/25/2026
// Last Modified : 9/25/2026
//
// Brief Description : Stores data needed to load a room and unload it's corresponding rooms.
*****************************************************************************/
using AriUtils;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TFOOL.World
{
    [CreateAssetMenu(fileName = "RoomData", menuName = "Scriptable Objects/Room Data")]
    public class RoomData : ScriptableObject
    {
        [SerializeField] private SceneField _thisScene;
        [SerializeField] private SceneField[] _adjacentScenes;

        private string[] _allScenes;

        public string ThisScene => _thisScene;
        public string[] AdjacentScenes => _adjacentScenes.Select(x => x.SceneName).ToArray();
        public string[] AllScenes
        {
            get
            {
                if (_allScenes == null || _allScenes.Length == 0)
                {
                    List<string> scenes = new() { _thisScene };
                    scenes.AddRange(AdjacentScenes);
                    _allScenes = scenes.ToArray();
                }
                return _allScenes;
            }
        }
    }

}