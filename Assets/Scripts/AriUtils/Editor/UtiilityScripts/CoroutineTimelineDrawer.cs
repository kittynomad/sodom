/*****************************************************************************
// File Name : CoroutineTimelineDrawer.cs
// Author : Arcadia Koederitz
// Creation Date : 7/28/2026
// Last Modified : 7/28/2026
//
// Brief Description : Custom property drawer for coroutine timelines.
*****************************************************************************/
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AriUtils.Editor
{
    [CustomPropertyDrawer(typeof(CoroutineTimeline))]
    public class CoroutineTimelineDrawer : PropertyDrawer
    {
        private readonly string[] READ_ONLY_FIELDS = { "_parentBehavior", "time" };

        private bool isInitialized;
        private SerializedProperty _parentBehavior;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!isInitialized)
            {
                Init(property);
            }

            // Automatically assign the ParentBehaviour Property.
            SerializedObject sObj = property.serializedObject;
            if (sObj != null && sObj.targetObject is MonoBehaviour)
            {
                _parentBehavior.objectReferenceValue = sObj.targetObject;
                sObj.ApplyModifiedProperties();
            }


            // Draw the properties.
            Rect dropdownRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(dropdownRect, property.isExpanded, label);
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                // Calculate the starting y position for the child fields.
                float yOffset = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                bool skipParent = false;

                void DrawProperty(SerializedProperty property)
                {
                    float childHeight = EditorGUI.GetPropertyHeight(property, true);
                    Rect childRect = new Rect(position.x, yOffset, position.width, childHeight);
                    if (skipParent && SerializedProperty.EqualContents(property, _parentBehavior))
                    {
                        return;
                    }

                    bool readOnly = READ_ONLY_FIELDS.Contains(property.name);
                    if (readOnly)
                    {
                        EditorGUI.BeginDisabledGroup(true);
                    }
                    // Draw the child property field.
                    EditorGUI.PropertyField(childRect, property, true);
                    if (readOnly)
                    {
                        EditorGUI.EndDisabledGroup();
                    }

                    // Move to the next property.
                    yOffset += childHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                // Draw the parent proeprty as a ReadOnly field at the top.
                DrawProperty(_parentBehavior);
                skipParent = true;

                LoopForProperties(DrawProperty, property);

                EditorGUI.indentLevel--;
            }
        }

        /// <summary>
        /// Initialize the sub-property references.
        /// </summary>
        /// <param name="property"></param>
        private void Init(SerializedProperty property)
        {
            _parentBehavior = property.FindPropertyRelative(nameof(_parentBehavior));
            isInitialized = true;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            // If we have a class selected, then loop through all of it's properties and add their heights to the total.
            if (property.isExpanded)
            {
                void IncreaseHeight(SerializedProperty ppt)
                {
                    height += EditorGUI.GetPropertyHeight(ppt, true) + EditorGUIUtility.standardVerticalSpacing;
                }

                LoopForProperties(IncreaseHeight, property);
            }
            return height;
        }

        /// <summary>
        /// Loops through the child properties in a given SerializedProperty and calls a propertyFunc on 
        /// copies of the property.
        /// </summary>
        /// <param name="propertyFunc">The function to call on copies of the base property's children.</param>
        /// <param name="baseProperty">The base property to run the function on the children of.</param>
        private static void LoopForProperties(Action<SerializedProperty> propertyFunc, SerializedProperty baseProperty)
        {
            // Not sure why we need to copy the property.
            SerializedProperty copy = baseProperty.Copy();
            SerializedProperty end = copy.GetEndProperty();

            // Tracks if the loop should enter child properties. Only need to enter children the first loop, as
            // further children will be drawn automatically.
            bool enterChildren = true;
            // Loop over all the properties in the copy until we hit the end property.
            while (copy.NextVisible(enterChildren) && !SerializedProperty.EqualContents(copy, end))
            {
                //Skip the default m_Script property
                if (copy.name.Equals("m_Script", StringComparison.Ordinal))
                {
                    enterChildren = false;
                    continue;
                }

                propertyFunc(copy);

                enterChildren = false;
            }
        }
    }

}