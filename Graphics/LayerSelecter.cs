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
}