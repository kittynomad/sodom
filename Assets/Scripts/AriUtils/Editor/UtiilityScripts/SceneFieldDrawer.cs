/*****************************************************************************
// File Name : SceneFieldDrawer.cs
// Author : 
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using UnityEditor;
using UnityEngine;

namespace AriUtils.Editor
{
    [CustomPropertyDrawer(typeof(SceneField))]
    public class SceneFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Find Properties of the SceneField.
            SerializedProperty _sceneAsset = property.FindPropertyRelative(nameof(_sceneAsset));
            SerializedProperty _sceneName = property.FindPropertyRelative(nameof(_sceneName));

            // Add the prefix label.
            position = EditorGUI.PrefixLabel(position, label);

            if (_sceneAsset != null)
            {
                // Draw an ObjectField that takes in a SceneAsset.
                _sceneAsset.objectReferenceValue = EditorGUI.ObjectField(position, _sceneAsset.objectReferenceValue, typeof(SceneAsset), false);

                if (_sceneAsset.objectReferenceValue != null)
                {
                    // Update the SceneName field.  This is the only field that will persist into a build.
                    _sceneName.stringValue = (_sceneAsset.objectReferenceValue as SceneAsset).name;
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
