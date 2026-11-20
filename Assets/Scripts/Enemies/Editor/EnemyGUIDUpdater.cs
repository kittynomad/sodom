/************************************************************************
// File Name : EnemyGUIDUpdater.cs
// Author : Arcadia Koederitz
// Creation Date : 10/13/2026
// Last Modified : 10/13/2026
//
// Brief Description : Automatically creates GUIDs for enemies placed in the scene
*****************************************************************************/
using UnityEditor;
using UnityEngine;

namespace TFOOL.Enemies.Editor
{
    [InitializeOnLoad]
    public static class EnemyGUIDUpdater
    {
        static EnemyGUIDUpdater()
        {
            EditorApplication.hierarchyChanged += UpdateEnemyGUID;
        }

        private static void UpdateEnemyGUID()
        {
            if (Selection.activeObject is GameObject go 
                && go.TryGetComponent(out EnemyController enemy) 
                && !PrefabUtility.IsPartOfPrefabAsset(go) // Only assign a GUID if the GameObject is a part of a prefab instance, not an asset.
                && !enemy.IsGUIDAssigned)
            {
                enemy.GenerateGUID();
                EditorUtility.SetDirty(enemy);
            }
        }
    }
}