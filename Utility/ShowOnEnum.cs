// ---------------------------------------------------------------------------------
#region File Info - ShowOnEnum.cs
// ---------------------------------------------------------------------------------
// Project:     Bounder Framework
// Created:     Sarah Herzog 2019
// Purpose:     Property drawer for conditionally hiding in the inspector
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
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class ShowOnEnum : PropertyAttribute
    {
        public string conditionalSourceField = "";
        public int compare = 0;

        public ShowOnEnum(string _conditionalSourceField, int _compare)
        {
            conditionalSourceField = _conditionalSourceField;
            compare = _compare;
        }
    }
    // -----------------------------------------------------------------------------
    #endregion
    // -----------------------------------------------------------------------------


    // -----------------------------------------------------------------------------
    #region PropertyDrawer: ShowOnEnumProperyDrawer
    // -----------------------------------------------------------------------------
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ShowOnEnum))]
    public class ShowOnEnumProperyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            ShowOnEnum showInInspector = (ShowOnEnum)attribute;
            bool enabled = ShouldShow(showInInspector, _property);
            bool wasEnabled = GUI.enabled;
            GUI.enabled = enabled;
            if (enabled)
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(_position, _property, _label, true);
                --EditorGUI.indentLevel;
            }

            GUI.enabled = wasEnabled;
        }

        private bool ShouldShow(ShowOnEnum _attribute, SerializedProperty _property)
        {
            bool enabled = true;
            string propertyPath = _property.propertyPath;
            string conditionPath = propertyPath.Replace(_property.name, _attribute.conditionalSourceField);
            SerializedProperty sourcePropertyValue = _property.serializedObject.FindProperty(conditionPath);

            if (sourcePropertyValue != null)
            {
                enabled = sourcePropertyValue.enumValueIndex == _attribute.compare;
            }
            else
            {
                Debug.LogWarning("Attempting to use ShowOnEnumValue but no matching SourcePropertyValue found in object: " + _attribute.conditionalSourceField);
            }

            return enabled;
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            ShowOnEnum showInInspector = (ShowOnEnum)attribute;
            bool enabled = ShouldShow(showInInspector, _property);

            if (enabled)
            {
                return EditorGUI.GetPropertyHeight(_property, _label);
            }
            else
            {
                return -EditorGUIUtility.standardVerticalSpacing;
            }
        }
    }
#endif
    // -----------------------------------------------------------------------------
    #endregion
    // -----------------------------------------------------------------------------

}