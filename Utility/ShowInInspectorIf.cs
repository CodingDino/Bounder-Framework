// ************************************************************************ 
// File Name:   ShowInInspectorIf.cs 
// Purpose:    	Property drawer for conditionally hiding in the inspector
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
using System;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ShowInInspectorIf
// ************************************************************************ 
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ShowInInspectorIf : PropertyAttribute 
{
	public string conditionalSourceField = "";
	public bool compare = true;

	public ShowInInspectorIf(string _conditionalSourceField)
	{
		conditionalSourceField = _conditionalSourceField;
		compare = true;
	}

	public ShowInInspectorIf(string _conditionalSourceField, bool _compare)
	{
		conditionalSourceField = _conditionalSourceField;
		compare = _compare;
	}
}
// ************************************************************************
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: ShowInInspectorIfProperyDrawer
// ************************************************************************ 
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ShowInInspectorIf))]
public class ShowInInspectorIfProperyDrawer : PropertyDrawer 
{
	public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
	{
		ShowInInspectorIf showInInspector = (ShowInInspectorIf)attribute;
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

	private bool ShouldShow(ShowInInspectorIf _attribute, SerializedProperty _property)
	{
		bool enabled = true;
		string propertyPath = _property.propertyPath;
		string conditionPath = propertyPath.Replace(_property.name, _attribute.conditionalSourceField);
		SerializedProperty sourcePropertyValue = _property.serializedObject.FindProperty(conditionPath);

		if (sourcePropertyValue != null)
		{
			enabled = sourcePropertyValue.boolValue == _attribute.compare;
		}
		else
		{
			Debug.LogWarning("Attempting to use ShowInInspectorIf but no matching SourcePropertyValue found in object: "+_attribute.conditionalSourceField);
		}

		return enabled;
	}

	public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
	{
		ShowInInspectorIf showInInspector = (ShowInInspectorIf)attribute;
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
// ************************************************************************
#endregion
// ************************************************************************
