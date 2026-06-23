/*****************************************************************************
// File Name : VectorLabelsAttributeDrawer.cs
// Author : Arcadia Koederitz
// Creation Date : 1/28/2026
// Last Modified : 1/28/2026
//
// Brief Description : Drawer that replaces a Vector's default x, y, z, w labels with custom ones defined by an attrubute.
// Based on this unity forum post: https://discussions.unity.com/t/how-to-change-the-names-of-a-vector-3-that-is-set-in-the-inspector/216935/3
*****************************************************************************/
using System;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace CustomAttributes
{
    [CustomPropertyDrawer(typeof(VectorLabelsAttribute))]
    public class VectorLabelsAttributeEditor : PropertyDrawer
    {
        #region CONSTS
        private const int TWO_LINES_THRESHOLD = 375;
        private const float LABEL_MARGIN = 10;
        #endregion

        private static GUIContent[] defaultLabels = new GUIContent[] {
            new GUIContent("X"),
            new GUIContent("Y"),
            new GUIContent("Z"),
            new GUIContent("W")
        };

        /// <summary>
        /// Gets the height of this property, takinginto account if the labels wrap to 2 lines.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Caluclate if this property's height should be doubled due to taking up to lines.
            float multiplier = Screen.width < TWO_LINES_THRESHOLD ? 2 : 1;
            return multiplier * base.GetPropertyHeight(property, label);
        }

        /// <summary>
        /// Draws the vector property on the Editor.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            VectorLabelsAttribute vla = (VectorLabelsAttribute)attribute;

            // Each vector type needs specialized implementation.
            int[] iArray;
            float[] fArray;
            switch (property.propertyType)
            {
                case SerializedPropertyType.Vector2Int:
                    iArray = new int[] {property.vector2IntValue.x, property.vector2IntValue.y };
                    iArray = DrawFields(position, iArray, ObjectNames.NicifyVariableName(property.name), vla.Lables, EditorGUI.IntField);
                    property.vector2IntValue = new Vector2Int(iArray[0], iArray[1]);
                    break;
                case SerializedPropertyType.Vector3Int:
                    iArray = new int[] { property.vector3IntValue.x, property.vector3IntValue.y, property.vector3IntValue.z };
                    iArray = DrawFields(position, iArray, ObjectNames.NicifyVariableName(property.name), vla.Lables, EditorGUI.IntField);
                    property.vector3IntValue = new Vector3Int(iArray[0], iArray[1], iArray[2]);
                    break;
                case SerializedPropertyType.Vector2:
                    fArray = new float[] { property.vector2Value.x, property.vector2Value.y };
                    fArray = DrawFields(position, fArray, ObjectNames.NicifyVariableName(property.name), vla.Lables, EditorGUI.FloatField);
                    property.vector2Value = new Vector2(fArray[0], fArray[1]);
                    break;
                case SerializedPropertyType.Vector3:
                    fArray = new float[] { property.vector3Value.x, property.vector3Value.y, property.vector3Value.z };
                    fArray = DrawFields(position, fArray, ObjectNames.NicifyVariableName(property.name), vla.Lables, EditorGUI.FloatField);
                    property.vector3Value = new Vector3(fArray[0], fArray[1], fArray[2]);
                    break;
                case SerializedPropertyType.Vector4:
                    fArray = new float[] { property.vector4Value.x, property.vector4Value.y, property.vector4Value.z, property.vector4Value.w };
                    fArray = DrawFields(position, fArray, ObjectNames.NicifyVariableName(property.name), vla.Lables, EditorGUI.FloatField);
                    property.vector4Value = new Vector4(fArray[0], fArray[1], fArray[2], fArray[3]);
                    break;
                default:
                    base.OnGUI(position, property, label);
                    break;
            }
        }

        /// <summary>
        /// Draws the vector fields with custom labels.
        /// </summary>
        /// <typeparam name="T">The type of the vector (int or float)/typeparam>
        /// <param name="rect">The entire rect with which to draw the vector.</param>
        /// <param name="vector">The vector that is being drawn.</param>
        /// <param name="mainLabel">The main label of the vector.</param>
        /// <param name="vectorLabels">The labels for each value.</param>
        /// <param name="fieldDrawer">The function that draws the actual field.</param>
        /// <returns>The output vector value after editor modifications.</returns>
        private T[] DrawFields<T>(Rect rect, T[] vector, string mainLabel, string[] vectorLabels,  Func<Rect, GUIContent, T, T> fieldDrawer)
        {
            T[] result = vector;
            bool twoLinesLayout = Screen.width < TWO_LINES_THRESHOLD;

            // Create a rect for the main label of the variable.
            Rect mainLabelRect = rect;
            mainLabelRect.width = EditorGUIUtility.labelWidth;
            if (twoLinesLayout)
            {
                mainLabelRect.height /= 2;
            }

            // Create a rect for the individual vector fields.
            Rect fieldRect = rect;
            if(twoLinesLayout)
            {
                fieldRect.height /= 2;
                fieldRect.y += mainLabelRect.height;
                fieldRect.width = (rect.width - 2 * LABEL_MARGIN) / vector.Length;
            }
            else
            {
                fieldRect.x += mainLabelRect.width;
                fieldRect.width = (rect.width - mainLabelRect.width - 2 * LABEL_MARGIN) / vector.Length;
            }

            EditorGUI.LabelField(mainLabelRect, mainLabel);

            // Draw all the vector fields.
            for(int i = 0; i < vector.Length; i++)
            {
                GUIContent label = vectorLabels.Length > i ? new GUIContent(vectorLabels[i]) : defaultLabels[i];

                // Set width of labels.
                Vector2 labelSize = EditorStyles.label.CalcSize(label);
                EditorGUIUtility.labelWidth = Mathf.Max(labelSize.x + 5, 0.3f * fieldRect.width);

                result[i] = fieldDrawer(fieldRect, label, vector[i]);
                fieldRect.x += fieldRect.width + (2 * LABEL_MARGIN);
            }
            EditorGUIUtility.labelWidth = 0;
            return result;
        }
    }
}
