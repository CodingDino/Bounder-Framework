// ************************************************************************ 
// File Name:   Condition.cs 
// Purpose:    	A Condition that can be checked against a PlayerProfile
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
#region Class: Condition
// ************************************************************************
public class Condition : ScriptableObject 
{
	// ********************************************************************
	#region Class: Progress 
	// ********************************************************************
	[System.Serializable]
	public class Progress
	{
		public int progress = 0;
	}
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private int m_quantity = 1;
	#endregion
	// ********************************************************************



	// ********************************************************************
	#region Private Data Members 
	// ********************************************************************
	private Progress m_progress = null;
	private bool m_incrementalProgress = false;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Properties 
	// ********************************************************************
	public virtual int quantity { get { return m_quantity; } }
	public virtual Progress progress { get { return m_progress; } set { m_progress = value; m_incrementalProgress = true; } }
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Events 
	// ********************************************************************
	public delegate void ConditionTriggered(Condition _condition);
	public event ConditionTriggered OnConditionTriggered;
	public static event ConditionTriggered OnConditionTriggeredGlobal;
	public event ConditionTriggered OnConditionProgress;
	#endregion
	// ********************************************************************


	// ********************************************************************
	#region Public Functions 
	// ********************************************************************
	public virtual void Register(bool _register) 
	{
		// Register or deregister callbacks here to know when to check progress
	}
	// ********************************************************************
	public virtual bool Evaluate() 
	{ 
		return GetProgress() >= m_quantity; 
	}
	// ********************************************************************
	public virtual int GetProgress() 
	{
		// Retrieve saved progress, or Calculate from game state if cumulative
		if (m_progress != null && m_incrementalProgress)
			return m_progress.progress;
		return GetProgress_Cumulative();
	}
	// ********************************************************************
	public virtual int GetProgress_Cumulative() 
	{
		// Calculate from game state
		return 0;
	}
	// ********************************************************************
	public virtual void AddProgress(int _toAdd = 1) 
	{
		// Save to player profile here, if incremental
		if (m_progress != null && m_progress.progress < m_quantity)
		{
			m_progress.progress += _toAdd;
			if (OnConditionProgress != null)
				OnConditionProgress(this);
		}
	}
	// ********************************************************************
	public virtual void Trigger() 
	{
		if (OnConditionTriggered != null)
			OnConditionTriggered(this);
		if (OnConditionTriggeredGlobal != null)
			OnConditionTriggeredGlobal(this);
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
