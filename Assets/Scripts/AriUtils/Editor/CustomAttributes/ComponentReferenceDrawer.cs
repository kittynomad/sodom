/*****************************************************************************
// File Name : ComponentReferenceDrawer.cs
// Author : Arcadia Koederitz
// Creation Date : 10/04/2026
// Last Modified : 10/04/2026
//
// Brief Description : Custom Property drawer that automatically searches the base object (if it's a component) 
for a component of the matching type.
*****************************************************************************/
using System;
using UnityEditor;
using UnityEngine;

namespace AriUtils.Editor
{
    [CustomPropertyDrawer(typeof(ComponentReferenceAttribute))]
    public class ComponentReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Get the component from the parent SerializedObject if it's a monobehavior.
            if (property.serializedObject.targetObject is Component parentComponent && 
                EditorUtils.IsClassOrSubclass(fieldInfo.FieldType, typeof(Component)))
            {
                if (property.objectReferenceValue == null)
                {
                    property.objectReferenceValue = parentComponent.GetComponent(fieldInfo.FieldType);
                }

                if (property.objectReferenceValue == null)
                {
                    // Display a debug message.
                    EditorGUI.HelpBox(position, $"No {fieldInfo.FieldType} attached to this GameObject.", MessageType.Error);
                }
                else if (!(Attribute.GetCustomAttribute(parentComponent.GetType(), typeof(RequireComponent)) is RequireComponent))
                {
                    
                    EditorGUI.HelpBox(position, $"It is recommended to use the RequireComponent(typeof" +
                        $"({fieldInfo.FieldType.Name})) attribute on {parentComponent.GetType().Name}", MessageType.Info);
                }
            }
            else
            {
                // Show an error message.
                EditorGUI.HelpBox(position, "ComponentReference attribute is not valid on non-component types or on" +
                    " scripts that are not derived from MonoBehavior.", MessageType.Error);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!(property.serializedObject.targetObject is Component parentComponent && EditorUtils.IsClassOrSubclass(fieldInfo.FieldType, typeof(Component))) || 
                property.objectReferenceValue == null || 
                !(Attribute.GetCustomAttribute(parentComponent.GetType(), typeof(RequireComponent)) is RequireComponent))
            {
                // Show an error message.
                return base.GetPropertyHeight(property, label) * 2;
            }
            else
            {
                return 0;
            }
        }
    }
}
