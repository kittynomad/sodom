/************************************************************************
// File Name : EnemyController.GUID.cs
// Author : Arcadia Koederitz
// Creation Date : 10/20/2026
// Last Modified : 10/20/2026
//
// Brief Description : struct for containing an enemy's GUID.s
*****************************************************************************/
using System;
using UnityEditor;
using UnityEngine;

namespace TFOOL.Enemies
{
    public partial class EnemyController : MonoBehaviour
#if UNITY_EDITOR
        , ISerializationCallbackReceiver
#endif
    {
        [SerializeField] private string _guidString;
        private Guid _guid;

        public string EnemyGUID => _guidString;
        public bool IsGUIDAssigned => _guidString != null && _guidString != "" && _guidString != Guid.Empty.ToString();

#if UNITY_EDITOR
        /// <summary>
        /// Creates a unique GUID for this enemy to be used for save data.
        /// </summary>
        public void GenerateGUID()
        {
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                _guid = Guid.NewGuid();
                _guidString = _guid.ToString();
            }
            else
            {
                Debug.LogError("Cannot generate a GUID for an enemy at runtime.");
            }
        }

        public void OnAfterDeserialize()
        {
            
        }

        public void OnBeforeSerialize()
        {
            // Prefab should not have a serialized guid.
            if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
            {
                _guidString = "";
            }
            // If the _guidString was changeed via an external source (and therefore _guid was not changed), revert back to _guid.
            else if (_guid.ToString() != _guidString)
            {
                _guidString = _guid.ToString();
            }
        }
#endif
    }
}