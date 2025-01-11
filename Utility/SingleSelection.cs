// ---------------------------------------------------------------------------------
#region File Info - SingleSelection.cs
// ---------------------------------------------------------------------------------
// Project:     Bounder Framework
// Created:     Sarah Herzog 2024
// Purpose:     Property drawer for forcing flag enums to only have one selection
// ---------------------------------------------------------------------------------
#endregion
// ---------------------------------------------------------------------------------
namespace Bounder.Framework
{


    // -----------------------------------------------------------------------------
    #region Libraries
    // -----------------------------------------------------------------------------
    using UnityEngine;
    using System;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    // -----------------------------------------------------------------------------
    #endregion
    // -----------------------------------------------------------------------------


    // -----------------------------------------------------------------------------
    #region PropertyAttribute: ShowOnEnum
    // -----------------------------------------------------------------------------
    public class SingleSelectionAttribute : PropertyAttribute
    {
        // Marker attribute for single selection.
    }
    // -----------------------------------------------------------------------------
    #endregion
    // -----------------------------------------------------------------------------


    // -----------------------------------------------------------------------------
    #region PropertyDrawer: ShowOnEnumProperyDrawer
    // -----------------------------------------------------------------------------
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SingleSelectionAttribute))]
    public class SingleSelectionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Enum)
            {
                // Use EnumPopup for single selection
                property.enumValueIndex = EditorGUI.Popup(position, label.text, property.enumValueIndex, property.enumDisplayNames);
            }
            else if (property.propertyType == SerializedPropertyType.Integer && fieldInfo.FieldType.IsDefined(typeof(FlagsAttribute), false))
            {
                // Handle Flags enums, enforce single selection
                Enum enumValue = (Enum)Enum.ToObject(fieldInfo.FieldType, property.intValue);
                string[] names = Enum.GetNames(fieldInfo.FieldType);
                int[] values = (int[])Enum.GetValues(fieldInfo.FieldType);

                int selectedIndex = Array.IndexOf(values, property.intValue);
                int newIndex = EditorGUI.Popup(position, label.text, selectedIndex, names);

                if (newIndex >= 0 && newIndex < values.Length)
                {
                    // Update to only the single selected value
                    property.intValue = values[newIndex];
                }
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use SingleSelection with Enum or Flags");
            }
        }
    }
#endif
    // -----------------------------------------------------------------------------
    #endregion
    // -----------------------------------------------------------------------------

}