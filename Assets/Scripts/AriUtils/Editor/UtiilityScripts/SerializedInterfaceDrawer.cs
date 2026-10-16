/*****************************************************************************
// File Name : SerializeInterfaceDrawer.cs
// Author : Arcadia Koederitz
// Creation Date : 10/04/2026
// Last Modified : 10/04/2026
//
// Brief Description : Property Drawer that handles displaying options for serialzied interfaces.
*****************************************************************************/
using System;
using UnityEditor;
using UnityEngine;

namespace AriUtils.Editor
{
    [CustomPropertyDrawer(typeof(SerializedInterface<>))]
    public class SerializedInterfaceDrawer : PropertyDrawer
    {
        private EditorClassDropdown classDropdown;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //Debug.Log("GUI");
            EditorGUI.BeginProperty(position, label, property);
            Rect fullRect = position;
            SerializedProperty _referenceType = property.FindPropertyRelative(nameof(_referenceType));
            SerializedProperty _objectReferenceValue = property.FindPropertyRelative(nameof(_objectReferenceValue));
            SerializedProperty _managedReferenceValue = property.FindPropertyRelative(nameof(_managedReferenceValue));

            SerializedInterface serializedInterface = property.boxedValue as SerializedInterface;

            position = EditorGUI.PrefixLabel(position, label);

            // Create the toggle for switching between a class and object reference.
            Rect typeToggleRect = position;
            typeToggleRect.height = EditorGUIUtility.singleLineHeight;
            typeToggleRect.xMax = typeToggleRect.xMin + typeToggleRect.height;
            _referenceType.boolValue = EditorGUI.Toggle(typeToggleRect, _referenceType.boolValue);

            position.xMin += typeToggleRect.width + EditorGUIUtility.standardVerticalSpacing;

            if (_referenceType.boolValue)
            {
                // Handle Managed Property type.
                DrawClassDropdown(fullRect, position, _managedReferenceValue, serializedInterface.InterfaceType);
                _objectReferenceValue.objectReferenceValue = null;
            }
            else
            {
                // Handle Object property type.
                DrawUnityObjectField(position, _objectReferenceValue, serializedInterface.InterfaceType);
                _managedReferenceValue.managedReferenceValue = null;
            }
            EditorGUI.EndProperty();
        }

        #region Unity Object Field
        private void DrawUnityObjectField(Rect position, SerializedProperty objectReferenceProperty, Type interfaceType)
        {
            // Draws a UnityObject drag-and-drop reference field that accepts objects that implement the interface.
           objectReferenceProperty.objectReferenceValue = EditorGUI.ObjectField(position, 
               objectReferenceProperty.objectReferenceValue, interfaceType, true);
        }
        #endregion

        #region ClassField
        private void DrawClassDropdown(Rect fullRect, Rect position, SerializedProperty managedReferenceProperty, Type interfaceType)
        {
            
            if (classDropdown == null)
            {
                // Only include non-unity engine object classes that implement the interface.
                bool ClassFilter(Type type)
                {
                    return interfaceType.IsAssignableFrom(type) && !EditorUtils.IsClassOrSubclass(type, typeof(UnityEngine.Object));
                }

                classDropdown = new EditorClassDropdown(ClassFilter, managedReferenceProperty);
            }

            classDropdown.DrawDropdown(fullRect, position, managedReferenceProperty, "");
        }
        #endregion

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty _referenceType = property.FindPropertyRelative(nameof(_referenceType));
            SerializedProperty _managedReferenceValue = property.FindPropertyRelative(nameof(_managedReferenceValue));
            if (_referenceType.boolValue)
            {
                // If managed reference, then the height should depend on if the property is expanded.
                return EditorUtils.GetDynamicPropertyHeight(_managedReferenceValue);
            }
            else
            {
                return base.GetPropertyHeight(property, label);
            }
        }
    }
}