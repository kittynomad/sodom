/*****************************************************************************
// File Name : ClassDropdown.cs
// Author : Arcadia Koederitz
// Creation Date : 10/04/2026
// Last Modified : 10/04/2026
//
// Brief Description : Draws a custom dropdown for an abstract base class and any relevant serialized properties.
// Based on an article at: https://medium.com/@modyari/unity-workflow-customization-robust-custom-dropdowns-to-unlock-manageable-architecture-f16c183b993a 
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assemblies;

namespace AriUtils.Editor
{
    public class EditorClassDropdown
    {
        private int selectionIndex;
        private readonly string[] selectionOptions;
        private readonly List<Type> selectionTypes;

        public EditorClassDropdown(Predicate<Type> classMatcher, SerializedProperty property, Type assemblyRestrictionType = null)
        {
            // Create a temp list to add selection options to.
            List<string> tempSelections = new List<string>() { "Null" };

            selectionTypes = new List<Type>();

            if (assemblyRestrictionType != null)
            {
                // If provided a restriction type, only search the assembly that the base type in it.
                AddTypes(classMatcher, ref tempSelections, Assembly.GetAssembly(assemblyRestrictionType));
            }
            else
            {
                // Loop through all types in all assemblies (More generic impmenentation).
                foreach (Assembly asmb in CurrentAssemblies.GetLoadedAssemblies())
                {
                    AddTypes(classMatcher, ref tempSelections, asmb);
                }
            }

            // Convert the temp list into a permanent array.
            selectionOptions = tempSelections.ToArray();

            UpdateSelectionIndex(property);
        }

        /// <summary>
        /// Draws the actual class dropdown from a given property and rect.  Called from OnGUI.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public void DrawDropdown(Rect fullRect, Rect position, SerializedProperty property, string label)
        {
            UpdateSelectionIndex(property);

            // Draw the dropdown
            Rect dropdownRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.BeginChangeCheck();
            selectionIndex = EditorGUI.Popup(dropdownRect, label, selectionIndex, selectionOptions);
            // If the user made a change to the popup selection:
            if (EditorGUI.EndChangeCheck())
            {
                // Creates an instance of the type the user selected and assign it to the property value.
                SetPropertyValue(property, selectionIndex);
            }

            // Draw the subclass serialized fields.

            if (property.managedReferenceValue != null)
            {
                fullRect.height = EditorGUIUtility.singleLineHeight;
                if (property.hasVisibleChildren)
                {
                    property.isExpanded = EditorGUI.Foldout(fullRect, property.isExpanded, "");
                }

                if (property.isExpanded)
                {
                    EditorGUI.indentLevel++;

                    // Calculate the starting y position for the child fields.
                    float yOffset = fullRect.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                    void DrawProperty(SerializedProperty property)
                    {
                        float childHeight = EditorGUI.GetPropertyHeight(property, true);
                        Rect childRect = new Rect(fullRect.x, yOffset, fullRect.width, childHeight);

                        // Draw the child property field.
                        EditorGUI.PropertyField(childRect, property, true);

                        // Move to the next property.
                        yOffset += childHeight + EditorGUIUtility.standardVerticalSpacing;
                    }

                    EditorUtils.LoopForProperties(DrawProperty, property);

                    EditorGUI.indentLevel--;
                }
            }
        }

        /// <summary>
        /// Searches through an assembly and adds all types that are not filtered out by the class filter.
        /// </summary>
        /// <param name="classMatcher"></param>
        /// <param name="tempSelections"></param>
        /// <param name="assembly"></param>
        private void AddTypes(Predicate<Type> classMatcher, ref List<string> tempSelections, Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                //Debug.Log($"Type: {type.Name}.  IsInterface: {type.IsInterface}.  IsAbstract: {type.IsAbstract}.  Matches Class: {classMatcher(type)}");
                // By default, filter out abstract classes and interfaces (as we cannot make instances of them)
                if (type.IsInterface || type.IsAbstract || !classMatcher(type))
                {
                    
                    continue;
                }
                
                // Store the type and the type's name for use in the dropdown.
                selectionTypes.Add(type);
                string displayName = type.Name;

                // Attempt to put the type into a given group in the dropdown.
                if (Attribute.GetCustomAttribute(type, typeof(DropdownGroupAttribute)) is DropdownGroupAttribute dga)
                {
                    displayName = dga.GroupName + "/" + displayName;
                }
                tempSelections.Add(displayName);
            }

        }


        /// <summary>
        /// Updates the index of the selected class to match the one stored in the property.
        /// </summary>
        private void UpdateSelectionIndex(SerializedProperty property)
        {
            object currentSubclass = property.managedReferenceValue;

            // If no subclass is selected, then pick the first type by default.
            if (currentSubclass == null)
            {
                selectionIndex = 0;
                //SetPropertyValue(property, selectionIndex);
                return;
            }

            // If a subclass is selected, loop through our valid types array to find the correct index.
            selectionIndex = selectionTypes.IndexOf(currentSubclass.GetType()) + 1;
            // If we fail to find the correct index, then throw an error that current subclass is invalid.
            if (selectionIndex == 0)
            {
                throw new ArrayTypeMismatchException("Could not find type " + currentSubclass + " in the type list.");
            }
        }


        /// <summary>
        /// Sets the value of the property to a given subclass instance specified by the type index.
        /// </summary>
        /// <param name="property">The property to set the value of.</param>
        /// <param name="typeIndex">The index of the subclass type to set to the property.</param>
        private void SetPropertyValue(SerializedProperty property, int typeIndex)
        {
            if (typeIndex == 0)
            {
                property.managedReferenceValue = null;
            }
            else
            {
                property.managedReferenceValue = Activator.CreateInstance(selectionTypes[typeIndex - 1]);
            }

            property.serializedObject.ApplyModifiedProperties();
        }
    }

}