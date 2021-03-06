﻿// ************************************************************************ 
// File Name:   LayerSelecter.cs 
// Purpose:    	Allows the sorting layer of the object to be selected.
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2013 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports 
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
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: LayerSelecter
// ************************************************************************ 
[ExecuteInEditMode]
public class LayerSelecter : MonoBehaviour 
{
    // ********************************************************************
	#region Private Data Members 
    // ********************************************************************
	[SerializeField]
	[SortingLayerAttribute]
	private string m_sortingLayer = "Default";
	[SerializeField]
	private int m_orderInLayer = 0;
	#endregion
	// ********************************************************************
	
	
    // ********************************************************************
	#region MonoBehaviour Methods
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
	void Update () {
		if (GetComponent<Renderer>() != null)
		{
			GetComponent<Renderer>().sortingLayerName = m_sortingLayer;
			GetComponent<Renderer>().sortingOrder = m_orderInLayer;
		}
	}
	// ********************************************************************
	#endregion
	// ********************************************************************
}
// ************************************************************************
#endregion
// ************************************************************************