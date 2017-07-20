// ************************************************************************ 
// File Name:   SortingLayerProperyDrawer.cs 
// Purpose:    	Property drawer for showing a sorting layer
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Reflection;
#endif
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: SortingLayerProperyDrawer
// ************************************************************************ 
public class SortingLayerAttribute : PropertyAttribute {}
// ************************************************************************
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: SortingLayerProperyDrawer
// ************************************************************************ 
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SortingLayerAttribute))]
public class SortingLayerProperyDrawer : PropertyDrawer 
{
	private int textHeight = 16;

	public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
	{
		EditorGUI.BeginProperty( _position, _label, _property);
		int origIndent = EditorGUI.indentLevel;
		Type utilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty = utilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		string[] values = (string[])sortingLayersProperty.GetValue(null,new object[0]);
		int index = 0;
		for(; index < values.Length -1; ++index )
		{
			if (values[index].Equals(_property.stringValue))
				break;
		}

		index = EditorGUI.Popup( new Rect (_position.xMin,
		                                   _position.yMin, 
		                                   _position.width,
		                                   textHeight),
		                        _property.displayName,
		                        index,
		                        values);

		if (index < values.Length)
			_property.stringValue = values[index];

		EditorGUI.indentLevel = origIndent;
		EditorGUI.EndProperty();
		
	}
}
// ************************************************************************
#endif
#endregion
// ************************************************************************
