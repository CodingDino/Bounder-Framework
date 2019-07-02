// ************************************************************************ 
// File Name:   SetActiveOnAnimationEvent.cs 
// Purpose:    	SetActive on a GameObject after an animation event
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
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


// ************************************************************************ 
// Class: ActivateOnAnimationEvent
// ************************************************************************ 
public class SetActiveOnAnimationEvent : MonoBehaviour {
	
	
	// ********************************************************************
	// Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private GameObject m_object = null;
	
	
	// ********************************************************************
	// Function:	Start()
	// Purpose:		Run when new instance of the object is created.
	// ********************************************************************
	public void SetActive (bool _active) {
		m_object.SetActive(_active);
	}
}