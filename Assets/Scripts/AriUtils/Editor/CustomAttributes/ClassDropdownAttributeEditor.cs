/*****************************************************************************
// File Name : ClassDropdownAttributeDrawer.cs
// Author : Arcadia Koederitz
// Creation Date : 12/30/2025
// Last Modified : 12/30/2025
//
// Brief Description : Draws a custom dropdown for an abstract base class and any relevant serialized properties.
// Based on an article at: https://medium.com/@modyari/unity-workflow-customization-robust-custom-dropdowns-to-unlock-manageable-architecture-f16c183b993a 
*****************************************************************************/
using System;
using UnityEditor;
using UnityEngine;

namespace AriUtils.Editor
{
    [CustomPropertyDrawer(typeof(ClassDropdownAttribute))]
    public class ClassDropdownAttributeDrawer : PropertyDrawer
    {
        private EditorClassDropdown editorDropdown;

        /// <summary>
        /// Draws the class dropdown and relevant properties, if any.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Perform initialization if we haven't already.
            if (editorDropdown == null)
            {
                ClassDropdownAttribute dda = (attribute as ClassDropdownAttribute);
                Type baseType = fieldInfo.FieldType;
                bool ClassFilter(Type type)
                {
                    return EditorUtils.IsClassOrSubclass(baseType, type);
                }

                editorDropdown = new EditorClassDropdown(ClassFilter, property, dda.RestrictAssemblies ? baseType : null);
           }

            editorDropdown.DrawDropdown(position, position, property, property.displayName);
        }

        /// <summary>
        /// Specifies the total height taken up by this property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorUtils.GetDynamicPropertyHeight(property);
        }
    }
}


// Filter by allowed and excluded types.
//if (dda.ExcludedTypes != null)
//{
//    bool isExcluded = false;
//    foreach(Type et in dda.ExcludedTypes)
//    {
//        if (IsClassOrSubclass(et, type))
//        {
//            isExcluded = true;
//            break;
//        }
//    }
//    if (isExcluded)
//    {
//        continue;
//    }
//}
//if (dda.AllowedTypes != null)
//{
//    bool isAllowed = false;
//    foreach (Type at in dda.AllowedTypes)
//    {
//        if (IsClassOrSubclass(at, type))
//        {
//            isAllowed = true;
//            break;
//        }
//    }
//    if (!isAllowed)
//    {
//        continue;
//    }
//}