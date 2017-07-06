// ************************************************************************ 
// File Name:   LayerSelecter.cs 
// Purpose:    	Allows the sorting layer of the object to be selected.
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2013 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Reflection;
#endif


// ************************************************************************ 
// Attributes 
// ************************************************************************ 
[ExecuteInEditMode]


// ************************************************************************ 
// Class: LayerSelecter
// ************************************************************************ 
public class LayerSelecter : MonoBehaviour {


    // ********************************************************************
    // Private Data Members 
    // ********************************************************************
	[SerializeField]
	private string m_sortingLayer = "Default";
	[SerializeField]
	private int m_orderInLayer = 0;
	 
	
	
    // ********************************************************************
    // Properties 
    // ********************************************************************
	
	
    // ********************************************************************
    // Function:	Start()
	// Purpose:		Run when new instance of the object is created.
    // ********************************************************************
	void Start () {
		if (GetComponent<Renderer>() != null)
		{
			GetComponent<Renderer>().sortingLayerName = m_sortingLayer;
			GetComponent<Renderer>().sortingOrder = m_orderInLayer;
		}
		if (Application.isPlaying)
			this.enabled = false;
	}
	
	
    // ********************************************************************
    // Function:	Update()
	// Purpose:		Called once per frame.
    // ********************************************************************
	void Update () {
		if (GetComponent<Renderer>() != null)
		{
			GetComponent<Renderer>().sortingLayerName = m_sortingLayer;
			GetComponent<Renderer>().sortingOrder = m_orderInLayer;
		}
	}


	// ********************************************************************
	#if UNITY_EDITOR
	// ********************************************************************
	public static void DrawSortingGUI(UnityEngine.Object _target, string _paramName, ref string _currentLayer)
	{
		SerializedObject serialized = new SerializedObject(_target);
		serialized.Update();

		// default - just draw objects
		SerializedProperty property = serialized.GetIterator();
		property.NextVisible(true);
		do
		{
			// Dropdown list for graphic layers
			if (property.name == _paramName)
			{
				Type utilityType = typeof(InternalEditorUtility);
				PropertyInfo sortingLayersProperty = utilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
				string[] sortingLayers = (string[])sortingLayersProperty.GetValue(null,new object[0]);
				// find current layer
				int currentLayer = sortingLayers.Length;
				for (int i = 0; i < sortingLayers.Length; ++i)
				{
					if (sortingLayers[i] == _currentLayer)
					{
						currentLayer = i;
					}
				}
				int newChoice = EditorGUILayout.Popup("Layer",currentLayer, sortingLayers);
				if (newChoice != currentLayer)
				{
					_currentLayer = sortingLayers[newChoice];
					EditorUtility.SetDirty(_target);
				}
			}
			else
			{
				EditorGUILayout.PropertyField(property, !property.hasVisibleChildren || property.isExpanded);
			}
		} while (property.NextVisible(false));

		serialized.ApplyModifiedProperties();
	}



	[CustomEditor(typeof(LayerSelecter))]
	public class Inspector : Editor
	{
		public override void OnInspectorGUI()
		{
			LayerSelecter layerSelecter = (LayerSelecter)target;
			DrawSortingGUI(target,"m_sortingLayer",ref layerSelecter.m_sortingLayer);
		}
	}
	// ********************************************************************
	#endif
	// ********************************************************************
}