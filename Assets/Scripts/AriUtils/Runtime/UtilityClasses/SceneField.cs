/*****************************************************************************
// File Name : SceneField.cs
// Author : Arcadia Koederitz
// Creation Date : 10/04/2026
// Last Modified : 10/04/2026
//
// Brief Description : Acts as a serializeable scene field in the inspector that resolves to a string in a build.
Based on a post on this unity forum: https://discussions.unity.com/t/inspector-field-for-scene-asset/40763/5
*****************************************************************************/
using UnityEngine;

namespace AriUtils
{
    [System.Serializable]
    public struct SceneField
    {
        [SerializeField] private string _sceneName;

        public static implicit operator string(SceneField sceneField)
        {
            return sceneField._sceneName;
        }

#if UNITY_EDITOR
        [SerializeField] private Object _sceneAsset;
#endif
    }
}
