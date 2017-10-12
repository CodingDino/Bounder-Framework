// ************************************************************************ 
// File Name:   PlayerProfile.cs 
// Purpose:    	Base Player Profile for all games
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2017 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: PlayerProfile
// ************************************************************************
public class PlayerProfile : ScriptableObject 
{
	// ********************************************************************
	#region Public Data Members 
	// ********************************************************************
	public List<string> conversationsSeen = new List<string>();
	public List<string> choicesMade = new List<string>();
	public List<string> availableFeatures = new List<string>();
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Delegates 
	// ********************************************************************
	public delegate void FeatureBecameAvailable(string _featureID);
	public static event FeatureBecameAvailable OnFeatureBecameAvailable;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Data Members 
	// ********************************************************************
	public bool IsFeatureAvailable(string _featureID)
	{
		return availableFeatures.Contains(_featureID);
	}
	// ********************************************************************
	public bool TryAddFeature(string _featureID)
	{
		if (availableFeatures.Contains(_featureID))
		{
			return false;
		}
		else
		{
			availableFeatures.Add(_featureID);

			if (OnFeatureBecameAvailable != null)
				OnFeatureBecameAvailable(_featureID);

			return true;
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	#if UNITY_EDITOR
	private bool m_expanded = false;
	#endif
	#endregion
	// ********************************************************************


	// ********************************************************************
	#if UNITY_EDITOR
	// ********************************************************************
	public void DrawUI(SerializedProperty parentProperty)
	{
		SerializedObject serialized = new SerializedObject(this);
		serialized.Update();

		Rect outerRect = EditorGUILayout.BeginVertical("Box");
		EditorGUI.DrawRect(outerRect,Color.white);
		// create delete button for child
		{
			GUIStyle style = new GUIStyle();
			style.alignment = TextAnchor.MiddleRight;
			EditorGUILayout.BeginHorizontal(style);
			if (GUILayout.Button(m_expanded ? "-" : "+",GUILayout.Width(25)))
			{
				m_expanded = !m_expanded;
			}
			EditorGUILayout.PropertyField(parentProperty,false);
			EditorGUILayout.EndHorizontal();
		}
		++EditorGUI.indentLevel;
		if (m_expanded)
		{
			SerializedProperty property = serialized.GetIterator();
			bool enterChildren = true;
			while (property.NextVisible(enterChildren))
			{
				EditorGUILayout.PropertyField(property, !property.hasVisibleChildren || property.isExpanded);
				enterChildren = false;
			}
		}
		--EditorGUI.indentLevel;
		EditorGUILayout.EndVertical();

		serialized.ApplyModifiedProperties();
	}
	// ********************************************************************
	#endif
	// ********************************************************************

}
#endregion
// ************************************************************************
